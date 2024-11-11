using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    //[SerializeField] private Player player;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Player>() != null)
    //    {
    //        Debug.Log("Collision detected with: " + collision.gameObject.name); // Отладка
    //        if (levelManager.keysList.Count > 0)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var player))
        {
            gameObject.SetActive(false);
        }
    }

}