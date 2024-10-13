using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingView : MonoBehaviour
{
    //[SerializeField] private GameObject _content;
    [SerializeField] private TMP_Dropdown _recipeDropdown;
    [SerializeField] private Image _recipeImage;
    [SerializeField] private Button _craftButton;
    [SerializeField] private RecipeResourcePanel _resourcePanelprefab;
    [SerializeField] private Transform _resourcePanelParent;
    [SerializeField] private InventoryView _inventoryViewPrefab;
    [SerializeField] private Transform _inventoriesParent;

    private CraftingStation _craftingStation;
    private bool _initialized;
    private CraftingRecipe _currentRecipe;
    private List<RecipeResourcePanel> _resourcePanels = new();
    private OverlapFixRequester _overlapFixRequester = new OverlapFixRequester();
    private RectTransform _rectTransform;
    private Vector3 _position => Camera.main.WorldToScreenPoint(_craftingStation.transform.position);

    private bool _awaitingCraftingConfirmation;
    private bool _craftingConfirmed;

    private List<ItemSlot> _productSlots = new();
    private List<InventoryView> _inventoryViews = new();

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }

        //if (_craftingStation.Selectable.IsSelected || _craftingStation.Inventories.Any(i => i.Inventory.IsOpen))
        //{
        //    if (!_content.activeSelf)
        //        Show();
        //}
        //else
        //{
        //    if (_content.activeSelf)
        //        Hide();
        //}

        if (_currentRecipe != null)
        {
            _craftButton.interactable = _resourcePanels.TrueForAll(p => p.HasResources());
        }
    }

    private void OnDisable()
    {
        _overlapFixRequester.RequestFixUnsubscribe(_rectTransform);
    }

    private void OnDestroy()
    {
        _craftingStation.InventoriesUpdated -= DisplayInventories;
    }

    public void Init(CraftingStation craftingStation)
    {
        _craftingStation = craftingStation;
        foreach (var recipe in _craftingStation.Recipes)
        {
            _recipeDropdown.options.Add(new TMP_Dropdown.OptionData(recipe.Product.Name, recipe.Product.Icon,
                Color.white));
        }

        _recipeDropdown.onValueChanged.AddListener(OnRecipeSelected);
        _craftButton.onClick.AddListener(Craft);
        _rectTransform = GetComponent<RectTransform>();
        //Hide();
        Show();
        _initialized = true;
        //ClearInventories();
        DisplayInventories(_craftingStation);
        craftingStation.InventoriesUpdated += DisplayInventories;
    }


    public void Show()
    {
        // _content.SetActive(true);
        _overlapFixRequester.RequestFixSubscribe(_rectTransform, _position);
        OnRecipeSelected(_recipeDropdown.value);
    }

    public void Hide()
    {
        // _content.SetActive(false);
        _overlapFixRequester.RequestFixUnsubscribe(_rectTransform);
    }

    private void OnRecipeSelected(int optionIndex)
    {
        var option = _recipeDropdown.options[optionIndex];
        _currentRecipe = null;
        foreach (var recipe in _craftingStation.Recipes)
        {
            if (recipe.Product.Name == option.text)
            {
                _currentRecipe = recipe;
                break;
            }
        }

        _craftingStation.OnRecipeSelected(_currentRecipe);
        _recipeImage.sprite = _currentRecipe.Product.Icon;
        SpawnResourcePanels();
    }

    private async void Craft()
    {
        //TODO handle multiple IRecipeProduct implementations
        _productSlots.Clear();

        if (await CraftTask())
        {
            foreach (var inventory in _craftingStation.Inventories)
            {
                foreach (var slot in inventory.Inventory.ItemSlots)
                {
                    if (!_productSlots.Contains(slot))
                    {
                        while (slot.Amount > 0)
                        {
                            slot.Decrement();
                        }
                    }
                }
            }
        }
    }

    //TODO make it abstract and implement for each IRecipeProduct implementation
    private async Task<bool> CraftTask()
    {
        _craftingConfirmed = false;

        if (_currentRecipe.Product is ItemData)
        {
            var productData = _currentRecipe.Product as ItemData;

            var slot = _craftingStation.Inventories[0].Inventory.ItemSlots[0];
            slot.Item = new Item(productData);
            slot.Increment();
            _productSlots.Add(slot);
            _craftingConfirmed = true;
        }

        if (_currentRecipe.Product is StructureData)
        {
            var productData = _currentRecipe.Product as StructureData;

            var ghost = Instantiate(productData.StructureGhostPrefab);
            ghost.SetPosition();
            Hide();

            _awaitingCraftingConfirmation = true;
            ghost.Confirmed += OnStructureGhostConfirmed;
            ghost.Canceled += OnStructureGhostCanceled;
            while (_awaitingCraftingConfirmation)
            {
                await Task.Yield();
            }

            return _craftingConfirmed;
        }

        return _craftingConfirmed;
    }

    private void OnStructureGhostConfirmed(StructureGhost ghost)
    {
        ghost.Confirmed -= OnStructureGhostConfirmed;
        ghost.Canceled -= OnStructureGhostCanceled;
        _craftingConfirmed = true;
        _awaitingCraftingConfirmation = false;
    }

    private void OnStructureGhostCanceled(StructureGhost ghost)
    {
        ghost.Confirmed -= OnStructureGhostConfirmed;
        ghost.Canceled -= OnStructureGhostCanceled;
        _craftingConfirmed = false;
        _awaitingCraftingConfirmation = false;
    }

    private void ClearResourcePanels()
    {
        for (int i = _resourcePanels.Count - 1; i >= 0; i--)
        {
            Destroy(_resourcePanels[i].gameObject);
        }

        _resourcePanels.Clear();
    }

    private void SpawnResourcePanels()
    {
        ClearResourcePanels();

        foreach (var requiredResource in _currentRecipe.RequiredResources)
        {
            (Inventory Inventory, ResourceType resource) inventory =
                _craftingStation.Inventories.FirstOrDefault(i => i.Resource == requiredResource.Resource);

            RecipeResourcePanel panel = Instantiate(_resourcePanelprefab, _resourcePanelParent);
            panel.Init(requiredResource, inventory.Inventory);
            _resourcePanels.Add(panel);
        }
    }

    public void DisplayInventories(CraftingStation craftingStation)
    {
        ClearInventories();
        foreach (var inventory in craftingStation.Inventories)
        {
            InventoryView inventoryView = Instantiate(_inventoryViewPrefab, _inventoriesParent);
            inventoryView.Init(inventory.Inventory);
            _inventoryViews.Add(inventoryView);
        }
    }
    private void ClearInventories()
    {
        for (int i = _inventoryViews.Count - 1; i >= 0; i--)
        {
            Destroy(_inventoryViews[i].gameObject);
        }

        _inventoryViews.Clear();
    }
}