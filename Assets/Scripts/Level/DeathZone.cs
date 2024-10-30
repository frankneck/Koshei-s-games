using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 newPosition;

    private bool shouldRotate;
    private Quaternion targetRotation;
    private Vector3 lastPostion;
    private Player player;
    private int rotate;

    private void Start()
    {
        rotate = 90;
        UpdateTargetRotation();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out player))
        {
            StartCoroutine(RotateAndTeleport());
        }
    }

    private IEnumerator RotateAndTeleport()
    {
        while (Quaternion.Angle(targetObject.rotation, targetRotation) > 0.1f)
        {
            targetObject.rotation = Quaternion.Lerp(targetObject.rotation, targetRotation, Time.deltaTime * 2f);
            yield return null;
        }

        if (player != null)
        {
            lastPostion = player.transform.position;
            newPosition.x = lastPostion.x;
            player.transform.position = newPosition;
        }

        rotate += 90;
        UpdateTargetRotation();
    }


    private void UpdateTargetRotation()
    {
        targetRotation = Quaternion.Euler(rotate, 0, 0);
    }
}
