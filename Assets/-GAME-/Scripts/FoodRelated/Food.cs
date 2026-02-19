using System;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public abstract class Food : Interactable
    {
        public enum FoodList
        {
            Bread,
            Chicken,
            Beef,
            Lettuce,
            Cheddar,
            Onion,
            Tomato,
            Ketchup,
            Mustard
            
        }
        [SerializeField] public FoodList foodType;
        [NonSerialized] public bool IsCooked;
    }
}
