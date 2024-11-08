using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trigger : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject yJoint;

    [Header("Rotation cube")]
    [SerializeField] private float rotationSpeed = 45f;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetSize = 12f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("New Postiton for y")]
    [SerializeField] private Vector3 newPosition;

    // Rotation of cube
    private Transform targetObjectCube;
    private Transform targetObjectYjoint;
    private Quaternion targetRotationY;
    private Quaternion targetRotationX;
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
        for (int i = 1; i < tilemapManager.sides.Length; i++)
            tilemapManager.sides[i].GetComponent<Collider2D>().enabled = false;

        targetObjectCube = cube.GetComponent<Transform>();
        targetObjectYjoint = yJoint.GetComponent<Transform>();

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

            Debug.Log($"shouldRotate: {shouldRotate}, rotateX: {cubeRotationManager.rotateX}, rotateY: {cubeRotationManager.rotateY}, targetRotation: {targetRotationY.eulerAngles}");

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
        if (this.gameObject.tag == "Right" || this.gameObject.tag == "Left")
        {
            targetObjectCube.rotation = Quaternion.RotateTowards(targetObjectCube.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(targetObjectCube.rotation, targetRotationY) < 0.1f || Quaternion.Angle(targetObjectYjoint.rotation, targetRotationY) < 0.1f)
            {
                targetObjectCube.rotation = targetRotationY;

                if (player != null)
                {
                    player.transform.position = newPosition;
                    CharacterOn(player);
                    shouldRotate = false;
                }
            }
        }
        else if (this.gameObject.tag == "Down")
        {
            targetObjectYjoint.rotation = Quaternion.RotateTowards(targetObjectYjoint.rotation, targetRotationX, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(targetObjectCube.rotation, targetRotationX) < 0.1f || Quaternion.Angle(targetObjectYjoint.rotation, targetRotationX) < 0.1f)
            {
                targetObjectYjoint.rotation = targetRotationX;

                if (player != null)
                {
                    player.transform.position = newPosition;
                    CharacterOn(player);
                    shouldRotate = false;
                }
            }
        }
    }
    
    private void UpdateRotationY()
    {
        if (this.gameObject.CompareTag("Left"))
        {
            cubeRotationManager.rotateY += -90;
        }
        else if (this.gameObject.CompareTag("Right"))
        {
            cubeRotationManager.rotateY += 90;
        }
        targetRotationY = Quaternion.Euler(0, cubeRotationManager.rotateY, 0);
    }
    
    private void UpdateRotationX()
    {
        cubeRotationManager.rotateX += 90;
        targetRotationX = Quaternion.Euler(cubeRotationManager.rotateX, 0, 0);
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
