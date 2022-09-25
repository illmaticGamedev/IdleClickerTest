using System;
using justDice_IdleClickerTest;
using UnityEngine;

namespace justDice_IdleClickerTest
{
    public class BuyAttackerButton : MonoBehaviour
    {
        [SerializeField] Attacker attacker;

        private void OnMouseDown()
        {
            buyAttacker();
        }

        void buyAttacker()
        {
            attacker.gameObject.SetActive(true);
            GameManager.Instance.OnAttackerBought(this);
        }
        
    }
}
