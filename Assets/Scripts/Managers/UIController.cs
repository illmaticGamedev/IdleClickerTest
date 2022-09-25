using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace justDice_IdleClickerTest
{
    public class UIController : Singleton<UIController>
    {
        [Header("Gold and Tap")] 
        [SerializeField] TMP_Text txtCurrentGold;
        [SerializeField] TMP_Text txtUpgradeLevel;
        [SerializeField] Button btnGoldTap;
        [SerializeField] Button btnUpgradeLevel;
        
        [Header("Attacker UI")]
        [SerializeField] GameObject attackerAvailableBanner;
        [SerializeField] TMP_Text txtAttackerCost;
        
        private void Start()
        {
            setButtonEvents();
        }
        
        void setButtonEvents()
        {
            btnGoldTap.onClick.AddListener(() => GameManager.Instance.CollectGoldOnTap());
            btnUpgradeLevel.onClick.AddListener(() => GameManager.Instance.UpgradeTapLevel());
        }

        void upgradeButtonState()
        {
            if (GameManager.Instance.EnoughGoldForTapUpgrade())
            {
                btnUpgradeLevel.interactable = true;
            }
            else
            {
                btnUpgradeLevel.interactable = false;
            }
        }

        public void SetAttackerStateAndValue(string price, bool state)
        {
            txtAttackerCost.text = price;
            attackerAvailableBanner.SetActive(state);
        }
        
        public void SetLevelText(double currentLevel)
        {
            txtUpgradeLevel.text = "x" + currentLevel;
        }

        public void SetCurrentGold(double currentGold)
        {
            txtCurrentGold.text = Math.Round(currentGold).ToString();
            upgradeButtonState();
        }
    }
}