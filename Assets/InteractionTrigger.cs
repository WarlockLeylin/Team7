using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionTrigger : MonoBehaviour
{
    private bool playerIsInZone = false;
    public KeyCode interactionKey = KeyCode.E;

    void Update()
    {
        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            MissionManager manager = FindObjectOfType<MissionManager>();
            if (manager != null)
            {
                // Запускаем генерацию новых миссий
                manager.GenerateNewMissions();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            Debug.Log($"[Подсказка] Нажмите {interactionKey}, чтобы получить новые миссии у Работодателя.");
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