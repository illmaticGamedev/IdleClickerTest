using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace justDice_IdleClickerTest
{
    public sealed class RemoteConfig : MonoBehaviour
    {
        private string fileURL = "https://drive.google.com/uc?export=download&id=12yER5n35P1xNOwIWBPXqiWilHee9wY57";
        public ConfigModel currentGameSettings;

        private void Awake()
        {
            currentGameSettings = new ConfigModel();
        }

        void Start()
        {
            StartCoroutine(GetConfig(fileURL));
            Time.timeScale = 0;
        }

        IEnumerator GetConfig(string url)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    currentGameSettings = JsonConvert.DeserializeObject<ConfigModel>(request.downloadHandler.text);
                    Managers.Instance.gameManager.LoadConfigFromRemoteFile();
                    Managers.Instance.uIManager.RemoveConfigFetchScreen(true);
                }
                catch (Exception e)
                {
                    Managers.Instance.uIManager.RemoveConfigFetchScreen(false);
                    SetSettingsToDefaultValues();
                }

            }
            else
            {
                Debug.Log("Error: " + request.error);
                Managers.Instance.uIManager.RemoveConfigFetchScreen(false);
                SetSettingsToDefaultValues();
            }
        }

        public void SetSettingsToDefaultValues()
        {
            currentGameSettings.AttackerBaseBuyCost = ProjectConstants.BASE_ATTACKER_BUY_COST;
            currentGameSettings.AttackerBuyingCostMultiplier =  ProjectConstants.ATTACKER_BUY_COST_MULTIPLIER;
            currentGameSettings.BaseTapGold =  ProjectConstants.BASE_TAP_GOLD;
            currentGameSettings.TapGoldSquaredValue =  ProjectConstants.TAP_GOLD_SQUARED_VALUE;
            currentGameSettings.TapBaseUpgradeCost =  ProjectConstants.BASE_TAP_UPGRADE_COST;
            currentGameSettings.TapUpgradeCostMultiplier =  ProjectConstants.TAP_UPGRADE_COST_MULTIPLIER;
            currentGameSettings.AttackerBaseUpgradeCost =  ProjectConstants.ATTACKER_BASE_UPGRADE_COST;
            currentGameSettings.AttackDelayTime =  ProjectConstants.ATTACKER_DELAY_TIME;
            currentGameSettings.BaseAttackRewardGold =  ProjectConstants.BASE_ATTACK_REWARD_GOLD;
            currentGameSettings.AttackGoldRewardMultiplier =  ProjectConstants.ATTACKER_GOLD_REWARD_MULTITPLIER;
        }
    }

    // Model for json file in the remote config file - has to match all the elements. if wrong or invalid file, game will use default values.
    public struct ConfigModel
    {
        public float BaseAttackRewardGold { get; set; }
        public float AttackerBaseBuyCost { get; set; }
        public float AttackerBuyingCostMultiplier { get; set; }
        public float AttackDelayTime { get; set; }
        public float AttackGoldRewardMultiplier { get; set; }
        public float AttackerBaseUpgradeCost { get; set; }
        public float BaseTapGold { get; set; }
        public float TapGoldSquaredValue { get; set; }
        public float TapBaseUpgradeCost { get; set; }
        public float TapUpgradeCostMultiplier { get; set; }
    }
    
}