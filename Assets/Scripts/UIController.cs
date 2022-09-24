using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace justDice_IdleClickerTest
{
    public class UIController : Singleton<UIController>
    {
        [Header("UI Feedback References")] 
        [SerializeField] TMP_Text txtCurrentGold;
        [SerializeField] TMP_Text txtUpgradeLevel;

        [Header("Button References")] 
        [SerializeField] Button btnGoldTap;
        [SerializeField] Button btnUpgradeLevel;

        private void Start()
        {
            setButtonEvents();
        }

        public void SetLevelText(int currentLevel)
        {
            txtUpgradeLevel.text = "x" + currentLevel;
        }

        public void SetCurrentGold(int currentGold)
        {
            txtCurrentGold.text = currentGold.ToString();
            upgradeButtonState();
        }
        
        void setButtonEvents()
        {
            btnGoldTap.onClick.AddListener(() => GameManager.Instance.CollectGoldOnTap());
            btnUpgradeLevel.onClick.AddListener(() => GameManager.Instance.UpgradeLevel());
        }

        void upgradeButtonState()
        {
            if (GameManager.Instance.EnoughGoldForUpgrade())
            {
                btnUpgradeLevel.interactable = true;
            }
            else
            {
                btnUpgradeLevel.interactable = false;
            }
        }
    }
}