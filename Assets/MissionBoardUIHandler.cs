using UnityEngine;
using UnityEngine.UI; // ��� ������ � ����������� Button
using TMPro; // ��� ������ � TextMeshProUGUI

public class MissionBoardUIHandler : MonoBehaviour // <-- ��� ������ ������ ��������� � ������ �����
{
    private MissionManager manager;

    [Header("UI ��������")]
    // ������ ������/������ ������ (� ��� ������ ���� Button � TMP_Text)
    public GameObject missionEntryPrefab;
    // ������������ ������, ��� ����� ���������� ������ (Content � ScrollView)
    public Transform contentParent;

    void Awake()
    {
        // ������� ��������
        manager = FindObjectOfType<MissionManager>();
        if (manager == null)
        {
            Debug.LogError("MissionManager �� ������ � �����!");
        }
        // �������� ���� � ������
        gameObject.SetActive(false);
    }

    // ���������� ��� ���������� ������ ������ � UI
    public void GenerateMissionList()
    {
        if (manager == null || missionEntryPrefab == null || contentParent == null) return;

        // ������� ���������� ������
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (manager.availableMissions.Count == 0) return;

        // ������� ������ ��� ������ ��������� ������
        foreach (Mission mission in manager.availableMissions)
        {
            // ���������, ��� ������ ��������
            GameObject entry = Instantiate(missionEntryPrefab, contentParent);

            // ��������� ������
            TextMeshProUGUI nameText = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = $"{mission.missionName} (�������: ${mission.reward})";
            }

            // ��������� �������� ������
            Button button = entry.GetComponent<Button>();
            if (button != null)
            {
                // ���������� ������-������� ��� �������� ������ ������
                button.onClick.AddListener(() => AcceptMission(mission));
            }
        }
    }

    // ���������� ��� ������� �� ������ ������
    public void AcceptMission(Mission mission)
    {
        if (manager == null) return;

        // 1. ��������� ������
        manager.StartMission(mission);

        // 2. �������� UI
        gameObject.SetActive(false);
    }
}