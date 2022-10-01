using UnityEngine;

namespace justDice_IdleClickerTest
{
    public sealed class AttackerManager : MonoBehaviour
    {
        [SerializeField] BuyAttackerButton[] attackerBuyBtns = new BuyAttackerButton[5];
        
        // Buying New Attacker
        public float attackerBuyCost;
        public float attackerBuyingCostMultiplier;
        
        public float attackerAttackTimeDelay;
        public float attackerGoldMultiplier;
        public float baseGoldPerAttack;
        
        public void FetchRemoteAttackerSettings()
        {
            ConfigModel newConfig = Managers.Instance.configManager.currentGameSettings;
            
            attackerBuyingCostMultiplier =  newConfig.AttackerBuyingCostMultiplier;
            attackerAttackTimeDelay = newConfig.AttackDelayTime;
            attackerGoldMultiplier = newConfig.AttackGoldRewardMultiplier;
            baseGoldPerAttack = newConfig.BaseAttackRewardGold;

            foreach (var attackerBtn in attackerBuyBtns)
            {
                attackerBtn.attacker.StartAttacks();
            }

        }
        
        public void VerifyBudgetToBuyAttacker()
        {
            if (Managers.Instance.gameManager.currentGold >= attackerBuyCost)
            {
                foreach (var item in attackerBuyBtns)
                {
                    if (item != null)
                    {
                        item.gameObject.SetActive(true);
                    }
                }
                
                Managers.Instance.uIManager.SetAttackerStateAndValue(attackerBuyCost.ToString(), false);
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
                    Managers.Instance.uIManager.SetAttackerStateAndValue(attackerBuyCost.ToString(), true);
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
            
            Managers.Instance.gameManager.UpdateGold(-(int)attackerBuyCost);
            attackerBuyCost *= attackerBuyingCostMultiplier;
            VerifyBudgetToBuyAttacker();
        }

        public void SaveAttackerData(int attackerIndex, int level, float attackerUpgradeCost, float goldPerAttack)
        {
            Managers.Instance.dataManager.SaveAttackerData(attackerIndex, level, attackerUpgradeCost, goldPerAttack);
        }

        public AttackerData LoadAttackerData(int attackerIndex)
        {
            return Managers.Instance.dataManager.LoadAttackerData(attackerIndex);
        }
    }
}