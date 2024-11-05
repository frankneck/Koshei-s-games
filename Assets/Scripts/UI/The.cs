using UnityEngine;

public class The : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject characterPrefab1;
    [SerializeField] private GameObject characterPrefab2;

    public static The Instance { get; private set; }

    //public GameObject characterPrefab1 => characterPrefab1;
    //public GameObject characterPrefab2 => characterPrefab2;
    //public GameObject characterPrefab3 => characterPrefab3;

    void Start()
    {
        Instance??= this;
        
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
