using System;
using _GAME_.Scripts.FoodRelated.Coffee;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.MachineScripts
{
    public class CoffeMakerPodArea : MonoBehaviour
    {
        [NonSerialized] public CoffeePod PodInserted;
        [SerializeField] private Transform targetPos;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CoffeePod pod) && !PodInserted && !pod.IsPickedUp)
            {
                PodInserted = pod;
                pod.RigidBody.isKinematic = true;
                pod.GetComponent<Collider>().gameObject.layer = 0;
                pod.transform.DOMove(targetPos.position, 0.1f);
                pod.transform.DORotate(targetPos.eulerAngles, 0.1f);
            }
        }
    }
}
