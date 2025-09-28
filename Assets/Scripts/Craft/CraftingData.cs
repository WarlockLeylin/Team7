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
    [Header("Базовые Данные")]
    public string itemName;
    public Sprite itemIcon;
    public string category; // Например, "Холодное Оружие"

    [Header("Параметры Крафта")]
    public List<CraftRequirement> requirements; // Список материалов
    public int successChance; // Процент успеха (например, 80)
    public int criticalChance; // Процент крит. успеха (например, 10)

    [Header("Требования")]
    public int minCraftingLevel; // Минимальный уровень для крафта
}