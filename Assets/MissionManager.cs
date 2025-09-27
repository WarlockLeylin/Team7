using UnityEngine;
using System.Collections.Generic;

// ��������� ��� �������� ������ ������
[System.Serializable]
public class Mission
{
    public string missionName = "�������� ��������";
    public float reward = 500f;
    public Vector3 deliveryLocation; // ���������� Waypoint 3
}

public class MissionManager : MonoBehaviour
{
    public enum MissionState
    {
        None,           // ������ �� �����, ����� �������
        Available,      // ������ �������� (����� � MissionBoard)
        Active,         // ������ ������� (����� ������� �� DeliveryZone)
    }

    // === ���������� ===
    public MissionState CurrentState { get; private set; } = MissionState.None;

    [Header("Waypoints")]
    // Waypoint 2: ���� ������ ������ (MissionBoard)
    public GameObject missionStartWaypoint;
    // Waypoint 3: ���� �������� (DeliveryZone)
    public GameObject missionDeliveryWaypoint;

    [Header("������ ������")]
    public List<Mission> availableMissions = new List<Mission>();
    public Mission activeMission;

    [Header("���������")]
    public float currentMoney = 0f;

    // === ������ ===

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

        // ���� ������ ���, ������� ��������, ��������� ������� ������� DeliveryZone
        if (availableMissions.Count == 0 && missionDeliveryWaypoint != null)
        {
            availableMissions.Add(new Mission
            {
                missionName = "�������� �������� (������� � ������������)",
                reward = 750f,
                deliveryLocation = missionDeliveryWaypoint.transform.position
            });
        }
    }

    // ���������� MissionStartWaypoint.cs, ����� ����� �������/������� �� ����
    public void SetMissionAvailable(bool isAvailable)
    {
        if (CurrentState == MissionState.None || CurrentState == MissionState.Available)
        {
            CurrentState = isAvailable ? MissionState.Available : MissionState.None;
        }
    }

    // ���������� �� UI (Mission Board), ����� ����� �������� ������
    public void StartMission(Mission missionToStart)
    {
        if (CurrentState != MissionState.Available) return;

        activeMission = missionToStart;
        CurrentState = MissionState.Active;

        // 1. ������ MissionBoard
        if (missionStartWaypoint != null) missionStartWaypoint.SetActive(false);

        // 2. ������������ DeliveryZone � ���������� ��� ������� (Waypoint 3)
        if (missionDeliveryWaypoint != null)
        {
            missionDeliveryWaypoint.transform.position = activeMission.deliveryLocation;
            missionDeliveryWaypoint.SetActive(true);
        }

        Debug.Log($"[Mission System] ������ {activeMission.missionName} �������!");

        // �������� ������ ����� ������ ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ���������� MissionDeliveryWaypoint.cs
    public void CompleteMission()
    {
        if (CurrentState != MissionState.Active) return;

        float reward = activeMission.reward;

        CurrentState = MissionState.None;
        currentMoney += reward;
        activeMission = null;

        // 1. ������ DeliveryZone
        if (missionDeliveryWaypoint != null) missionDeliveryWaypoint.SetActive(false);

        // 2. ������������ MissionBoard ��� ��������� ������
        if (missionStartWaypoint != null) missionStartWaypoint.SetActive(true);

        Debug.Log($"[Mission System] ������ ���������! ��������: ${reward}. ������: ${currentMoney}");
    }

    // ���������� MissionGiver'�� (Waypoint 1)
    public void GenerateNewMissions()
    {
        if (CurrentState == MissionState.Active) return;

        // � �������� ������� ����� ����� ������ ���������.
        // ��� �������: ������� � ��������� ��� ������.
        availableMissions.Clear();

        // ������ 1
        availableMissions.Add(new Mission
        {
            missionName = "������� ���� �� ����� A",
            reward = 1500f,
            deliveryLocation = missionDeliveryWaypoint.transform.position
        });

        // ������ 2
        availableMissions.Add(new Mission
        {
            missionName = "�������� �� ����� Z",
            reward = 2200f,
            deliveryLocation = missionDeliveryWaypoint.transform.position
        });

        // ���� UI ������, ��������� ���
        MissionBoardUIHandler uiHandler = missionStartWaypoint.GetComponent<MissionBoardUIHandler>();
        if (uiHandler != null && uiHandler.gameObject.activeSelf)
        {
            uiHandler.GenerateMissionList();
        }

        Debug.Log($"MissionGiver: ������������� {availableMissions.Count} ����� ������. ������ ����� � MissionBoard.");
    }
}