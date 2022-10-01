using UnityEngine;
using UnityEngine.Serialization;

namespace justDice_IdleClickerTest
{
    public sealed class Managers : Singleton<Managers>
    {
        [Header("Components")]
        public UiManager uIManager;
        public GameManager gameManager;
        public Effects effectsManager;
        public RemoteConfig configManager;
        public DataManager dataManager;
        public AttackerManager attackerManager;
    }
}