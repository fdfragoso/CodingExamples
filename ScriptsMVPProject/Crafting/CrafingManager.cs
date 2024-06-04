using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrafingManager : MonoBehaviour
{
    public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private Image resultPrefab;
    [SerializeField] private Button craftingSlotPrefab;
    [SerializeField] private Transform recipeParent;
    [SerializeField] private Transform resultParent;
    [SerializeField] private Transform[] ingredientParents;

    private CraftingRecipe currentRecipe;

    private void Start()
    {
        foreach(CraftingRecipe recipe in Recipes)
        {
            var button = Instantiate(craftingSlotPrefab, recipeParent);
            //(button.transform as RectTransform).localPosition = Vector2.zero;
            button.onClick.AddListener(()=>SelectRecipe(recipe));
            button.GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
        }
    }

    private void SelectRecipe(CraftingRecipe recipe)
    {
        ResetCrafting();

        currentRecipe = recipe;

        var result = Instantiate(resultPrefab, resultParent);
        (result.transform as RectTransform).localPosition = Vector2.zero;
        result.sprite = currentRecipe.craftingResult.image;
        for (int i = 0; i < currentRecipe.ingredients.Count; i++)
        {
            AddItemToSlot(i);
        }
    }

    private void AddItemToSlot(int index)
    {
        var obj = Instantiate(ingredientPrefab, Vector2.zero, Quaternion.identity);
        obj.GetComponent<Image>().sprite = currentRecipe.ingredients[index].item.image;
        obj.transform.SetParent(ingredientParents[index]);
        (obj.transform as RectTransform).localPosition = Vector2.zero;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetItemAmount(currentRecipe.ingredients[index].item).ToString("n0") + "/" + currentRecipe.ingredients[index].amount;
    }

    public void CraftItem()
    {
        if (currentRecipe == null)
            return;

        bool hasAllIngredients = true;
        for (int i = 0; i < currentRecipe.ingredients.Count; i++)
        {
            if(hasAllIngredients)
            {
                hasAllIngredients = inventory.HasItem(currentRecipe.ingredients[i].item, currentRecipe.ingredients[i].amount);
            }
        }

        if(hasAllIngredients)
        {
            for (int i = 0; i < currentRecipe.ingredients.Count; i++)
            {
                inventory.RemoveItem(currentRecipe.ingredients[i].item, currentRecipe.ingredients[i].amount);
            }
            inventory.AddItem(currentRecipe.craftingResult, 1);
            SelectRecipe(currentRecipe);
        }
        
    }

    private void ResetCrafting()
    {
        foreach (Transform parent in ingredientParents)
        {
            if (parent.childCount > 0)
                Destroy(parent.GetChild(0).gameObject);
        }
        if (resultParent.childCount > 0)
            Destroy(resultParent.GetChild(0).gameObject);

        currentRecipe = null;
    }

    private void OnDisable()
    {
        ResetCrafting();
    }
}
