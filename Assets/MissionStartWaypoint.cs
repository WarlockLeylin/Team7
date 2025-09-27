// В файле MissionStartWaypoint.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionStartWaypoint : MonoBehaviour
{
    private MissionManager manager;

    // Новая публичная ссылка на компонент UI Handler
    // Перетащите сюда ваш UI-Canvas с прикрепленным скриптом MissionBoardUIHandler.cs
    public MissionBoardUIHandler uiHandler;

    private bool playerIsInZone = false;
    private KeyCode interactionKey = KeyCode.E;

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();

        if (manager == null || uiHandler == null)
        {
            Debug.LogError("MissionManager или UI Handler не назначен!");
        }
    }

    void Update()
    {
        // Логика открытия UI по кнопке E
        if (playerIsInZone && Input.GetKeyDown(interactionKey) && manager.CurrentState == MissionManager.MissionState.Available)
        {
            if (uiHandler != null)
            {
                uiHandler.GenerateMissionList(); // Обновляем список перед показом
                uiHandler.gameObject.SetActive(true); // Активируем UI (Canvas)

                // Отпускаем курсор для выбора
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Debug.Log("Mission Board UI показан. Нажмите ESC для выхода.");
            }
        }

        // Логика закрытия UI по кнопке ESC
        if (uiHandler != null && uiHandler.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            uiHandler.gameObject.SetActive(false);

            // Блокируем курсор обратно
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Mission Board UI закрыт.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && manager.CurrentState == MissionManager.MissionState.None)
        {
            playerIsInZone = true;
            manager.SetMissionAvailable(true);
            Debug.Log($"[Подсказка] Нажмите {interactionKey}, чтобы открыть Доску Миссий.");
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

            // Скрываем UI при выходе из зоны
            if (uiHandler != null && uiHandler.gameObject.activeSelf)
            {
                uiHandler.gameObject.SetActive(false);
            }

            // Если игрок ушел, блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}