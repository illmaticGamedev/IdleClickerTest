using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace justDice_IdleClickerTest
{
    public sealed class DataManager : MonoBehaviour
    {
        public static PlayerData GamePlayerData;
        private string playerDataSavePath;
   
        
        // current In Game stats
        private void Awake()
        {
            playerDataSavePath = Path.Combine(Application.persistentDataPath, "playerData.bin");
            GamePlayerData = new PlayerData();
        }

        /// <summary>
        /// Saving data with binary formatter instead of the unity player pref system to avoid basic player hacks.
        /// </summary>
        public void SavePlayerData(long gold, int level, float attackerBuyCost)
        {
            GamePlayerData.Gold = gold;
            GamePlayerData.Level = level;
            GamePlayerData.AttackerBuyCost = attackerBuyCost;

            Stream saveStream = new FileStream(playerDataSavePath, FileMode.Create);
            BinaryFormatter binarySaver = new BinaryFormatter();
            binarySaver.Serialize(saveStream, GamePlayerData);
            saveStream.Close();
        }
        
        public void SaveAttackerData(int attackerIndex, int level, float attackerUpgradeCost, float goldPerAttack)
        {
            AttackerData newAttacker = new AttackerData();
            newAttacker.Level = level;
            newAttacker.UpgradeCost = attackerUpgradeCost;
            newAttacker.GoldPerAttack = goldPerAttack;
            
            Stream saveStream = new FileStream(Path.Combine(Application.persistentDataPath, "attacker_" + attackerIndex + ".bin"), FileMode.Create);
            BinaryFormatter binarySaver = new BinaryFormatter();
            binarySaver.Serialize(saveStream, newAttacker);
            saveStream.Close();
        }
        
        public PlayerData LoadPlayerData()
        {
            if (File.Exists(playerDataSavePath))
            {
                Stream loadStream = new FileStream(playerDataSavePath, FileMode.Open);
                BinaryFormatter binaryLoader = new BinaryFormatter();
                GamePlayerData = (PlayerData)binaryLoader.Deserialize(loadStream);
                loadStream.Close();
            }

            return GamePlayerData;
        }

        public AttackerData LoadAttackerData(int attackerIndex)
        {
            AttackerData attackerData = new AttackerData();
            
            if (File.Exists(Path.Combine(Application.persistentDataPath, "attacker_" + attackerIndex + ".bin")))
            {
                Stream loadStream = new FileStream(Path.Combine(Application.persistentDataPath, "attacker_" + attackerIndex + ".bin"), FileMode.Open);
                BinaryFormatter binaryLoader = new BinaryFormatter();
                attackerData = (AttackerData)binaryLoader.Deserialize(loadStream);
                loadStream.Close();
            }

            return attackerData;
        }
        
        [ContextMenu("DELETE ALL SAVED DATA")]
        public void DeleteAllSavedData()
        {
            //Adding else to repeat the path so ContextMenu callback works without running the game.
            if (File.Exists(playerDataSavePath))
            {
                File.Delete(playerDataSavePath);
            }

            for (int i = 1; i < 6; i++)
            {
                if (File.Exists(Path.Combine(Application.persistentDataPath, "attacker_" + i + ".bin")))
                {
                    File.Delete(Path.Combine(Application.persistentDataPath, "attacker_" + i + ".bin"));
                }
            }
    
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Game");
        }

    }
    
    [Serializable]
    public class PlayerData
    {
        public int  Level;
        public long Gold;
        public float AttackerBuyCost;
    }

    [Serializable]
    public class AttackerData
    {
        public int Level;
        public float UpgradeCost;
        public float GoldPerAttack;
    }
}