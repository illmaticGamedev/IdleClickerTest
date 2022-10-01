using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace justDice_IdleClickerTest
{
    public sealed class UiManager : MonoBehaviour
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

        [Header("Settings - Temp")] 
        [SerializeField] GameObject settingsPanel;
        [SerializeField] Button btnSettings;
        [SerializeField] Button btnQuit;
        [SerializeField] Button btnResetAllSavedData;
        [SerializeField] Button btnCloseSettingsPanel;
        
        private void Start()
        {
            waitForRemoteConfigCheck.gameObject.SetActive(true);
            setButtonEvents();
        }
        
        void setButtonEvents()
        {
            btnGoldTap.onClick.AddListener(() => Managers.Instance.gameManager.AddGold(true));
            btnUpgradeLevel.onClick.AddListener(() => Managers.Instance.gameManager.UpgradeTapLevel());
            btnResetAllSavedData.onClick.AddListener(() => Managers.Instance.dataManager.DeleteSavedPlayerData());
            btnSettings.onClick.AddListener(() => { settingsPanel.SetActive(true); });
            btnSettings.onClick.AddListener(() => { settingsPanel.SetActive(true); });
            btnQuit.onClick.AddListener(() => { Application.Quit(); });
            btnCloseSettingsPanel.onClick.AddListener(() => { settingsPanel.SetActive(false); });
        }

        void upgradeButtonState()
        {
            if (Managers.Instance.gameManager.EnoughGoldForTapUpgrade())
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
        
        public void SetLevelText(int currentLevel)
        {
            txtUpgradeLevel.text = "x" + currentLevel;
        }

        public void SetCurrentGold(long currentGold)
        {
            txtCurrentGold.text = currentGold.ToString();
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