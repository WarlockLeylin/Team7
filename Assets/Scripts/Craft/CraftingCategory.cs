using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCraftingCategory", menuName = "Crafting/Category")]
public class CraftingCategory : ScriptableObject
{
    [Header("Данные Категории")]
    public string categoryName; // "Холодное Оружие"
    public Sprite categoryIcon; // Иконка для меню слева
    public int maxLevel = 5;

    [Header("Прогресс Игрока")]
    public int currentLevel = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 300; // Опыт, необходимый для следующего уровня

    [Header("Доступные Рецепты")]
    public List<CraftableItem> availableItems; // Все рецепты в этой категории
}