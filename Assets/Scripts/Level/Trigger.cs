using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // cube
    [SerializeField] private Vector3 newPosition; // new posis 

    private Quaternion targetRotation; // к чему придем
    private int rotate;

    private Vector3 lastPostion;
    private Player player;
   
    Vector2[] directions = { Vector2.down, Vector2.right, Vector2.left };

    private void Start()
    {
        rotate = 90;
        UpdateTargetRotation();
    }

    public void OnTriggerExit2D(Collider2D collision)
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
            targetObject.rotation = Quaternion.RotateTowards(targetObject.rotation, targetRotation, Time.deltaTime * 4f);
            yield return null;
        }

        if (player != null)
        {
            lastPostion = player.transform.position;
            newPosition.x = lastPostion.x;
            if (lastPostion.z == -12f)
                newPosition.z = lastPostion.z - 1f;
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
