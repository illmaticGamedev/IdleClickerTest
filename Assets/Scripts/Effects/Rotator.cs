using UnityEngine;
using Random = UnityEngine.Random;

namespace justDice_IdleClickerTest
{
   public class Rotator : MonoBehaviour
   {
      [SerializeField] private float rotateSpeed;
      [SerializeField] private Vector3 rotateAngle;
      [SerializeField] private bool useRandomAnglesOnStart = false;

      private void Start()
      {
         if (useRandomAnglesOnStart)
         {
            rotateAngle.x = Random.Range(-45, 45);
            rotateAngle.y = Random.Range(-45, 45);
            rotateAngle.z = Random.Range(-45, 45);
         }
      }

      private void Update()
      {
         transform.Rotate(rotateAngle * (rotateSpeed * Time.deltaTime));
      }

      public void RandomizeAngles()
      {
         int randomAngle = Random.Range(-2, 3);

         if (randomAngle != 0)
            rotateAngle = new Vector3(randomAngle, randomAngle, randomAngle);
      }
   }
}
