using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace justDice_IdleClickerTest
{
    public class GoldDropPool : MonoBehaviour
    {
        [SerializeField] private List<Transform> coins = new List<Transform>();
        [SerializeField] private List<Transform> coinsInUse = new List<Transform>();
        [SerializeField] private float coinDropSpeed;
        [SerializeField] private float coinDropHorizontalRange;
        [SerializeField] private float coinResetDistance = 8;
        
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                coins.Add(transform.GetChild(i));
            }
        }

        private void Update()
        {
            foreach (var coin in coinsInUse)
            {
                coin.position = Vector3.MoveTowards(coin.position, Vector3.up * (coinResetDistance + 1), coinDropSpeed * Time.deltaTime);
               
                if (coin.position.y > coinResetDistance)
                {
                    resetCoin(coin);
                    break;
                }
            }
        }
        
        void resetCoin(Transform coin)
        {
            coin.gameObject.SetActive(false);
            coin.transform.localPosition = Vector3.zero;
            coins.Add(coin);
            coinsInUse.Remove(coin);
        }

        public void DropCoin()
        {
            if (coins.Count > 0)
            {
                coinsInUse.Add(coins[0]);
                coins[0].gameObject.SetActive(true);
                
                Vector3 coinPos = Vector3.zero;
                coinPos.x = Random.Range(-coinDropHorizontalRange, coinDropHorizontalRange);
                coinPos.y = Random.Range(1, 3);
                coins[0].position = coinPos;
                
                coins.Remove(coins[0]);
            }
        }
    
    }
}
