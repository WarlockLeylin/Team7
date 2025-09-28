using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("��� ��������� ������")]
    // ���������� ���� ��� ���� ScriptableObjects CraftingCategory
    public List<CraftingCategory> allCategories;

    [Header("UI")]
    public GameObject craftingUI; // ����� UI Canvas/Panel �����

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // ��������, ��� UI ����� � ������
        if (craftingUI != null)
            craftingUI.SetActive(false);
    }

    // ���������� InteractionTrigger'��
    public void ToggleCraftingUI(bool state)
    {
        if (craftingUI != null)
        {
            craftingUI.SetActive(state);
            // ���������� ��������, ��� � � MissionManager
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }
    }

    // === ������ ��������� ===
    public void AddEXP(string categoryName, int amount)
    {
        CraftingCategory category = allCategories.Find(c => c.categoryName == categoryName);
        if (category == null) return;

        category.currentEXP += amount;

        while (category.currentEXP >= category.expToNextLevel && category.currentLevel < category.maxLevel)
        {
            category.currentEXP -= category.expToNextLevel;
            category.currentLevel++;

            // ������ ������ ����� ��� ���������� ������ (��������, ����������� �� 50)
            category.expToNextLevel += 50;
            Debug.Log($"������� ������ '{category.categoryName}' ������� �� {category.currentLevel}!");
        }

        // ���������� UI (���� �� ������) ������ ���� � ��������� UI �������!
    }

    // === ������ ������ ===
    public bool TryCraftItem(CraftableItem item)
    {
        // 1. �������� ������
        CraftingCategory category = allCategories.Find(c => c.categoryName == item.category);
        if (category == null || category.currentLevel < item.minCraftingLevel)
        {
            Debug.Log("������� ������ ������� ������.");
            return false;
        }

        // 2. �������� ��������� (��������)
        /* if (!InventoryManager.CheckMaterials(item.requirements))
        {
             Debug.Log("�� ������� ����������.");
             return false;
        } */

        // 3. ���������� ������
        int roll = Random.Range(0, 100);

        if (roll < item.successChance)
        {
            // �������� �����!
            // InventoryManager.RemoveMaterials(item.requirements);
            // InventoryManager.AddItem(item.itemName);

            // �������� �� ����. �����
            if (roll < item.criticalChance)
            {
                Debug.Log($"����������� �����! ������ {item.itemName} (����������).");
                AddEXP(item.category, 50); // ������ ����� �� ����
            }
            else
            {
                Debug.Log($"������� ������ {item.itemName}.");
                AddEXP(item.category, 30);
            }
            return true;
        }
        else
        {
            // �������
            // InventoryManager.RemoveMaterials(item.requirements, 0.5f); // ����� ������� ����� ����������
            Debug.Log("����� �� ������. ��������� �������� (��������).");
            AddEXP(item.category, 5); // ������� ����� �� �������
            return false;
        }
    }
}