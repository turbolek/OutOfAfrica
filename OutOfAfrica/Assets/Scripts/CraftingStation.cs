using System.Collections.Generic;
using UnityEngine;
using Variables;

public class CraftingStation : MonoBehaviour
{
    [field: SerializeField] public Inventory InventoryPrefab;
    [field: SerializeField] private CraftingView _craftingViewPrefab;
    [field: SerializeField] public List<CraftingRecipe> Recipes { get; private set; } = new();
    [SerializeField] private TransformVariable _mainCanvasVariable;


    public List<(Inventory Inventory, ResourceType Resource)> Inventories { get; private set; } = new();

    public Selectable Selectable { get; private set; }

    private CraftingRecipe _currentRecipe;

    private void Start()
    {
        Selectable = GetComponent<Selectable>();
        CraftingView craftingView = Instantiate(_craftingViewPrefab, _mainCanvasVariable.Value);
        craftingView.Init(this);
        OnRecipeSelected(Recipes[0]);
    }

    public void OnRecipeSelected(CraftingRecipe recipe)
    {
        if (!Recipes.Contains(recipe))
        {
            return;
        }

        if (recipe == _currentRecipe)
        {
            return;
        }

        _currentRecipe = recipe;
        SpawnInventories(recipe);
    }

    private void ClearInventories()
    {
        for (int i = Inventories.Count - 1; i >= 0; i--)
        {
            Destroy(Inventories[i].Inventory.gameObject);
        }

        Inventories.Clear();
    }


    private void SpawnInventories(CraftingRecipe recipe)
    {
        ClearInventories();

        foreach (var resource in recipe.RequiredResources)
        {
            var inventory = Instantiate(InventoryPrefab, transform);
            inventory.gameObject.name = resource.Resource.name;
            Inventories.AddExclusive(new(inventory, resource.Resource));
            inventory.Init(Selectable);
        }
    }
}