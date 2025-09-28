using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq; // ������������ ��� LINQ (Find)

public class CraftingUIHandler : MonoBehaviour
{
    // --- ������ �� UI-�������� (��������� � ����������) ---
    [Header("1. ����� ������� (���������)")]
    public GameObject categoryButtonPrefab; // ������ ������ ��� ��������� (���, Lv3)
    public Transform categoryContentParent; // ��������� ��� ������ ���������

    [Header("2. ������ ������� (������ ��������)")]
    public GameObject recipeDetailsPrefab; // ������ ���� ������� (������, �����, �����, ������)
    public Transform recipeContentParent;  // ���������, ���� ��������� ����� �������� (Content_Recipes)

    private CraftingManager manager;

    void Awake()
    {
        manager = CraftingManager.Instance;
        if (manager == null)
        {
            Debug.LogError("CraftingManager �� ������ � �����!");
            return;
        }
    }

    void OnEnable()
    {
        // ��������� ������ ��������� ��� ������ �������� UI
        GenerateCategoryList();

        // �� ��������� �������� ������ �������
        if (recipeContentParent.parent.gameObject.activeSelf)
        {
            recipeContentParent.parent.gameObject.SetActive(false);
        }
    }

    // ���������� OnEnable, ����� ��������� ����� �������
    public void GenerateCategoryList()
    {
        // ������� ������ ������
        foreach (Transform child in categoryContentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CraftingCategory category in manager.allCategories)
        {
            GameObject buttonObj = Instantiate(categoryButtonPrefab, categoryContentParent);
            Button button = buttonObj.GetComponent<Button>();

            // ��������� ������ � ���������
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = category.categoryName;
                texts[1].text = $"Lv{category.currentLevel} ({category.currentEXP}/{category.expToNextLevel})";
            }

            // ��������� ������
            Image icon = buttonObj.GetComponentsInChildren<Image>().FirstOrDefault(img => img != buttonObj.GetComponent<Image>());
            if (icon != null && category.categoryIcon != null)
            {
                icon.sprite = category.categoryIcon;
            }

            // ���������� ��������: ��� ������� ������� ������ ���� ���������
            button.onClick.AddListener(() => OpenCategoryDetails(category));
        }
    }

    // ���������� ��� ������� �� ������ ��������� (����� �������)
    public void OpenCategoryDetails(CraftingCategory category)
    {
        // 1. ���������� ��������� ������ �������, ���� �� ��� �����
        if (!recipeContentParent.parent.gameObject.activeSelf)
        {
            recipeContentParent.parent.gameObject.SetActive(true);
        }

        // 2. ���������� ������ ��������
        GenerateRecipeListWithDetails(category);
    }

    // ���������� ������ ���� ������� ������� (��� ������ �������)
    private void GenerateRecipeListWithDetails(CraftingCategory category)
    {
        // ������� ���������� ������
        foreach (Transform child in recipeContentParent)
        {
            Destroy(child.gameObject);
        }

        int currentLevel = category.currentLevel;

        foreach (CraftableItem item in category.availableItems)
        {
            // ������ �� ������: �� ����������, ���� ������� ���� ����������
            if (currentLevel < item.minCraftingLevel)
            {
                continue;
            }

            // --- 1. �������� ����� ������� ---
            GameObject recipeBlock = Instantiate(recipeDetailsPrefab, recipeContentParent);

            // --- 2. ���������� � ���������� ��������� ������ ������� ---

            // ��������
            Transform titleTransform = recipeBlock.transform.Find("Text_Title");
            if (titleTransform != null) titleTransform.GetComponent<TextMeshProUGUI>().text = item.itemName;

            // �����
            Transform successTransform = recipeBlock.transform.Find("Text_Success");
            if (successTransform != null) successTransform.GetComponent<TextMeshProUGUI>().text = $"�����: {item.successChance}%";

            Transform critTransform = recipeBlock.transform.Find("Text_Critical");
            if (critTransform != null) critTransform.GetComponent<TextMeshProUGUI>().text = $"����. �����: {item.criticalChance}%";

            // ������
            Transform iconTransform = recipeBlock.transform.Find("Image_Icon");
            if (iconTransform != null) iconTransform.GetComponent<Image>().sprite = item.itemIcon;

            // *******************************************************************
            // !!! ��� ��������� ���������� ������ �� ���� ������ ������� !!!
            // *******************************************************************

            // --- 3. ���������� �������� ������ ����� ---
            Button craftButton = recipeBlock.transform.Find("Button_Craft").GetComponent<Button>();
            if (craftButton != null)
            {
                craftButton.onClick.RemoveAllListeners();
                craftButton.onClick.AddListener(() => manager.TryCraftItem(item));
            }
        }
    }
}