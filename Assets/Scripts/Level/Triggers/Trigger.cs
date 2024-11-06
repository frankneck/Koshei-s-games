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

    // Camera
    private float initialSize;
    private bool isZooming = false;

    // Player
    private Player player;

    //Other
    private bool triggerPerformed = false;
    
    // Managers
    private CubeRotationManager cubeRotationManager;
    private TilemapManager tilemapManager;

    private void Awake()
    {
        if (cubeRotationManager == null)
            cubeRotationManager = cube.GetComponent<CubeRotationManager>();

        if (tilemapManager == null)
            tilemapManager = cube.GetComponent<TilemapManager>();
    }
    void Start()
    {
        for (int i = 2; i < tilemapManager.sides.Length; i++)
            tilemapManager.sides[i].GetComponent<Collider2D>().enabled = false;

        targetObject = cube.GetComponent<Transform>();
       
        initialSize = mainCamera.orthographicSize; // Запоминание позиции камеры
    }
    private void FixedUpdate()
    {
        Debug.Log($"FixedUpdate Start - rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}");

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

        Debug.Log($"FixedUpdate End - rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}");
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var collidedPlayer))
        {
            player = collidedPlayer;

            Debug.Log($"shouldRotate: {shouldRotate}, rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}, targetRotation: {targetRotation.eulerAngles}");

            triggerPerformed = true;
            shouldRotate = true;
            if (this.gameObject.CompareTag("Down"))
            {
                UpdateRotationX();

                newPosition.x = player.transform.position.x;
                newPosition.z = player.transform.position.z;
            }
            if (this.gameObject.CompareTag("Left") || this.gameObject.CompareTag("Right"))
            {
                UpdateRotationY();

                newPosition.y = player.transform.position.y;
                newPosition.z = player.transform.position.z;
            }
            CharacterOff(player);
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
    private void UpdateRotationY()
    {
        Debug.Log($"Before UpdateRotationY - rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}");

        if (this.gameObject.CompareTag("Left"))
        {
            cubeRotationManager.rotateY -= 90;
            //cubeRotationManager.yRotationChange = Quaternion.AngleAxis(-90, Vector3.left);
        }
        else if (this.gameObject.CompareTag("Right"))
        {
            cubeRotationManager.rotateY += 90;
            //cubeRotationManager.yRotationChange = Quaternion.AngleAxis(90, Vector3.left);
        }
        targetRotation = Quaternion.Euler(cubeRotationManager.rotateX, cubeRotationManager.rotateY, 0);
        //targetRotation *= cubeRotationManager.yRotationChange;

        Debug.Log($"After UpdateRotationY - rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}, targetRotation: {targetRotation.eulerAngles}");
    }
    private void UpdateRotationX()
    {
        cubeRotationManager.rotateX += 90;
        targetRotation = Quaternion.Euler(cubeRotationManager.rotateX, cubeRotationManager.rotateY, 0);
        Debug.Log($"UpdateRotationX - Updated rotateX: {cubeRotationManager.rotateX}, New targetRotation: {targetRotation.eulerAngles}");

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
    private void ResetCameraZoom()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, initialSize, Time.deltaTime * zoomSpeed);

        if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
        {
            isZooming = false;
            mainCamera.orthographicSize = initialSize;
        }
    }
    private void DefineCurrentSide()
    {
        Debug.Log($"Текущая тайлмап: {tilemapManager.currentTilemap.name}");
        Debug.Log($"Текущий тег: {this.gameObject.tag == "Down"}");

        switch (System.Array.IndexOf(tilemapManager.sides, tilemapManager.currentTilemap))
        {
            // For A
            case 0: 
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if(this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For B
            case 1:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;
            // For C
            case 2:
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер C");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер C");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер C");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For D
            case 3:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;
            // For E
            case 4:
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            // For F
            case 5:     
                if (this.gameObject.tag == "Down")
                {
                    Debug.Log("Нижний триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Left")
                {
                    Debug.Log("Левый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                else if (this.gameObject.tag == "Right")
                {
                    Debug.Log("Правый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                break;

            default:
                Debug.LogWarning("Сторона не обработана.");
                break;
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
}
