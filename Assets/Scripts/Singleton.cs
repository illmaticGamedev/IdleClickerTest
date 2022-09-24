using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T Instance = null;

    public  void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
    }
}
