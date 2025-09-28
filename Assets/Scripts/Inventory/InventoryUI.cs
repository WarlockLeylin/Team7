using UnityEngine;
using TMPro; // ОБЯЗАТЕЛЬНО для работы с текстом
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    // --- UI Элементы для ресурсов (НОВОЕ) ---
    [Header("Отображение ресурсов")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI metalText;
    public TextMeshProUGUI gunpowderText;

    // --- Элементы для слотов (ВАШЕ СУЩЕСТВУЮЩЕЕ) ---
    [Header("Слоты инвентаря")]
    public Transform itemsParent;
    private Inventory inventory;
    private InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        
        if (inventory != null)
        {
            // Подписываемся на событие (вызовет UpdateUI, когда инвентарь изменится)
            inventory.OnInventoryChanged += UpdateUI;
        }
        
        // Находим все слоты
        if (itemsParent != null)
        {
             slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }
       
        // Первоначальное обновление при старте
        UpdateUI();
    }
    
    private void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать ошибок при закрытии сцены
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateUI;
        }
    }

    // Удаляем неэффективный метод Update()
    // Этот метод теперь вызывается только по событию OnInventoryChanged
    public void UpdateUI()
    {
        if (inventory == null) return;

        // === 1. Обновление текстовых ресурсов (Деньги, Металл, Порох) ===
        if (moneyText != null)
        {
            // Форматируем до двух знаков после запятой
            moneyText.text = $"Деньги: ${inventory.money:F2}"; 
        }
        if (metalText != null)
        {
            // Используем GetResourceAmount, который мы определили в Inventory.cs
            metalText.text = $"Металл: {inventory.GetResourceAmount("Металл")}";
        }
        if (gunpowderText != null)
        {
            gunpowderText.text = $"Порох: {inventory.GetResourceAmount("Порох")}";
        }

    }
}