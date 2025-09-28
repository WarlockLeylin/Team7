using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionStartWaypoint : MonoBehaviour
{
    private MissionManager manager;

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
                uiHandler.GenerateMissionList();
                uiHandler.gameObject.SetActive(true); // Активируем UI (Canvas)

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Debug.Log("Mission Board UI показан. Нажмите ESC для выхода.");
            }
        }

        if (uiHandler != null && uiHandler.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            uiHandler.gameObject.SetActive(false);

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

            if (uiHandler != null && uiHandler.gameObject.activeSelf)
            {
                uiHandler.gameObject.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}