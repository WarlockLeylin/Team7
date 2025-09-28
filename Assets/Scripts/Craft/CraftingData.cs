using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CraftRequirement
{
    public string materialName;
    public int requiredAmount;
}

[System.Serializable]
public class CraftableItem
{
    [Header("������� ������")]
    public string itemName;
    public Sprite itemIcon;
    public string category; // ��������, "�������� ������"

    [Header("��������� ������")]
    public List<CraftRequirement> requirements; // ������ ����������
    public int successChance; // ������� ������ (��������, 80)
    public int criticalChance; // ������� ����. ������ (��������, 10)

    [Header("����������")]
    public int minCraftingLevel; // ����������� ������� ��� ������
}