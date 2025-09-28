using UnityEngine;
using System.Collections.Generic; 

[RequireComponent(typeof(Collider))]
public class InteractionTrigger : MonoBehaviour
{
    public GameObject PlayerRoot;
    private bool playerIsInZone = false;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Настройки миссий для выдачи")]
    public List<Mission> MissionsToGive = new List<Mission>(); 

    void Update()
    {
        MissionManager manager = FindObjectOfType<MissionManager>();

        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            if (manager != null)
            {
                manager.GenerateNewMissions(MissionsToGive);
                Debug.Log($"MissionGiver: Предложены новые миссии. Отправляйтесь к Mission Board!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            Debug.Log($"[Подсказка] Нажмите {interactionKey}, чтобы получить новые миссии.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            Debug.Log("Подсказка скрыта.");
        }
    }
}