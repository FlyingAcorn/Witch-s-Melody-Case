using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated.FriedFood
{
    public class FryableFood : MonoBehaviour
    {
        [NonSerialized] public Food Food;
        [SerializeField] private Renderer myRenderer;
        [SerializeField] private List<Material> foodStateMaterials;
        
        private void Awake()
        {
            Food = GetComponent<Food>();
        }

        public void ChangeMaterial(int index)
        {
            myRenderer.material = foodStateMaterials[index];
        }
        
        
        
    }
}
