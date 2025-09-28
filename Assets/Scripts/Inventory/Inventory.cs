using UnityEngine;
using System; // Для Action
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    // Событие, которое будет вызываться при изменении любого ресурса или денег
    public event Action OnInventoryChanged;

    // Хранилище ресурсов (для Металла и Пороха)
    private Dictionary<string, int> resources = new Dictionary<string, int>();
    public float money = 0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Делаем инвентарь постоянным между сценами
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Методы, используемые другими скриптами (MissionManager) ---

    public int GetResourceAmount(string resourceName)
    {
        if (resources.ContainsKey(resourceName))
        {
            return resources[resourceName];
        }
        return 0;
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources.Add(resourceName, amount);
        }
        // Вызываем событие, чтобы UI обновился
        OnInventoryChanged?.Invoke(); 
        Debug.Log($"Ресурс {resourceName} добавлен. Всего: {GetResourceAmount(resourceName)}");
    }

    public void RemoveResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] -= amount;
            if (resources[resourceName] <= 0)
            {
                resources.Remove(resourceName);
            }
            // Вызываем событие
            OnInventoryChanged?.Invoke(); 
        }
        Debug.Log($"Ресурс {resourceName} списан. Остаток: {GetResourceAmount(resourceName)}");
    }
    
    public void AddMoney(float amount)
    {
        money += amount;
        // Вызываем событие
        OnInventoryChanged?.Invoke(); 
        Debug.Log($"Получено ${amount}. Новый баланс: ${money}");
    }
}