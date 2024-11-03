using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minX, minY, maxX, maxY;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPostion = player.position + offset;

        float clampedX = Mathf.Clamp(targetPostion.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPostion.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, targetPostion.z);

    }



}
