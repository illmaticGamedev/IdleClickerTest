using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace justDice_IdleClickerTest
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Gold Setup")]
        public double currentGold;
        [SerializeField] double goldPerTap = 0;
        [SerializeField] double tapUpgradeCost = 0;

        [Header("Attacker")]
        [SerializeField] double attackerBaseBuyCost = 100;
        [SerializeField] double attackerBuyCost;
        [SerializeField] double attackerBuyingCostMultiplier = 10;
        
        [Header("Math Setup - Tap")]
        [SerializeField] float baseTapGold = 5;
        [SerializeField] float upgradeSquaredMultiplier = 2.1f;
        
        [Header("Tap Upgrade")] 
        [SerializeField] int tapCurrentLevel = 1;
        [SerializeField] float baseUpgradeCost = 5;
        [SerializeField] float upgradeCostMultiplier = 1.08f;

        [Header("References")] 
        [SerializeField] Effects effects;
        [SerializeField] GoldDropPool goldDropObjectPool;
        [SerializeField] BuyAttackerButton[] attackerBuyBtns = new BuyAttackerButton[5];
        
        //Remote Data
        [HideInInspector] public ConfigModel _ConfigModel = new ConfigModel();
        
        //Saving Data - playerpref names
        private string savedTapLevel = "TapLevel";
        private string savedCurrentGold = "CurrentGold";
        private string savedAttackerBuyingCost = "AttackerBuyingCost";
        
        void Start()
        {
            //Default Config Values (if remote fails), Would ideally keep it in a GLOBAL_SETTINGS class with all const variables.
            _ConfigModel.AttackerBaseBuyCost = 100.ToString();
            _ConfigModel.AttackerBuyingCostMultiplier = 10.ToString();
            _ConfigModel.BaseTapGold = 5.ToString();
            _ConfigModel.TapGoldSquaredValue = 2.1f.ToString();
            _ConfigModel.TapBaseUpgradeCost = 5.ToString();
            _ConfigModel.TapUpgradeCostMultiplier = 1.08.ToString();
            _ConfigModel.AttackerBaseUpgradeCost = 100.ToString();
            _ConfigModel.AttackDelayTime = 1.ToString();
            _ConfigModel.BaseAttackRewardGold = 5.ToString();
            _ConfigModel.AttackGoldRewardMultiplier = 2.1f.ToString();
            _ConfigModel.AttackerBuyingCostMultiplier = 10.ToString();

            loadData();
            updateGoldPerTapAndUpgradeCost();
        }
        
        void updateGoldPerTapAndUpgradeCost()
        {
            goldPerTap = baseTapGold * Mathf.Pow(tapCurrentLevel, upgradeSquaredMultiplier);
            tapUpgradeCost = Mathf.Pow(upgradeCostMultiplier * baseUpgradeCost, tapCurrentLevel);
        }
        
        void verifyGoldForBuyingAttacker()
        {
            if (currentGold >= attackerBuyCost)
            {
                foreach (var item in attackerBuyBtns)
                {
                    if (item != null)
                    {
                        item.gameObject.SetActive(true);
                    }
                }
                
                UIController.Instance.SetAttackerStateAndValue(attackerBuyCost.ToString(), false);
            }
            else
            {
                bool arrayValid = false;
                foreach (var item in attackerBuyBtns)
                {
                    if (item != null)
                    {
                        item.gameObject.SetActive(false);
                    }
                }

                foreach (var item in attackerBuyBtns)
                {
                    if (item != null)
                    {
                        arrayValid = true;
                        break;
                    }
                }
                
                if(attackerBuyBtns.Length > 0 && arrayValid)
                    UIController.Instance.SetAttackerStateAndValue(attackerBuyCost.ToString(), true);
            }
        }
        
        void saveData()
        {
            PlayerPrefs.SetInt(savedTapLevel, tapCurrentLevel);
            PlayerPrefs.SetString(savedCurrentGold, currentGold.ToString());
            PlayerPrefs.SetString(savedAttackerBuyingCost, attackerBuyCost.ToString());
        }

        void loadData()
        {
            if (PlayerPrefs.HasKey(savedCurrentGold))
            {
                tapCurrentLevel = PlayerPrefs.GetInt(savedTapLevel);
                currentGold = float.Parse(PlayerPrefs.GetString(savedCurrentGold));
                attackerBuyCost = float.Parse(PlayerPrefs.GetString(savedAttackerBuyingCost));
                UIController.Instance.SetLevelText(tapCurrentLevel);
                UpdateGold(0);
            }
        }
        
        public void OnAttackerBought(BuyAttackerButton boughtAttackerBtn)
        {
            for (int i = 0; i < attackerBuyBtns.Length; i++)
            {
                if (attackerBuyBtns[i] == boughtAttackerBtn)
                {
                    attackerBuyBtns[i].gameObject.SetActive(false);
                    attackerBuyBtns[i] = null;
                    break;
                }
            }
            
            verifyGoldForBuyingAttacker();
            UpdateGold(-attackerBuyCost);
            attackerBuyCost *= attackerBuyingCostMultiplier;
        }
        
        public void CollectGoldOnTap(bool directHit = true, double attackerGold = 0)
        {
            if (directHit)
            {
                currentGold += goldPerTap;
            }
            else
            {
                currentGold += attackerGold;
            }
            
            UIController.Instance.SetCurrentGold(currentGold);
            effects.PlayTapEffects(directHit);
            goldDropObjectPool.DropCoin();
            verifyGoldForBuyingAttacker();
            saveData();
        }

        public void UpdateGold(double change)
        {
            currentGold += change;
            UIController.Instance.SetCurrentGold(currentGold);
        }
        
        public void UpgradeTapLevel()
        {
            if (currentGold >= tapUpgradeCost)
            {
                currentGold -= tapUpgradeCost;
                tapCurrentLevel += 1;
                updateGoldPerTapAndUpgradeCost();
                
                UIController.Instance.SetLevelText(tapCurrentLevel);
                UIController.Instance.SetCurrentGold((int)currentGold);
                verifyGoldForBuyingAttacker();
            }
        }
        
        public bool EnoughGoldForTapUpgrade()
        {
            if (currentGold >= tapUpgradeCost)
                return true;
            
            return false;
        }

        public void LoadConfigFromRemoteFile(ConfigModel configModel)
        {
            _ConfigModel = configModel;
            attackerBaseBuyCost = float.Parse(_ConfigModel.AttackerBaseBuyCost);
            attackerBuyingCostMultiplier = float.Parse(_ConfigModel.AttackerBuyingCostMultiplier);
            baseTapGold = float.Parse(_ConfigModel.BaseTapGold);
            upgradeSquaredMultiplier = float.Parse(_ConfigModel.TapGoldSquaredValue);
            baseUpgradeCost = float.Parse(_ConfigModel.TapBaseUpgradeCost);
            upgradeCostMultiplier = float.Parse(_ConfigModel.TapUpgradeCostMultiplier);
            
            
            updateGoldPerTapAndUpgradeCost();
        }
        
        [ContextMenu("DELETE ALL SAVED DATA")]
        public void DeleteAllPrefs()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Game");
        }
        
    }
}
