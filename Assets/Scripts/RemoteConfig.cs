using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace justDice_IdleClickerTest
{
    public class RemoteConfig : MonoBehaviour
    {
        private string fileURL = "https://drive.google.com/uc?export=download&id=12yER5n35P1xNOwIWBPXqiWilHee9wY5";
        
        void Start()
        {
            StartCoroutine(GetConfig(fileURL));
            Time.timeScale = 0;
        }

        IEnumerator GetConfig(string url)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var newConfig = JsonConvert.DeserializeObject<ConfigModel>(request.downloadHandler.text);
                    GameManager.Instance.LoadConfigFromRemoteFile(newConfig);
                    UIController.Instance.RemoveConfigFetchScreen(true);
                }
                catch (Exception e)
                {
                    Debug.Log("Error: " + request.error);
                    UIController.Instance.RemoveConfigFetchScreen(false);
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
                UIController.Instance.RemoveConfigFetchScreen(false);
            }
        }
    }

    // Model for json file in the remote config file - has to match all the elements,if wrong file, game will use default files.
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