using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance;
    public static SQLManager SQLmanager;
    
    public Supervisor Supervisor;
    public Session Session;
    public Player Player;
    public FlyingGame FlyingGame;

    void Awake()
    {
        // This way we make sure there is only ONE instance, the first one created in the program.
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
