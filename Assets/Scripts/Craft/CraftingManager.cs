using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("Все Категории Крафта")]
    // Перетащите сюда все ваши ScriptableObjects CraftingCategory
    public List<CraftingCategory> allCategories;

    [Header("UI")]
    public GameObject craftingUI; // Общий UI Canvas/Panel стола

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Убедимся, что UI скрыт в начале
        if (craftingUI != null)
            craftingUI.SetActive(false);
    }

    // Вызывается InteractionTrigger'ом
    public void ToggleCraftingUI(bool state)
    {
        if (craftingUI != null)
        {
            craftingUI.SetActive(state);
            // Управление курсором, как и в MissionManager
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }
    }

    // === ЛОГИКА ПРОГРЕССА ===
    public void AddEXP(string categoryName, int amount)
    {
        CraftingCategory category = allCategories.Find(c => c.categoryName == categoryName);
        if (category == null) return;

        category.currentEXP += amount;

        while (category.currentEXP >= category.expToNextLevel && category.currentLevel < category.maxLevel)
        {
            category.currentEXP -= category.expToNextLevel;
            category.currentLevel++;

            // Расчет нового опыта для следующего уровня (например, увеличиваем на 50)
            category.expToNextLevel += 50;
            Debug.Log($"Уровень крафта '{category.categoryName}' повышен до {category.currentLevel}!");
        }

        // Обновление UI (если он открыт) должно быть в отдельном UI скрипте!
    }

    // === ЛОГИКА КРАФТА ===
    public bool TryCraftItem(CraftableItem item)
    {
        // 1. Проверка уровня
        CraftingCategory category = allCategories.Find(c => c.categoryName == item.category);
        if (category == null || category.currentLevel < item.minCraftingLevel)
        {
            Debug.Log("Уровень крафта слишком низкий.");
            return false;
        }

        // 2. Проверка инвентаря (заглушка)
        /* if (!InventoryManager.CheckMaterials(item.requirements))
        {
             Debug.Log("Не хватает материалов.");
             return false;
        } */

        // 3. Выполнение крафта
        int roll = Random.Range(0, 100);

        if (roll < item.successChance)
        {
            // Успешный крафт!
            // InventoryManager.RemoveMaterials(item.requirements);
            // InventoryManager.AddItem(item.itemName);

            // Проверка на крит. успех
            if (roll < item.criticalChance)
            {
                Debug.Log($"Критический успех! Создан {item.itemName} (Улучшенный).");
                AddEXP(item.category, 50); // Больше опыта за крит
            }
            else
            {
                Debug.Log($"Успешно создан {item.itemName}.");
                AddEXP(item.category, 30);
            }
            return true;
        }
        else
        {
            // Неудача
            // InventoryManager.RemoveMaterials(item.requirements, 0.5f); // Можно удалить часть материалов
            Debug.Log("Крафт не удался. Материалы потеряны (частично).");
            AddEXP(item.category, 5); // Немного опыта за попытку
            return false;
        }
    }
}