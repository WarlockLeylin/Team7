using UnityEngine;
using System.Collections.Generic;

// Структура для хранения данных миссии
[System.Serializable]
public class Mission
{
    public string missionName = "Доставка Ресурсов";
    public float reward = 500f;
    public Vector3 deliveryLocation; // Координаты Waypoint 3
}

public class MissionManager : MonoBehaviour
{
    public enum MissionState
    {
        None,           // Миссия не взята, доска открыта
        Available,      // Миссия доступна (игрок у MissionBoard)
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
    public Mission activeMission;

    [Header("Экономика")]
    public float currentMoney = 0f;

    // === МЕТОДЫ ===

    void Start()
    {
        if (missionDeliveryWaypoint != null)
        {
            missionDeliveryWaypoint.SetActive(false);
        }
        if (missionStartWaypoint != null)
        {
            missionStartWaypoint.SetActive(true);
        }

        // Если миссий нет, создаем тестовую, используя текущую позицию DeliveryZone
        if (availableMissions.Count == 0 && missionDeliveryWaypoint != null)
        {
            availableMissions.Add(new Mission
            {
                missionName = "Тестовая Доставка (Начните с Работодателя)",
                reward = 750f,
                deliveryLocation = missionDeliveryWaypoint.transform.position
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
        if (CurrentState != MissionState.Available) return;

        activeMission = missionToStart;
        CurrentState = MissionState.Active;

        // 1. Скрыть MissionBoard
        if (missionStartWaypoint != null) missionStartWaypoint.SetActive(false);

        // 2. Активировать DeliveryZone и установить его позицию (Waypoint 3)
        if (missionDeliveryWaypoint != null)
        {
            missionDeliveryWaypoint.transform.position = activeMission.deliveryLocation;
            missionDeliveryWaypoint.SetActive(true);
        }

        Debug.Log($"[Mission System] МИССИЯ {activeMission.missionName} АКТИВНА!");

        // Скрываем курсор после выбора миссии
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Вызывается MissionDeliveryWaypoint.cs
    public void CompleteMission()
    {
        if (CurrentState != MissionState.Active) return;

        float reward = activeMission.reward;

        CurrentState = MissionState.None;
        currentMoney += reward;
        activeMission = null;

        // 1. Скрыть DeliveryZone
        if (missionDeliveryWaypoint != null) missionDeliveryWaypoint.SetActive(false);

        // 2. Активировать MissionBoard для следующей миссии
        if (missionStartWaypoint != null) missionStartWaypoint.SetActive(true);

        Debug.Log($"[Mission System] МИССИЯ ВЫПОЛНЕНА! Получено: ${reward}. Баланс: ${currentMoney}");
    }

    // Вызывается MissionGiver'ом (Waypoint 1)
    public void GenerateNewMissions()
    {
        if (CurrentState == MissionState.Active) return;

        // В реальном проекте здесь будет логика генерации.
        // Для примера: очищаем и добавляем две миссии.
        availableMissions.Clear();

        // Миссия 1
        availableMissions.Add(new Mission
        {
            missionName = "Срочный Груз на Склад A",
            reward = 1500f,
            deliveryLocation = missionDeliveryWaypoint.transform.position
        });

        // Миссия 2
        availableMissions.Add(new Mission
        {
            missionName = "Доставка на Завод Z",
            reward = 2200f,
            deliveryLocation = missionDeliveryWaypoint.transform.position
        });

        // Если UI открыт, обновляем его
        MissionBoardUIHandler uiHandler = missionStartWaypoint.GetComponent<MissionBoardUIHandler>();
        if (uiHandler != null && uiHandler.gameObject.activeSelf)
        {
            uiHandler.GenerateMissionList();
        }

        Debug.Log($"MissionGiver: Сгенерировано {availableMissions.Count} новых миссий. Теперь идите к MissionBoard.");
    }
}