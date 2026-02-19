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
            Lettuce,
            Cheddar,
            Ketchup,
            Mustard
        }
        [SerializeField] public FoodList foodType;
        [NonSerialized] public bool IsCooked;
    }
}
