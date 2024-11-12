using UnityEngine;
using UnityEngine.SceneManagement; // Для перезагрузки сцены или выхода из игры

public class EndGameTrigger : MonoBehaviour
{
    public bool isOver = false;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Игра завершена!");
        Application.Quit();
    }
}