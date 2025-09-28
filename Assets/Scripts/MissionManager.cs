using UnityEngine;
using System.Collections.Generic;

// Структура для хранения данных миссии
[System.Serializable]
public struct ResourceRequirement
{
    public string resourceName;
    public int amount;
}

[System.Serializable]
public class Mission
{
    public string missionName = "Доставка Ресурсов";
    public float reward = 500f;
    public List<ResourceRequirement> requiredResources = new List<ResourceRequirement>();
    public Vector3 deliveryLocation;
    // Инвентарь, который ЗАПРАШИВАЕТ ресурсы (Работодатель/NPC)
    public Inventory requesterInventory; 
    // Инвентарь, который ВЫПОЛНЯЕТ миссию (Игрок)
    [System.NonSerialized] public Inventory executorInventory; 
    // NonSerialized, чтобы не пытался сохранить ссылку на игрока
}

public class MissionManager : MonoBehaviour
{
    public enum MissionState
    {
        None,           // Миссия не взята, доска открыта
        Available,      // Игрок в зоне MissionBoard, миссия не взята
        Active,         // Миссия активна (нужно доехать до DeliveryZone)
    }

    // === ПЕРЕМЕННЫЕ ===
    public MissionState CurrentState { get; private set; } = MissionState.None;

    [Header("Waypoints")]
    // Waypoint 2: Зона выбора миссии (MissionBoard)
    public GameObject missionStartWaypoint;
    // Waypoint 3: Зона доставки (DeliveryZone)
    public GameObject missionDeliveryWaypoint;

    [Header("Данные Миссий")]
    public List<Mission> availableMissions = new List<Mission>();
    [HideInInspector] public Mission activeMission; // Скрываем в Инспекторе
    
    // Удалено: public float currentMoney = 0f; (Деньги лучше хранить в Inventory.cs)

    // === МЕТОДЫ ===

    void Start()
    {
        missionDeliveryWaypoint?.SetActive(false);
        missionStartWaypoint?.SetActive(true);

        // Если миссий нет, создаем тестовую, используя текущую позицию DeliveryZone
        // Важно: Эта миссия будет доступна при старте, но без requesterInventory
        if (availableMissions.Count == 0 && missionDeliveryWaypoint != null)
        {
            availableMissions.Add(new Mission
            {
                missionName = "Тестовая Доставка (Доска Миссий)",
                reward = 750f,
                deliveryLocation = missionDeliveryWaypoint.transform.position,
                requesterInventory = null // Добавьте ссылку, если нужно
            });
        }
    }

    // Вызывается MissionStartWaypoint.cs, когда игрок заходит/выходит из зоны
    public void SetMissionAvailable(bool isAvailable)
    {
        if (CurrentState == MissionState.None || CurrentState == MissionState.Available)
        {
            CurrentState = isAvailable ? MissionState.Available : MissionState.None;
        }
    }

    // Вызывается из UI (Mission Board), когда игрок выбирает миссию
    public void StartMission(Mission missionToStart)
    {
        if (CurrentState != MissionState.Available || missionToStart == null)
        {
            return;
        }

        activeMission = missionToStart;
        CurrentState = MissionState.Active;

        // Устанавливаем инвентарь ИСПОЛНИТЕЛЯ (игрока)
        // Предполагаем, что Inventory.instance - это инвентарь игрока
        activeMission.executorInventory = Inventory.instance;

        // 1. Деактивировать MissionBoard (Waypoint 2)
        missionStartWaypoint?.SetActive(false);

        // 2. Активировать DeliveryZone и установить его позицию (Waypoint 3)
        missionDeliveryWaypoint?.SetActive(true);


        Debug.Log($"[Mission System] МИССИЯ {activeMission.missionName} АКТИВНА!");

        // Скрываем курсор после выбора миссии
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Вызывается MissionDeliveryWaypoint.cs
    public void CompleteMission()
    {
        if (CurrentState != MissionState.Active || activeMission == null)
        {
            return;
        }


        // Выдаем награду исполнителю (игроку)
        float reward = activeMission.reward;
        activeMission.executorInventory.AddMoney(reward);

        // Передаем ресурсы заказчику (если есть)
        if (activeMission.requesterInventory != null)
        {
            foreach (var req in activeMission.requiredResources)
            {
                activeMission.requesterInventory.AddResource(req.resourceName, req.amount);
            }
        }

        availableMissions.Remove(activeMission);

        // --- 3. Очистка и сброс состояния ---

        // 1. Скрыть DeliveryZone
        missionDeliveryWaypoint?.SetActive(false);

        // 2. Активировать MissionBoard для следующей миссии
        if (availableMissions.Count > 0)
        {
            missionStartWaypoint?.SetActive(true);
        }

        CurrentState = MissionState.None;
        activeMission = null; // Освобождаем активную миссию

        Debug.Log($"[Mission System] МИССИЯ ВЫПОЛНЕНА! Получено: ${reward}.");
    }



    // Вызывается MissionGiver'ом (Waypoint 1)
    // Inventory requesterInventory - удален, так как не используется корректно
    public void GenerateNewMissions(List<Mission> customMissions)
    {
        if (CurrentState == MissionState.Active)
        {
            Debug.LogWarning("Нельзя генерировать новые миссии, пока активна текущая!");
            return;
        }

        availableMissions.Clear();

        // Копируем миссии, чтобы изменения в UI не влияли на исходный список
        foreach(var mission in customMissions)
        {
             // Создаем копию миссии, чтобы не менять исходные данные
             Mission newMission = new Mission
             {
                 missionName = mission.missionName,
                 reward = mission.reward,
                 requiredResources = new List<ResourceRequirement>(mission.requiredResources),
                 deliveryLocation = mission.deliveryLocation,
                 requesterInventory = mission.requesterInventory
             };
             availableMissions.Add(newMission);
        }
        
        missionStartWaypoint?.SetActive(true);

        // Обновление UI
        MissionBoardUIHandler uiHandler = missionStartWaypoint?.GetComponent<MissionBoardUIHandler>();
        if (uiHandler != null && missionStartWaypoint.activeSelf)
        {
            uiHandler.GenerateMissionList();
        }
        Debug.Log($"MissionGiver: Сгенерировано {availableMissions.Count} новых миссий. Теперь идите к MissionBoard.");
    }
}