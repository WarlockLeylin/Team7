using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq; // Используется для LINQ (Find)

public class CraftingUIHandler : MonoBehaviour
{
    // --- Ссылки на UI-Элементы (Назначить в Инспекторе) ---
    [Header("1. Левая Колонка (Категории)")]
    public GameObject categoryButtonPrefab; // Префаб кнопки для категории (нож, Lv3)
    public Transform categoryContentParent; // Контейнер для кнопок категорий

    [Header("2. Правая Колонка (Список Рецептов)")]
    public GameObject recipeDetailsPrefab; // Полный блок рецепта (иконка, текст, шансы, кнопка)
    public Transform recipeContentParent;  // Контейнер, куда спавнятся блоки рецептов (Content_Recipes)

    private CraftingManager manager;

    void Awake()
    {
        manager = CraftingManager.Instance;
        if (manager == null)
        {
            Debug.LogError("CraftingManager не найден в сцене!");
            return;
        }
    }

    void OnEnable()
    {
        // Обновляем список категорий при каждом открытии UI
        GenerateCategoryList();

        // По умолчанию скрываем правую колонку
        if (recipeContentParent.parent.gameObject.activeSelf)
        {
            recipeContentParent.parent.gameObject.SetActive(false);
        }
    }

    // Вызывается OnEnable, чтобы заполнить левую колонку
    public void GenerateCategoryList()
    {
        // Очистка старых кнопок
        foreach (Transform child in categoryContentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CraftingCategory category in manager.allCategories)
        {
            GameObject buttonObj = Instantiate(categoryButtonPrefab, categoryContentParent);
            Button button = buttonObj.GetComponent<Button>();

            // Настройка текста и прогресса
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = category.categoryName;
                texts[1].text = $"Lv{category.currentLevel} ({category.currentEXP}/{category.expToNextLevel})";
            }

            // Настройка иконки
            Image icon = buttonObj.GetComponentsInChildren<Image>().FirstOrDefault(img => img != buttonObj.GetComponent<Image>());
            if (icon != null && category.categoryIcon != null)
            {
                icon.sprite = category.categoryIcon;
            }

            // Назначение действия: при нажатии открыть детали этой категории
            button.onClick.AddListener(() => OpenCategoryDetails(category));
        }
    }

    // Вызывается при нажатии на кнопку категории (Левая колонка)
    public void OpenCategoryDetails(CraftingCategory category)
    {
        // 1. Показываем контейнер правой колонки, если он был скрыт
        if (!recipeContentParent.parent.gameObject.activeSelf)
        {
            recipeContentParent.parent.gameObject.SetActive(true);
        }

        // 2. Генерируем список рецептов
        GenerateRecipeListWithDetails(category);
    }

    // Генерирует полный блок деталей рецепта (для правой колонки)
    private void GenerateRecipeListWithDetails(CraftingCategory category)
    {
        // Очищаем предыдущий список
        foreach (Transform child in recipeContentParent)
        {
            Destroy(child.gameObject);
        }

        int currentLevel = category.currentLevel;

        foreach (CraftableItem item in category.availableItems)
        {
            // Фильтр по уровню: не отображаем, если уровень ниже требуемого
            if (currentLevel < item.minCraftingLevel)
            {
                continue;
            }

            // --- 1. Создание блока рецепта ---
            GameObject recipeBlock = Instantiate(recipeDetailsPrefab, recipeContentParent);

            // --- 2. Нахождение и заполнение элементов внутри префаба ---

            // Название
            Transform titleTransform = recipeBlock.transform.Find("Text_Title");
            if (titleTransform != null) titleTransform.GetComponent<TextMeshProUGUI>().text = item.itemName;

            // Шансы
            Transform successTransform = recipeBlock.transform.Find("Text_Success");
            if (successTransform != null) successTransform.GetComponent<TextMeshProUGUI>().text = $"Успех: {item.successChance}%";

            Transform critTransform = recipeBlock.transform.Find("Text_Critical");
            if (critTransform != null) critTransform.GetComponent<TextMeshProUGUI>().text = $"Крит. успех: {item.criticalChance}%";

            // Иконка
            Transform iconTransform = recipeBlock.transform.Find("Image_Icon");
            if (iconTransform != null) iconTransform.GetComponent<Image>().sprite = item.itemIcon;

            // *******************************************************************
            // !!! КОД ГЕНЕРАЦИИ МАТЕРИАЛОВ УДАЛЕН ИЗ ЭТОЙ ВЕРСИИ СКРИПТА !!!
            // *******************************************************************

            // --- 3. Назначение действия кнопке КРАФТ ---
            Button craftButton = recipeBlock.transform.Find("Button_Craft").GetComponent<Button>();
            if (craftButton != null)
            {
                craftButton.onClick.RemoveAllListeners();
                craftButton.onClick.AddListener(() => manager.TryCraftItem(item));
            }
        }
    }
}