using UnityEngine;
using System.Collections.Generic; // Добавлено для List<Mission>

[RequireComponent(typeof(Collider))]
public class InteractionTrigger : MonoBehaviour
{
    public GameObject PlayerRoot; // Корневой объект игрока
    private bool playerIsInZone = false;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Настройки миссий для выдачи")]
    // Замените этот список на MissionsToGive
    public List<Mission> MissionsToGive = new List<Mission>(); 
    // Предполагается, что Mission и ResourceRequirement определены где-то глобально
    // или в файле MissionManager.cs, и доступны.

    void Update()
    {
        // Проверяем, что не активна другая миссия, чтобы не сбивать логику
        MissionManager manager = FindObjectOfType<MissionManager>();

        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            if (manager != null)
            {
                // Запускаем генерацию новых миссий, передавая наш список
                manager.GenerateNewMissions(MissionsToGive);
                // После генерации новых миссий, игрок должен идти к MissionBoard
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