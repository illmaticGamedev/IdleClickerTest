using UnityEngine;

public class Effects : MonoBehaviour
{
   [Header("Hit Effects")] 
   [SerializeField] Animation hammerHitAnim;
   [SerializeField] Animation cubeHitAnim;
   [SerializeField] ParticleSystem hammerHitParticles;
   [SerializeField] Rotator cubeRotator;
   public void PlayTapEffects()
   {
       hammerHitAnim.Stop();
       hammerHitAnim.Play();
       
       cubeHitAnim.Stop();
       cubeHitAnim.Play();
       
       hammerHitParticles.Play();

       if (Random.Range(0, 20) == 1)
       {
           cubeRotator.RandomizeAngles();
       }
   }
}
