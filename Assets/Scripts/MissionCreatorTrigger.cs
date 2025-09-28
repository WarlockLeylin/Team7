using UnityEngine;

// Требует наличия Collider, настроенного как триггер
[RequireComponent(typeof(Collider))]
public class MissionCreatorTrigger : MonoBehaviour
{
    [Tooltip("Перетащите сюда корневой объект панели UI для создания миссий.")]
    public GameObject creatorUIPanel;
    
    [Tooltip("Клавиша для открытия меню создания миссий.")]
    public KeyCode interactionKey = KeyCode.K; // Используем K, чтобы избежать конфликта с E (по умолчанию)

    private bool playerIsInZone = false;

    void Start()
    {
        if (creatorUIPanel == null)
        {
            Debug.LogError("CreatorUIPanel не назначен в инспекторе MissionCreatorTrigger.cs!");
        }
        
        // Убедимся, что панель изначально скрыта
        if (creatorUIPanel != null)
        {
            creatorUIPanel.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Логика открытия UI
        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            // Открываем, только если она не активна
            if (creatorUIPanel != null && !creatorUIPanel.activeSelf)
            {
                creatorUIPanel.SetActive(true);
                
                // Отпускаем курсор для взаимодействия с UI
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Debug.Log($"UI создания миссий открыт. Нажмите ESC или {interactionKey} для закрытия.");
            }
        }
        
        // 2. Логика закрытия UI по кнопке ESC (стандартное поведение)
        if (creatorUIPanel != null && creatorUIPanel.activeSelf && (Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseCreatorUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            Debug.Log($"[Подсказка] Нажмите {interactionKey}, чтобы создать миссию.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            Debug.Log("Подсказка скрыта.");
            
            // Если игрок уходит, закрываем UI для предотвращения ошибок
            if (creatorUIPanel != null && creatorUIPanel.activeSelf)
            {
                CloseCreatorUI();
            }
        }
    }

    private void CloseCreatorUI()
    {
        creatorUIPanel.SetActive(false);
        
        // Блокируем курсор, чтобы игрок мог снова двигаться
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("UI создания миссий закрыт.");
    }
}