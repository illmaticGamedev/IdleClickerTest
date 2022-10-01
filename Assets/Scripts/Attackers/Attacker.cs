using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace justDice_IdleClickerTest
{
    public class Attacker : MonoBehaviour
    {
        // Indexing attacker to help save their data
        [FormerlySerializedAs("AttackerNumber")] public int AttackerIndex;
        
        [SerializeField] TrailRenderer shootTrail;
        [SerializeField] ParticleSystem trailHitParticles;
        [SerializeField] float arrowShootSpeed;
        [SerializeField] TMP_Text attackerLevelText;
        [SerializeField] GameObject upgradeIcon;
        [SerializeField] BuyAttackerButton relatedButton;
        
        private Animation preShootAnimation;
        private Vector3 trailStartPos;
        private Transform trailTransform;
        
        //SaveData for attacker
        [SerializeField] public int attackerLevel = 1;
        [SerializeField] public float currentGoldPerAttack;
        [SerializeField] public float attackerUpgradeCost;
        
        private void Start()
        {
            preShootAnimation = GetComponent<Animation>();
            trailTransform = shootTrail.transform;
            trailStartPos = trailTransform.position;
            attackerLevelText.GetComponent<Renderer>().sortingOrder = 10;
        }

        
        //Shoot the projectile when attacker is being used.
        void Update()
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
                    
                    Managers.Instance.gameManager.AddGold(false, (int)currentGoldPerAttack);
                    saveData();
                }
            }

            if (Managers.Instance.gameManager.currentGold >= attackerUpgradeCost)
            {
                upgradeIcon.SetActive(true);
            }
            else
            {
                upgradeIcon.SetActive(false);
            }
        }

        void attack()
        {
            shootTrail.transform.position = trailStartPos;
            preShootAnimation.Play();
            shootTrail.gameObject.SetActive(true);
            shootTrail.emitting = true;
        }
        
        void OnMouseDown()
        {
            upgradeAttack();
        }
        
        void saveData()
        {
            Managers.Instance.attackerManager.SaveAttackerData(AttackerIndex, attackerLevel, attackerUpgradeCost, currentGoldPerAttack);
        }

        void loadData()
        {
            var myAttackerData = Managers.Instance.attackerManager.LoadAttackerData(AttackerIndex);
            
            if (myAttackerData.Level > 0)
            {
                Debug.Log("Attacker Already Being Used");
                attackerLevel = myAttackerData.Level;
                attackerUpgradeCost =  myAttackerData.UpgradeCost;
                currentGoldPerAttack =  myAttackerData.GoldPerAttack;
                
                attackerLevelText.text = "x" + attackerLevel;
                currentGoldPerAttack = (long)(Managers.Instance.attackerManager.baseGoldPerAttack * Mathf.Pow(attackerLevel, Managers.Instance.attackerManager.attackerGoldMultiplier));
                Destroy(relatedButton);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        void upgradeAttack()
        {
            if(Managers.Instance.gameManager.currentGold >= attackerUpgradeCost)
            {
                Managers.Instance.gameManager.UpdateGold(-(int) attackerUpgradeCost);
                
                attackerLevel += 1;
                attackerUpgradeCost *= (int)Managers.Instance.attackerManager.attackerBuyingCostMultiplier;
                attackerLevelText.text = "x" + attackerLevel;
                currentGoldPerAttack = (long)(Managers.Instance.attackerManager.baseGoldPerAttack * Mathf.Pow(attackerLevel, Managers.Instance.attackerManager.attackerGoldMultiplier));
            }
        }
        
        public void StartAttacks()
        {
            currentGoldPerAttack = (int)(Managers.Instance.attackerManager.baseGoldPerAttack * Mathf.Pow(attackerLevel, Managers.Instance.attackerManager.attackerGoldMultiplier));
            loadData();
            InvokeRepeating(nameof(attack), Random.Range(0,1f), Managers.Instance.attackerManager.attackerAttackTimeDelay);
        }
        
    }
}
