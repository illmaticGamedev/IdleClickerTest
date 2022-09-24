using UnityEngine;

namespace justDice_IdleClickerTest
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T Instance = null;
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }
    }
}