using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace justDice_IdleClickerTest
{
    public sealed class GameManager : MonoBehaviour
    {
        [Header("Gold Setup")]
        public long currentGold;
        [SerializeField] float goldPerTap;

        [Header("Tap")]
        [SerializeField] float baseTapGold;
        [SerializeField] float upgradeSquaredMultiplier;
        [SerializeField] float tapUpgradeCost;
        
        [Header("Tap Upgrade")] 
        [SerializeField] int tapCurrentLevel;
        [SerializeField] float baseUpgradeCost;
        [SerializeField] float upgradeCostMultiplier;

        [Header("References")]
        [SerializeField] GoldDropPool goldDropObjectPool;
        
        void Start()
        {
            updateGoldPerTapAndUpgradeCost();
        }
        
        void updateGoldPerTapAndUpgradeCost()
        {
            goldPerTap = baseTapGold * Mathf.Pow(tapCurrentLevel, upgradeSquaredMultiplier);
            tapUpgradeCost = Mathf.Pow(upgradeCostMultiplier * baseUpgradeCost, tapCurrentLevel);
        }

        void saveData()
        {
            Managers.Instance.dataManager.SavePlayerData(currentGold, tapCurrentLevel, Managers.Instance.attackerManager.attackerBuyCost);
        }

        void loadSavedData()
        {
            // loading the last saved data from the binary file
            PlayerData gamePlayerData = Managers.Instance.dataManager.LoadPlayerData();
            
            tapCurrentLevel = gamePlayerData.Level;
            currentGold = gamePlayerData.Gold;
            Managers.Instance.attackerManager.attackerBuyCost = gamePlayerData.AttackerBuyCost;
            
            // if old data didnt exist then we get our default values
            if (gamePlayerData.Level == 0)
            {
                tapCurrentLevel = 1;
                currentGold = 0;
                Managers.Instance.attackerManager.attackerBuyCost = Managers.Instance.configManager.currentGameSettings.AttackerBaseBuyCost;
            }
            
            Managers.Instance.uIManager.SetLevelText(tapCurrentLevel);
            Managers.Instance.attackerManager.VerifyBudgetToBuyAttacker();
            UpdateGold(0);
        }
        
        // Tap directly adds gold while attackers have to send the gold value in the method.
        public void AddGold(bool directHit = true, long attackerGold = 0)
        {
            if (directHit)
            {
                currentGold += (int)goldPerTap;
            }
            else
            {
                currentGold += attackerGold;
            }
            
            Managers.Instance.uIManager.SetCurrentGold(currentGold);
            Managers.Instance.effectsManager.PlayTapEffects(directHit);
            Managers.Instance.attackerManager.VerifyBudgetToBuyAttacker();
            goldDropObjectPool.DropCoin();
            saveData();
        }

        public void UpdateGold(long change)
        {
            currentGold += change;
            Managers.Instance.uIManager.SetCurrentGold(currentGold);
        }
        
        public void UpgradeTapLevel()
        {
            if (currentGold >= tapUpgradeCost)
            {
                currentGold -= (int)tapUpgradeCost;
                tapCurrentLevel += 1;
                updateGoldPerTapAndUpgradeCost();
                
                Managers.Instance.uIManager.SetLevelText(tapCurrentLevel);
                Managers.Instance.uIManager.SetCurrentGold((int)currentGold);
                Managers.Instance.attackerManager.VerifyBudgetToBuyAttacker();
            }
        }
        
        public bool EnoughGoldForTapUpgrade()
        {
            if (currentGold >= tapUpgradeCost)
                return true;
            
            return false;
        }

        public void LoadConfigFromRemoteFile()
        {
            ConfigModel newConfig = Managers.Instance.configManager.currentGameSettings;
            
            baseTapGold = newConfig.BaseTapGold;
            upgradeSquaredMultiplier = newConfig.TapGoldSquaredValue;
            baseUpgradeCost = newConfig.TapBaseUpgradeCost;
            upgradeCostMultiplier = newConfig.TapUpgradeCostMultiplier;
            
            Managers.Instance.attackerManager.FetchRemoteAttackerSettings();
            loadSavedData();
            updateGoldPerTapAndUpgradeCost();
        }
        
    }
}
