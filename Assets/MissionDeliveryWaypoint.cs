using UnityEngine;

// Требует наличия Collider, настроенного как триггер
[RequireComponent(typeof(Collider))]
public class MissionDeliveryWaypoint : MonoBehaviour
{
    private MissionManager manager;

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это объект с тегом "Truck" или "Car"
        if (other.CompareTag("Truck") || other.CompareTag("Car"))
        {
            // Убеждаемся, что миссия активна
            if (manager.CurrentState == MissionManager.MissionState.Active)
            {
                manager.CompleteMission();
            }
        }
    }
}