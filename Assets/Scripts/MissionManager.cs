using UnityEngine;
using System.Collections.Generic;

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
    // Инвентарь работодателя
    public Inventory requesterInventory; 
    // Инвентарь игрока
    [System.NonSerialized] public Inventory executorInventory; 
}

public class MissionManager : MonoBehaviour
{
    public enum MissionState
    {
        None,           // Миссия не взята, доска открыта
        Available,      // Игрок в зоне MissionBoard, миссия не взята
        Active,         // Миссия активна (нужно доехать до DeliveryZone)
    }

    public MissionState CurrentState { get; private set; } = MissionState.None;

    [Header("Waypoints")]
    // Waypoint 2: Зона выбора миссии (MissionBoard)
    public GameObject missionStartWaypoint;
    // Waypoint 3: Зона доставки (DeliveryZone)
    public GameObject missionDeliveryWaypoint;

    [Header("Данные Миссий")]
    public List<Mission> availableMissions = new List<Mission>();
    [HideInInspector] public Mission activeMission;
    

    void Start()
    {
        missionDeliveryWaypoint?.SetActive(false);
        missionStartWaypoint?.SetActive(true);

        if (availableMissions.Count == 0 && missionDeliveryWaypoint != null)
        {
            availableMissions.Add(new Mission
            {
                missionName = "Тестовая Доставка (Доска Миссий)",
                reward = 750f,
                deliveryLocation = missionDeliveryWaypoint.transform.position,
                requesterInventory = null
            });
        }
    }

    public void SetMissionAvailable(bool isAvailable)
    {
        if (CurrentState == MissionState.None || CurrentState == MissionState.Available)
        {
            CurrentState = isAvailable ? MissionState.Available : MissionState.None;
        }
    }

    public void StartMission(Mission missionToStart)
    {
        if (CurrentState != MissionState.Available || missionToStart == null)
        {
            return;
        }

        activeMission = missionToStart;
        CurrentState = MissionState.Active;

        activeMission.executorInventory = Inventory.instance;

        missionStartWaypoint?.SetActive(false);
        missionDeliveryWaypoint?.SetActive(true);

        Debug.Log($"[Mission System] МИССИЯ {activeMission.missionName} АКТИВНА!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CompleteMission()
    {
        if (CurrentState != MissionState.Active || activeMission == null)
        {
            return;
        }

        float reward = activeMission.reward;
        activeMission.executorInventory.AddMoney(reward);

        if (activeMission.requesterInventory != null)
        {
            foreach (var req in activeMission.requiredResources)
            {
                activeMission.requesterInventory.AddResource(req.resourceName, req.amount);
            }
        }

        availableMissions.Remove(activeMission);
        missionDeliveryWaypoint?.SetActive(false);

        if (availableMissions.Count > 0)
        {
            missionStartWaypoint?.SetActive(true);
        }

        CurrentState = MissionState.None;
        activeMission = null;

        Debug.Log($"[Mission System] МИССИЯ ВЫПОЛНЕНА! Получено: ${reward}.");
    }


    public void GenerateNewMissions(List<Mission> customMissions)
    {
        if (CurrentState == MissionState.Active)
        {
            Debug.LogWarning("Нельзя генерировать новые миссии, пока активна текущая!");
            return;
        }

        availableMissions.Clear();

        foreach(var mission in customMissions)
        {
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