using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCraftingCategory", menuName = "Crafting/Category")]
public class CraftingCategory : ScriptableObject
{
    [Header("������ ���������")]
    public string categoryName; // "�������� ������"
    public Sprite categoryIcon; // ������ ��� ���� �����
    public int maxLevel = 5;

    [Header("�������� ������")]
    public int currentLevel = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 300; // ����, ����������� ��� ���������� ������

    [Header("��������� �������")]
    public List<CraftableItem> availableItems; // ��� ������� � ���� ���������
}