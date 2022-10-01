using UnityEngine;

namespace justDice_IdleClickerTest
{
    public class Effects : MonoBehaviour
    {
        [Header("Hit Effects")] 
        [SerializeField] Animation goldScorePopAnim;
        [SerializeField] Animation hammerHitAnim;
        [SerializeField] Animation cubeHitAnim;
        [SerializeField] ParticleSystem hammerHitParticles;
        [SerializeField] Rotator cubeRotator;
        [SerializeField] GoldDropPool goldDropObjectPool;
        
        public void PlayTapEffects(bool isTappingGold)
        {
            if (isTappingGold)
            {
                hammerHitAnim.Stop();
                hammerHitAnim.Play();
                hammerHitParticles.Play();
                cubeHitAnim.Stop();
                cubeHitAnim.Play();
            }
            
            if (!goldScorePopAnim.isPlaying)
            {
                goldScorePopAnim.Stop();
                goldScorePopAnim.Play();
            }

            if (Random.Range(0, 20) == 1)
            {
                cubeRotator.RandomizeAngles();
            }
            
            goldDropObjectPool.DropCoin();
        }
    }
}