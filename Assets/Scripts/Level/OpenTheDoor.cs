using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    
    private Keys keys;

    private void Start()
    {
        if (keys == null)
            keys = transform.GetComponent<Keys>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            switch (collision.name)
            {
                case "red door":
                    if (keys.redKey)
                        collision.gameObject.SetActive(false);
                    break;
                case "blue door":
                    if (keys.blueKey)
                        collision.gameObject.SetActive(false);
                    break;
                case "purple door":
                    if (keys.purpleKey)
                        collision.gameObject.SetActive(false);
                    break;
                case "green door":
                    if (keys.greenKey)
                        collision.gameObject.SetActive(false);
                    break;
                case "white door":
                    if (keys.whiteKey)
                        collision.gameObject.SetActive(false);
                    break;
                case "yellow door":
                    if (keys.yelllowKey)
                        collision.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}