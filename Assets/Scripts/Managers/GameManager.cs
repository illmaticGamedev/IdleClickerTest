using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace justDice_IdleClickerTest
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Gold Setup")]
        public double currentGold;
        [SerializeField] double goldPerTap = 0;
        [SerializeField] double tapUpgradeCost = 0;

        [Header("Attacker Config")]
        [SerializeField] double attackerBuyCost = 100;
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
        void Start()
        {
            updateGoldPerTapAndUpgradeCost();
        }
        
        void updateGoldPerTapAndUpgradeCost()
        {
            goldPerTap = baseTapGold * Mathf.Pow(tapCurrentLevel, upgradeSquaredMultiplier);
            tapUpgradeCost = Mathf.Pow(upgradeCostMultiplier * baseUpgradeCost, tapCurrentLevel);
        }
        
        public bool EnoughGoldForTapUpgrade()
        {
            if (currentGold >= tapUpgradeCost)
                return true;
            
            return false;
        }

        public void VerifyGoldForBuyingAttacker()
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
                foreach (var item in attackerBuyBtns)
                {
                    if(item != null)
                        item.gameObject.SetActive(false);
                }
                
                if(attackerBuyBtns.Length > 0)
                    UIController.Instance.SetAttackerStateAndValue(attackerBuyCost.ToString(), true);
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
            
            VerifyGoldForBuyingAttacker();
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
            VerifyGoldForBuyingAttacker();
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
                VerifyGoldForBuyingAttacker();
            }
        }

        public void LoadConfigFromRemoteFile(ConfigModel configModel)
        {
            _ConfigModel = configModel;
            attackerBuyCost = float.Parse(_ConfigModel.AttackerBaseBuyCost);
            attackerBuyingCostMultiplier = float.Parse(_ConfigModel.AttackerBuyingCostMultiplier);
            baseTapGold = float.Parse(_ConfigModel.BaseTapGold);
            upgradeSquaredMultiplier = float.Parse(_ConfigModel.TapGoldSquaredValue);
            baseUpgradeCost = float.Parse(_ConfigModel.TapBaseUpgradeCost);
            upgradeCostMultiplier = float.Parse(_ConfigModel.TapUpgradeCostMultiplier);
            
        }
        
    }
}
