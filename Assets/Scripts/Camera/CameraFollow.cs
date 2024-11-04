using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cube;
    [SerializeField] private float minX, minY, maxX, maxY;

    private Vector3 offset;
    private float lastMinY, lastMaxY;

    private void Start()
    {
        lastMinY = minY;
        lastMaxY = maxX;
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        float yRotation = cube.transform.rotation.eulerAngles.y;

        if (!(yRotation == 0) || (yRotation == 90))
        {
            minY = -6.03f;
            maxY = 7.02f;
        }
        else
        {
            minY = lastMinY;
            maxX = lastMaxY;
        }

        Vector3 targetPostion = player.position + offset;

        float clampedX = Mathf.Clamp(targetPostion.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPostion.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, targetPostion.z);
    }
}
