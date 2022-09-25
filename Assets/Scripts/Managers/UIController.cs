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

        [Header("Loading and Config")] 
        [SerializeField] Animator waitForRemoteConfigCheck;
        [SerializeField] TMP_Text txtConfigFetchStatus;
        
        private void Start()
        {
            waitForRemoteConfigCheck.gameObject.SetActive(true);
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

        public void RemoveConfigFetchScreen(bool success)
        {
            Time.timeScale = 1;
            
            if (success)
            {
                txtConfigFetchStatus.text = "Remote Config Fetched Successfully!";
            }
            else
            {
                txtConfigFetchStatus.text = "Remote Config Fetching Failed, Continuing with default settings.";
            }
            
            waitForRemoteConfigCheck.Play("FadeOut");
        }
    }
}