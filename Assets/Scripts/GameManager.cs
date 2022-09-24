using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace justDice_IdleClickerTest
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] float currentGold;
        [SerializeField] float goldPerTap = 0;
        [SerializeField] float upgradeCost = 0;
        
        [Header("Temp Math Setup")]
        [SerializeField] int baseTapGold = 5;
        [SerializeField] float upgradeSquaredMultiplier = 2.1f;
        
        [Header("Tap Upgrade")] 
        [SerializeField] int tapCurrentLevel = 1;
        [SerializeField] int baseUpgradeCost = 5;
        [SerializeField] float upgradeCostMultiplier = 1.08f;

        [Header("References")] 
        [SerializeField] Effects effects;
        private void Start()
        {
            updateGoldPerTapAndUpgradeCost();
        }

        public void CollectGoldOnTap()
        {
            currentGold += (int)goldPerTap;
            UIController.Instance.SetCurrentGold((int)currentGold);
            effects.PlayTapEffects();
        }
        
        public void UpgradeLevel()
        {
            if (currentGold >= upgradeCost)
            {
                currentGold -= upgradeCost;
                tapCurrentLevel += 1;
                updateGoldPerTapAndUpgradeCost();
                
                UIController.Instance.SetLevelText(tapCurrentLevel);
                UIController.Instance.SetCurrentGold((int)currentGold);
            }
        }

        void updateGoldPerTapAndUpgradeCost()
        {
            goldPerTap = baseTapGold * Mathf.Pow(tapCurrentLevel, upgradeSquaredMultiplier);
            upgradeCost = baseUpgradeCost * Mathf.Pow(upgradeCostMultiplier, tapCurrentLevel);
        }

        public bool EnoughGoldForUpgrade()
        {
            if (currentGold >= upgradeCost)
                return true;
            else return false;
        }
    }
}
