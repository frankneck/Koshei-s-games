using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trigger : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject cube;  // Сам куб

    [Header("Rotation cube")]
    [SerializeField] private float rotationSpeed = 45f;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetSize = 12f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("New Postiton for y")]
    [SerializeField] private Vector3 newPosition;

    // Rotation of cube
    private Transform targetObject;  // Присвоим значение куба
    private Quaternion targetRotation;
    public bool shouldRotate = false;
    private float rotateX = 0f;
    private float rotateY = 0f;

    // Camera
    private float initialSize;
    private bool isZooming = false;

    // Player
    private Player player;


    //Other
    private bool triggerPerformed = false;

    // Tilemap
    private Tilemap[] sides;
    private Tilemap currentTilemap;
    private Tilemap lastTilemap;

    private void Awake()
    {
        sides = new Tilemap[6];

        sides[0] = cube.transform.Find("A/FrontA").GetComponent<Tilemap>();  //A
        sides[1] = cube.transform.Find("B/FrontB").GetComponent<Tilemap>();  //B
        sides[2] = cube.transform.Find("C/FrontC").GetComponent<Tilemap>();  //C
        sides[3] = cube.transform.Find("D/FrontD").GetComponent<Tilemap>();  //D
        sides[4] = cube.transform.Find("E/FrontE").GetComponent<Tilemap>();  //E
        sides[5] = cube.transform.Find("F/FrontF").GetComponent<Tilemap>();  //F
    }

    void Start()
    {
        // Настройка изначальной tilemap - A
        currentTilemap = sides[0];
        lastTilemap = currentTilemap;

        // Отключение всех тайлмапов кроме A
        for (int i = 2; i < sides.Length; i++)
            sides[i].GetComponent<Collider2D>().enabled = false;

        targetObject = cube.GetComponent<Transform>();  // Объект, который будем двигать
        targetRotation = Quaternion.Euler(0, 0, 0); // Насколько двигаем объект
       
        initialSize = mainCamera.orthographicSize; // Запоминание позиции камеры
    }

    private void FixedUpdate()
    {
        if (shouldRotate)
        {
            Rotate();
            ZoomCamera();
        }
        else if (isZooming)
        {
            ResetCameraZoom();
        }
        
        if (triggerPerformed)
        {
            DefineCurrentSide();
            triggerPerformed = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var collidedPlayer))
        {
            player = collidedPlayer;

            Debug.Log($"Triggered by: {this.gameObject.tag}");
            Debug.Log($"shouldRotate: {shouldRotate}, rotateX: {rotateX}, rotateY: {rotateY}, targetRotation: {targetRotation.eulerAngles}");

            triggerPerformed = true;
            shouldRotate = true;
            if (this.gameObject.CompareTag("Down"))
            {
                UpdateRotationX();
                Debug.Log("Произошло вращение по X");

                newPosition.x = player.transform.position.x;
                newPosition.z = player.transform.position.z;
            }
            else if (this.gameObject.CompareTag("Left") || this.gameObject.CompareTag("Right"))
            {
                UpdateRotationY();
                Debug.Log("Произошло вращение по Y");

                newPosition.y = player.transform.position.y;
                newPosition.z = player.transform.position.z;
            }




            CharacterOff(player);
        }
    }

    private void CharacterOff(Player player) // Отключение персонажа
    {
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Collider2D>().enabled = false;
    }

    private void CharacterOn(Player player) // Включение персонажа
    {
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
    }

    private void UpdateRotationX()
    {
        rotateX += 90;
        targetRotation = Quaternion.Euler(rotateX, rotateY, 0);
        Debug.Log($"Updated rotateX: {rotateX}, New targetRotation: {targetRotation.eulerAngles}");
    }

    private void UpdateRotationY()
    {
        if (this.gameObject.CompareTag("Left"))
        {
            rotateY -= 90;
            Debug.Log("Left Trigger: Updated rotateY");
        }
        else if (this.gameObject.CompareTag("Right"))
        {
            rotateY += 90;
            Debug.Log("Right Trigger: Updated rotateY");
        }
        targetRotation = Quaternion.Euler(rotateX, rotateY, 0);
    }

    private void ResetCameraZoom()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, initialSize, Time.deltaTime * zoomSpeed);

        if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
        {
            isZooming = false;
            mainCamera.orthographicSize = initialSize;
        }
    }

    private void ZoomCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0.89375f, -6.14f, -29.35f), Time.deltaTime * rotationSpeed);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
        {
            isZooming = true;
            mainCamera.orthographicSize = targetSize;
        }
    }

    private void Rotate()
    {
        targetObject.rotation = Quaternion.RotateTowards(targetObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(targetObject.rotation, targetRotation) < 0.1f)
        {
            targetObject.rotation = targetRotation;

            if (player != null)
            {
                player.transform.position = newPosition;  // Даем персонажу новую позицию
                CharacterOn(player);
                shouldRotate = false;
            }
        }
    }

    private void DefineCurrentSide()
    {
        Debug.Log($"Текущая тайлмап: {currentTilemap.name}");
        Debug.Log($"Текущий тег: {this.gameObject.tag == "Down"}");

        switch (System.Array.IndexOf(sides, currentTilemap))
        {
            // For A
            case 0: 
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер A");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[5]; // F
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер A");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[4]; // E
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if(this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер A");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[2]; // C
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For B
            case 1:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[0]; // A
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[4]; // E
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[2]; // C
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;
            // For C
            case 2:
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[5]; // F
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[0]; // A
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер B");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[3]; // D
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For D
            case 3:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[1]; // B
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[2]; // C
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[4]; // E
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;
            // For E
            case 4:
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[5]; // F
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[3]; // D
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер D");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[0]; // A
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For F
            case 5:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер F");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[3]; // D
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер F");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[4]; // E
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер F");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[2]; // C
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            default:
                Debug.LogWarning("Сторона не обработана.");
                break;
        }
    }

}
