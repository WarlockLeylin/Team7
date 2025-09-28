using UnityEngine;
using UnityEngine.UI; // Для работы с компонентом Button
using TMPro; // Для работы с TextMeshProUGUI

public class MissionBoardUIHandler : MonoBehaviour // <-- Имя класса должно совпадать с именем файла
{
    private MissionManager manager;

    [Header("UI Элементы")]
    // Префаб кнопки/строки миссии (в нем должны быть Button и TMP_Text)
    public GameObject missionEntryPrefab;
    // Родительский объект, где будут спавниться кнопки (Content в ScrollView)
    public Transform contentParent;

    void Awake()
    {
        // Находим менеджер
        manager = FindObjectOfType<MissionManager>();
        if (manager == null)
        {
            Debug.LogError("MissionManager не найден в сцене!");
        }
        // Скрываем себя в начале
        gameObject.SetActive(false);
    }

    // Вызывается для обновления списка миссий в UI
    public void GenerateMissionList()
    {
        if (manager == null || missionEntryPrefab == null || contentParent == null) return;

        // Очищаем предыдущие записи
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (manager.availableMissions.Count == 0)
        {
            Debug.Log("Нет доступных миссий для отображения.");
            return;
        }

        // Создаем кнопку для каждой доступной миссии
            foreach (Mission mission in manager.availableMissions)
            {
                // Убедитесь, что префаб назначен
                GameObject entry = Instantiate(missionEntryPrefab, contentParent);

                // Настройка текста
                TextMeshProUGUI nameText = entry.GetComponentInChildren<TextMeshProUGUI>();
                if (nameText != null)
                {
                    nameText.text = $"{mission.missionName} (Награда: ${mission.reward})";
                }

                // Настройка действия кнопки
                Button button = entry.GetComponent<Button>();
                if (button != null)
                {
                    // Используем лямбда-функцию для передачи данных миссии
                    button.onClick.AddListener(() => AcceptMission(mission));
                }
            }
    }

    // Вызывается при нажатии на кнопку миссии
    public void AcceptMission(Mission mission)
    {
        if (manager == null) return;

        // 1. Запускаем миссию
        manager.StartMission(mission);

        // 2. Скрываем UI
        gameObject.SetActive(false);
    }
}