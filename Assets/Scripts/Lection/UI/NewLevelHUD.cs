using Platformer;
using System.Collections;
using TMPro;
using UnityEngine;

// Класс управляющий интерфейсом на уровне
public partial class LevelHUD : MonoBehaviour
{
    // Ссылка на текстовый виджет в котором отображается счет игрока
    [SerializeField] private TMP_Text scoreText;
    // Создадим очередь команд
    public readonly UICommandQueue CommandQueue = new UICommandQueue();

    private void Start()
    {
        // При создании интерфейса запустим задачу которая
        // позволяет ассинхронно обрабатывать команды
        StartCoroutine(AsyncUpdate());
    }

    // Метод обработки команд из очереди
    private IEnumerator AsyncUpdate()
    {
        while (true)
        {
            // Получим новую команду из очереди
            if (CommandQueue.TryDequeueCommand(out var command))
            {
            }

            // Обязательно нужно сказать, что мы закончили обновление 
            // на этом цикле обновления движка
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // При уничтожении интерфейса остановим задачу
        StopAllCoroutines();
    }
}
