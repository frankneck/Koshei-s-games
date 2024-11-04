using Unity.VisualScripting;
using UnityEngine;

public class DownTrigger : MonoBehaviour
{
    [Header("Rotation cube")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private float rotationSpeed = 45f;
    private float rotate = 90f;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetSize = 12f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("SpawnCharacter")]
    [SerializeField] private Vector3 newPosition;

    public bool shouldRotate = false;
    private Quaternion targetRotation;
    private Player player;
    private bool isZooming = false;
    private float initialSize;
    public bool isDownTrigger = false;


    private void Start()
    {
        targetRotation = Quaternion.Euler(rotate, 0, 0);
        initialSize = mainCamera.orthographicSize;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var collidedPlayer))
        {
            isDownTrigger = true;
        }
            
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var collidedPlayer))
        {
            shouldRotate = true;
            player = collidedPlayer; // Сохраняем ссылку на игрока
            
            newPosition.x = player.transform.position.x;
            newPosition.z = player.transform.position.z;


            CharacterOff(player);  // Отключаем персонажа и его коллизию, делаем кинематичным

            if (newPosition.z == -12f)
            {
                newPosition.z = -13f;
            }
        }
    }

    private void Update()
    {
        if (shouldRotate)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0.89375f, -6.14f, -29.35f), Time.deltaTime * rotationSpeed);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
            {
                isZooming = true;
                mainCamera.orthographicSize = targetSize;
            }

            targetObject.rotation = Quaternion.RotateTowards(targetObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(targetObject.rotation, targetRotation) < 0.1f)
            {
                targetObject.rotation = targetRotation;

                if (player != null)
                {
                    player.transform.position = newPosition;  // Даем персонажу новую позицию
                    CharacterOn(player);

                    UpdateRotation();
                    shouldRotate = false;                    
                    isDownTrigger = false;
                }
            }
        }
        else if (isZooming)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, initialSize, Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
            {
                isZooming = false;
                mainCamera.orthographicSize = initialSize;
            }
        }
    }


    private void CharacterOff(Player player)
    {
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Collider2D>().enabled = false;
    }

    private void CharacterOn(Player player)
    {
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
    }

    private void UpdateRotation()
    {
        rotate += 90;
        targetRotation = Quaternion.Euler(rotate, 0, 0);
    }
}
