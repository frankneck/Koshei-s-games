using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    [SerializeField] public bool yelllowKey = false;
    [SerializeField] public bool purpleKey = false;
    [SerializeField] public bool whiteKey = false;
    [SerializeField] public bool blueKey = false;
    [SerializeField] public bool greenKey = false;
    [SerializeField] public bool redKey = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            switch (collision.name)
            {
                case "green key":
                    greenKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                case "yellow key":
                    yelllowKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                case "purple key":
                    purpleKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                case "red key":
                    redKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                case "white key":
                    whiteKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                case "blue key":
                    blueKey = true;
                    collision.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}