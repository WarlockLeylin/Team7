using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionBoardUIHandler : MonoBehaviour
{
    private MissionManager manager;

    [Header("UI Элементы")]
    public GameObject missionEntryPrefab;
    public Transform contentParent;

    void Awake()
    {
        manager = FindObjectOfType<MissionManager>();
        if (manager == null)
        {
            Debug.LogError("MissionManager не найден в сцене!");
        }
        gameObject.SetActive(false);
    }

    public void GenerateMissionList()
    {
        if (manager == null || missionEntryPrefab == null || contentParent == null) return;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (manager.availableMissions.Count == 0)
        {
            Debug.Log("Нет доступных миссий для отображения.");
            return;
        }

        foreach (Mission mission in manager.availableMissions)
        {
            GameObject entry = Instantiate(missionEntryPrefab, contentParent);

            TextMeshProUGUI nameText = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = $"{mission.missionName} (Награда: ${mission.reward})";
            }
            Button button = entry.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => AcceptMission(mission));
            }
        }
    }

    public void AcceptMission(Mission mission)
    {
        if (manager == null) return;

        manager.StartMission(mission);

        gameObject.SetActive(false);
    }
}