using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace justDice_IdleClickerTest
{
    public class RemoteConfig : MonoBehaviour
    {
        private string fileURL = "https://drive.google.com/uc?export=download&id=12yER5n35P1xNOwIWBPXqiWilHee9wY57";
        
        void Start()
        {
            string jsonTextRow = new StreamReader(Application.dataPath + "/Scripts/RemoteConfigJustDice.txt").ReadToEnd();
            var newConfig = JsonConvert.DeserializeObject<ConfigModel>(jsonTextRow);
            GameManager.Instance.LoadConfigFromRemoteFile(newConfig);
            StartCoroutine(GetConfig(fileURL));
        }

        IEnumerator GetConfig(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.Send();
            
            if (request.isNetworkError == false)
            {
                Debug.Log("json content: " + request.downloadHandler.text);
                try
                {
                    var newConfig = JsonConvert.DeserializeObject<ConfigModel>(request.downloadHandler.text);
                    GameManager.Instance.LoadConfigFromRemoteFile(newConfig);
                }
                catch
                {
                    new Exception("Something wrong with JsonFile");
                }
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }

    public struct ConfigModel
    {
        public string BaseAttackRewardGold { get; set; }
        public string AttackerBaseBuyCost { get; set; }
        public string AttackerBuyingCostMultiplier { get; set; }
        public string AttackDelayTime { get; set; }
        public string AttackGoldRewardMultiplier { get; set; }
        public string AttackerBaseUpgradeCost { get; set; }
        public string BaseTapGold { get; set; }
        public string TapGoldSquaredValue { get; set; }
        public string TapBaseUpgradeCost { get; set; }
        public string TapUpgradeCostMultiplier { get; set; }
    }
    
}