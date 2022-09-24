using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rotator : MonoBehaviour
{
   [SerializeField] private float rotateSpeed;
   [SerializeField] private Vector3 rotateAngle;

   private void Update()
   {
      transform.Rotate(rotateAngle * (rotateSpeed * Time.deltaTime));
   }

   public void RandomizeAngles()
   {
      int randomAngle = Random.Range(-2, 3);
      
      if(randomAngle != 0)
         rotateAngle = new Vector3(randomAngle, randomAngle, randomAngle);
   }
}
