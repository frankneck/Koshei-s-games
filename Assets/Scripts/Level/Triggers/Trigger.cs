using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trigger : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject triggers;

    [Header("Rotation cube")]
    [SerializeField] private float rotationSpeed = 45f;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetSize = 12f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("New Postiton for y")]
    [SerializeField] private Vector3 newPosition;

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
    private TriggerManager triggerManager;

    private void Awake()
    {
        if (cubeRotationManager == null)
            cubeRotationManager = cube.GetComponent<CubeRotationManager>();

        if (tilemapManager == null)
            tilemapManager = cube.GetComponent<TilemapManager>();

        if (triggerManager == null)
            triggerManager = triggers.GetComponent<TriggerManager>();
    }
    
    void Start()
    {
        newPosition.z = -20f;

        for (int i = 1; i < tilemapManager.sides.Length; i++)
        {
            tilemapManager.sides[i].GetComponent<Collider2D>().enabled = false;
            tilemapManager.sides[i].GetComponent<Rigidbody2D>().simulated = false;
        }

        for (int i = 1; i < triggerManager.triggers.GetLength(0); i++)
        {
            for (int j = 0; j < triggerManager.triggers.GetLength(1); j++)
            {
                triggerManager.triggers[i, j].GetComponent<Collider2D>().enabled = false;
            }
        }
        initialSize = mainCamera.orthographicSize; // Запоминание позиции камеры
    }
    
    private void FixedUpdate()
    {
        if (shouldRotate)
        {
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

            triggerPerformed = true;
            shouldRotate = true;

			var direction = new Vector2(
                gameObject.transform.position.x,
				 gameObject.transform.position.y
            ).normalized;

            if (direction.x > 0.6f)
            {
				cubeRotationManager.Rotate(Vector3.up, Rotated);
			} else if (direction.x < -0.6f)
            {
				cubeRotationManager.Rotate(Vector3.down, Rotated);
			} else if (direction.y > 0.6f)
            {
				cubeRotationManager.Rotate(Vector3.left, Rotated);
			} else
			{
				cubeRotationManager.Rotate(Vector3.right, Rotated);
			}


			switch (this.gameObject.name)
            {
                // A
                // Left
                case "A7":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    
                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "A8":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Right
                case "A3":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "A4":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // С
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "A1":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "A2":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Down
                case "A5":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "A6":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер A");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                // F
                // Left
                case "F7":
                    UpdateRotationZplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "F8":
                    UpdateRotationZplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                // Right
                case "F3":
                    UpdateRotationZminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // С
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "F4":
                    UpdateRotationZminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // С
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "F1":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "F2":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                // Down
                case "F5":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "F6":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер F");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                // D
                // Left
                case "D7":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "D8":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Right
                case "D3":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "D4":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "D1":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "D2":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Down
                case "D5":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "D6":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // B
                // Left
                case "B7":
                    UpdateRotationZminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "B8":
                    UpdateRotationZminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[4]; // E
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Right
                case "B3":
                    UpdateRotationZplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "B4":
                    UpdateRotationZplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[2]; // C
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "B1":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "B2":
                    UpdateRotationXminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Down
                case "B5":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "B6":
                    UpdateRotationXplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер B");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // C
                // Left
                case "C7":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "C8":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Right
                case "C3":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "C4":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "C1":
                    UpdateRotationZminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "C2":
                    UpdateRotationZminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Down
                case "C5":
                    UpdateRotationZplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    //tilemapManager.sides[5].transform.rotation = Quaternion.Euler(0, 90, 0);
                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                case "C6":
                    UpdateRotationZplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер С");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    //tilemapManager.sides[5].transform.rotation = Quaternion.Euler(0, 90, 0);
                    //tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
                    //tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;
                    break;
                // E
                // Left
                case "E7":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "E8":
                    UpdateRotationYminus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Левый триггер D");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[3]; // D
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Right
                case "E3":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "E4":
                    UpdateRotationYplus();
                    newPosition.y = player.transform.position.y;

                    Debug.Log("Правый триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[0]; // A
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Up
                case "E1":
                    UpdateRotationZplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "E2":
                    UpdateRotationZplus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Верхний триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[1]; // B
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                // Down
                case "E5":
                    UpdateRotationZminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                case "E6":
                    UpdateRotationZminus();
                    newPosition.x = player.transform.position.x;

                    Debug.Log("Нижний триггер E");
                    tilemapManager.lastTilemap = tilemapManager.currentTilemap;
                    tilemapManager.currentTilemap = tilemapManager.sides[5]; // F
                    tilemapManager.currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    tilemapManager.lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                default:
                    break;
            }
            CharacterOff(player);
        }
    }
    
    private void Rotated()
    {
        tilemapManager.currentTilemap.transform.GetComponent<Rigidbody2D>().simulated = true;
        tilemapManager.lastTilemap.transform.GetComponent<Rigidbody2D>().simulated = false;

		shouldRotate = false;
		if (player != null)
        {
            player.transform.position = newPosition;
            CharacterOn(player);
            shouldRotate = false;

            switch (this.gameObject.name)
            {
                // A
                // Left
                case "A7":
                    SwitchTriggers(4);
                    break;
                case "A8":
                    SwitchTriggers(4);
                    break;
                // Right
                case "A3":
                    SwitchTriggers(2);
                    break;
                case "A4":
                    SwitchTriggers(2);
                    break;
                // Up
                case "A1":
                    SwitchTriggers(1);
                    break;
                case "A2":
                    SwitchTriggers(1);
                    break;
                // Down
                case "A5":
                    SwitchTriggers(5);
                    break;
                case "A6":
                    SwitchTriggers(5);
                    break;
                // F
                // Left
                case "F7":
                    SwitchTriggers(4);
                    break;
                case "F8":
                    SwitchTriggers(4);
                    break;
                // Right
                case "F3":
                    SwitchTriggers(2);
                    break;
                case "F4":
                    SwitchTriggers(2);
                    break;
                // Up
                case "F1":
                    SwitchTriggers(0);
                    break;
                case "F2":
                    SwitchTriggers(0);
                    break;
                // Down
                case "F5":
                    SwitchTriggers(3);
                    break;
                case "F6":
                    SwitchTriggers(3);
                    break;
                // D
                // Left
                case "D7":
                    SwitchTriggers(2);
                    break;
                case "D8":
                    SwitchTriggers(2);
                    break;
                // Right
                case "D3":
                    SwitchTriggers(4);
                    break;
                case "D4":
                    SwitchTriggers(4);
                    break;
                // Up
                case "D1":
                    SwitchTriggers(1);
                    break;
                case "D2":
                    SwitchTriggers(1);
                    break;
                // Down
                case "D5":
                    SwitchTriggers(5);
                    break;
                case "D6":
                    SwitchTriggers(5);
                    break;
                // B
                // Left
                case "B7":
                    SwitchTriggers(4);
                    break;
                case "B8":
                    SwitchTriggers(4);
                    break;
                // Right
                case "B3":
                    SwitchTriggers(2);
                    break;
                case "B4":
                    SwitchTriggers(2);
                    break;
                // Up
                case "B1":
                    SwitchTriggers(5);
                    break;
                case "B2":
                    SwitchTriggers(5);
                    break;
                // Down
                case "B5":
                    SwitchTriggers(0);
                    break;
                case "B6":
                    SwitchTriggers(0);
                    break;
                // C
                // Left
                case "C7":
                    SwitchTriggers(0);
                    break;
                case "C8":
                    SwitchTriggers(0);
                    break;
                // Right
                case "C3":
                    SwitchTriggers(3);
                    break;
                case "C4":
                    SwitchTriggers(3);
                    break;
                // Up
                case "C1":
                    SwitchTriggers(1);
                    break;
                case "C2":
                    SwitchTriggers(1);
                    break;
                // Down
                case "C5":
                    SwitchTriggers(5);
                    break;
                case "C6":
                    SwitchTriggers(5);
                    break;
                // E
                // Left
                case "E7":
                    SwitchTriggers(3);
                    break;
                case "E8":
                    SwitchTriggers(3);
                    break;
                // Right
                case "E3":
                    SwitchTriggers(0);
                    break;
                case "E4":
                    SwitchTriggers(0);
                    break;
                // Up
                case "E1":
                    SwitchTriggers(1);
                    break;
                case "E2":
                    SwitchTriggers(1);
                    break;
                // Down
                case "E5":
                    SwitchTriggers(5);
                    break;
                case "E6":
                    SwitchTriggers(5);
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateRotationYplus()
    {
        
    }

    private void UpdateRotationYminus()
    {
		
	}

    private void UpdateRotationXplus()
    {
		
	}
    private void UpdateRotationXminus()
    {
		
	}
    private void UpdateRotationZplus()
    {
		
	}

    private void UpdateRotationZminus()
    {
		
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
    private void SwitchTriggers(int strNumber)
    {
        for (int i = 0; i < triggerManager.lastTriggers.Length; i++)
        {
            triggerManager.lastTriggers[i] = triggerManager.currentTriggers[i];
        }
        for (int i = 0; i < triggerManager.currentTriggers.Length; i++)
        {
            triggerManager.currentTriggers[i] = triggerManager.triggers[strNumber, i];
        }
        for (int i = 0; i < triggerManager.currentTriggers.Length; i++)
        {
            triggerManager.currentTriggers[i].transform.GetComponent<Collider2D>().enabled = true;
        }
        for (int i = 0; i < triggerManager.lastTriggers.Length; i++)
        {
            triggerManager.lastTriggers[i].transform.GetComponent<Collider2D>().enabled = false;
        }
    }
}
