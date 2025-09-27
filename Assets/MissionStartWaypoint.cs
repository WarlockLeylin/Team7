// � ����� MissionStartWaypoint.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionStartWaypoint : MonoBehaviour
{
    private MissionManager manager;

    // ����� ��������� ������ �� ��������� UI Handler
    // ���������� ���� ��� UI-Canvas � ������������� �������� MissionBoardUIHandler.cs
    public MissionBoardUIHandler uiHandler;

    private bool playerIsInZone = false;
    private KeyCode interactionKey = KeyCode.E;

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();

        if (manager == null || uiHandler == null)
        {
            Debug.LogError("MissionManager ��� UI Handler �� ��������!");
        }
    }

    void Update()
    {
        // ������ �������� UI �� ������ E
        if (playerIsInZone && Input.GetKeyDown(interactionKey) && manager.CurrentState == MissionManager.MissionState.Available)
        {
            if (uiHandler != null)
            {
                uiHandler.GenerateMissionList(); // ��������� ������ ����� �������
                uiHandler.gameObject.SetActive(true); // ���������� UI (Canvas)

                // ��������� ������ ��� ������
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Debug.Log("Mission Board UI �������. ������� ESC ��� ������.");
            }
        }

        // ������ �������� UI �� ������ ESC
        if (uiHandler != null && uiHandler.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            uiHandler.gameObject.SetActive(false);

            // ��������� ������ �������
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Mission Board UI ������.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && manager.CurrentState == MissionManager.MissionState.None)
        {
            playerIsInZone = true;
            manager.SetMissionAvailable(true);
            Debug.Log($"[���������] ������� {interactionKey}, ����� ������� ����� ������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            if (manager.CurrentState == MissionManager.MissionState.Available)
            {
                manager.SetMissionAvailable(false);
            }

            // �������� UI ��� ������ �� ����
            if (uiHandler != null && uiHandler.gameObject.activeSelf)
            {
                uiHandler.gameObject.SetActive(false);
            }

            // ���� ����� ����, ��������� ������
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}