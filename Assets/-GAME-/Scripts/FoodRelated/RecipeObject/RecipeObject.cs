
namespace _GAME_.Scripts.FoodRelated.RecipeObject
{
    public abstract class RecipeObject : Interactable
    {
        public enum RecipeObjects
        {
            Bread,
            Bowl,
            FryingBag,
            Cup,
            Plate,
            Coffee
        }
        public RecipeObjects recipeObjectsType;
    }
}
