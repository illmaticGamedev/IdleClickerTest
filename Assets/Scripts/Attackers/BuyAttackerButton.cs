using UnityEngine;

namespace justDice_IdleClickerTest
{
    public class BuyAttackerButton : MonoBehaviour
    {
        public Attacker attacker;

        private void OnMouseDown()
        {
            buyAttacker();
        }

        void buyAttacker()
        {
            attacker.gameObject.SetActive(true);
            Managers.Instance.attackerManager.OnAttackerBought(this);
        }
        
    }
}
