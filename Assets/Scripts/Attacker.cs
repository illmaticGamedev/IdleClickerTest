using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace justDice_IdleClickerTest
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] TrailRenderer shootTrail;
        [SerializeField] ParticleSystem trailHitParticles;
        [SerializeField] float arrowShootSpeed;
        [SerializeField] TMP_Text attackerLevelText;
        [SerializeField] GameObject upgradeIcon;
        
        private Animation preShootAnimation;
        private Vector3 trailStartPos;
        private Transform trailTransform;
        
        //attack and gold logic
        private float attackerBuyingCostMultiplier;
        private float attackerAttackTimeDelay;
        private double attackerUpgradeCost;
        private float attackerGoldMultiplier;
        
        private int attackerLevel = 1;
        private double baseGoldPerAttack = 5;
        private double currentGoldPerAttack;
        
        private void Start()
        {
            preShootAnimation = GetComponent<Animation>();
            trailTransform = shootTrail.transform;
            trailStartPos = trailTransform.position;
            attackerLevelText.GetComponent<Renderer>().sortingOrder = 10;
            checkRemoteDataOrSetToDefault();
            InvokeRepeating(nameof(Attack), 0, attackerAttackTimeDelay);
        }

        private void Update()
        {
            if (shootTrail.gameObject.activeInHierarchy)
            {
                trailTransform.position += trailTransform.up * (Time.deltaTime * arrowShootSpeed);

                if (shootTrail.transform.position.y > 0.25f)
                {
                    trailHitParticles.transform.position = trailTransform.position + (trailTransform.forward * Random.Range(1f, 3f));
                    trailHitParticles.transform.localRotation = Quaternion.Euler(Random.Range(60, 120), 0, 0);
                    trailHitParticles.Play();
                    shootTrail.gameObject.SetActive(false);
                    shootTrail.emitting = false;
                    
                    GameManager.Instance.CollectGoldOnTap(false, currentGoldPerAttack);
                }
            }

            if (GameManager.Instance.currentGold >= attackerUpgradeCost)
            {
                upgradeIcon.SetActive(true);
            }
            else
            {
                upgradeIcon.SetActive(false);
            }
        }

        void Attack()
        {
            shootTrail.transform.position = trailStartPos;
            preShootAnimation.Play();
            shootTrail.gameObject.SetActive(true);
            shootTrail.emitting = true;
        }

        private void OnMouseDown()
        {
            UpgradeAttack();
        }

        public void UpgradeAttack()
        {
            if(GameManager.Instance.currentGold >= attackerUpgradeCost)
            {
                GameManager.Instance.UpdateGold(-attackerUpgradeCost);
                
                attackerLevel += 1;
                attackerUpgradeCost *= attackerBuyingCostMultiplier;
                attackerLevelText.text = "x" + attackerLevel;
                currentGoldPerAttack = baseGoldPerAttack * Mathf.Pow(attackerLevel, attackerGoldMultiplier);
            }
        }

        void checkRemoteDataOrSetToDefault()
        {
            ConfigModel newConfig = GameManager.Instance._ConfigModel;
            attackerUpgradeCost = float.Parse(newConfig.AttackerBaseUpgradeCost);
            attackerAttackTimeDelay = float.Parse(newConfig.AttackDelayTime);
            baseGoldPerAttack = float.Parse(newConfig.BaseAttackRewardGold);
            attackerGoldMultiplier = float.Parse(newConfig.AttackGoldRewardMultiplier);
            attackerBuyingCostMultiplier = float.Parse(newConfig.AttackerBuyingCostMultiplier);
        }
        
    }
}
