using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Rotation cube")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private float rotationSpeed = 45f;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetSize = 12f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("SpawnCharacter")]
    [SerializeField] private Vector3 newPosition;

    private bool shouldRotate = false;
    private Quaternion targetRotation;
    private Player player;
    private bool shouldChangeCamera;
    private bool isZooming = false;
    private float initialSize;

    private void Start()
    {
        targetRotation = Quaternion.Euler(90, 0, 0);
        initialSize = mainCamera.orthographicSize;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var collidedPlayer))
        {
            shouldChangeCamera = true;
            player = collidedPlayer; // Сохраняем ссылку на игрока
            shouldRotate = true;
            
            newPosition.x = player.transform.position.x;
            newPosition.z = player.transform.position.z;


            CharacterOff();  // Отключаем персонажа и его коллизию, делаем кинематичным

            if (newPosition.z == -12f)
            {
                Debug.Log("12f");
                newPosition.z = -13f;
            }
                
            else if (newPosition.z == -13f)
            {
                Debug.Log("13f");
                newPosition.z = -12f;   
            }
                
        }
    }

    private void Update()
    {
        if (shouldRotate)
        {
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
                shouldRotate = false;

                if (player != null)
                {
                    player.transform.position = newPosition;  // Даем персонажу новую позицию
                    CharacterOn(player);
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

    private void CharacterOff()
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
}
