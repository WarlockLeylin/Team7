using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionCreatorTrigger : MonoBehaviour
{
    [Tooltip("Перетащите сюда корневой объект панели UI для создания миссий.")]
    public GameObject creatorUIPanel;
    
    [Tooltip("Клавиша для открытия меню создания миссий.")]
    public KeyCode interactionKey = KeyCode.K; 

    private bool playerIsInZone = false;

    void Start()
    {
        if (creatorUIPanel == null)
        {
            Debug.LogError("CreatorUIPanel не назначен в инспекторе MissionCreatorTrigger.cs!");
        }
        
        if (creatorUIPanel != null)
        {
            creatorUIPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            if (creatorUIPanel != null && !creatorUIPanel.activeSelf)
            {
                creatorUIPanel.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Debug.Log($"UI создания миссий открыт. Нажмите ESC или {interactionKey} для закрытия.");
            }
        }
        
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
            
            if (creatorUIPanel != null && creatorUIPanel.activeSelf)
            {
                CloseCreatorUI();
            }
        }
    }

    private void CloseCreatorUI()
    {
        creatorUIPanel.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("UI создания миссий закрыт.");
    }
}