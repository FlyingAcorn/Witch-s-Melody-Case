using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace _GAME_.Scripts.FoodRelated
{
    public class RecipeFood : Interactable
    {
        protected override void Awake()
        {
            base.Awake();
        }
        public override void Interact()
        {
        }
        // recipefood + cookable obj (check if cooked)+ 2filling(max)+2 sauce (max)
    }
}
