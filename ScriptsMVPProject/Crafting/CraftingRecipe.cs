using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting System/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
    public ItemObject craftingResult;
}

[System.Serializable]
public class RecipeIngredient
{
    public ItemObject item;
    public int amount;
    public RecipeIngredient(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
