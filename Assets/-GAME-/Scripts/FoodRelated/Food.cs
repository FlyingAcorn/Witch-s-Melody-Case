using System;
namespace _GAME_.Scripts.FoodRelated
{
    public class Food : Interactable
    {
        public enum FoodList
        {
            Cookie,
            Chicken,
            Beef,
            Lettuce,
            Cheddar,
            Onion,
            Tomato,
            Ketchup,
            Mustard,
            Fries,
            OnionRings,
            Nuggets
        }
        public FoodList foodType;
        [NonSerialized] public bool PrepDone;
    }
}
