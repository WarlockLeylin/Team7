using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using TMPro; 

public class MissionCreatorUI : MonoBehaviour
{
    [Header("Основные поля миссии")]
    // Обновлено на TMP_InputField для совместимости с TextMeshPro
    public TMP_InputField missionNameInput; 
    public TMP_InputField rewardInput; 
    
    [Header("Поля требуемых ресурсов")]
    // НОВЫЕ ФИКСИРОВАННЫЕ ПОЛЯ
    public TMP_InputField metalAmountInput; 
    public TMP_InputField gunpowderAmountInput; 
    
    public Button createMissionButton; 
    
    // СТАРЫЕ ПОЛЯ УДАЛЕНЫ: resourcesContainer, resourceEntryPrefab, resourceEntries

    void Start()
    {
        createMissionButton.onClick.AddListener(CreateMission);
    }

    // МЕТОД AddResourceField() УДАЛЕН

    public void CreateMission()
    {
        string missionName = missionNameInput.text;
        float reward = 0f;
        int metalAmount = 0;
        int gunpowderAmount = 0;

        // 1. Проверка и парсинг Награды
        if (!float.TryParse(rewardInput.text, out reward) || reward <= 0)
        {
            Debug.LogError("Неверный формат или значение награды!");
            return;
        }

        // 2. Проверка и парсинг Металла
        if (!int.TryParse(metalAmountInput.text, out metalAmount) || metalAmount < 0)
        {
            Debug.LogError("Неверное количество Металла!");
            return;
        }
        
        // 3. Проверка и парсинг Пороха
        if (!int.TryParse(gunpowderAmountInput.text, out gunpowderAmount) || gunpowderAmount < 0)
        {
            Debug.LogError("Неверное количество Пороха!");
            return;
        }

        // 4. Формирование списка требуемых ресурсов
        // Используем Mission.ResourceRequirement из MissionManager.cs
        List<ResourceRequirement> requiredResources = new List<ResourceRequirement>();
        
        if (metalAmount > 0)
        {
            requiredResources.Add(new ResourceRequirement { resourceName = "Металл", amount = metalAmount });
        }
        
        if (gunpowderAmount > 0)
        {
            requiredResources.Add(new ResourceRequirement { resourceName = "Порох", amount = gunpowderAmount });
        }

        // 5. Создание и добавление миссии
        MissionManager manager = FindObjectOfType<MissionManager>();
        if (manager != null)
        {
            Mission mission = new Mission
            {
                missionName = missionName,
                reward = reward,
                requiredResources = requiredResources,
                deliveryLocation = Vector3.zero, // Задайте нужную позицию
                requesterInventory = Inventory.instance // Предполагаем, что игрок создает миссию
            };
            manager.availableMissions.Add(mission);
            Debug.Log($"Миссия '{missionName}' создана и добавлена в список доступных!");
        }
        
        // Опционально: закрыть UI после создания
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}