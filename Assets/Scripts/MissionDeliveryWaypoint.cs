using UnityEngine;

// Требует наличия Collider, настроенного как триггер
[RequireComponent(typeof(Collider))]
public class MissionDeliveryWaypoint : MonoBehaviour
{
    private MissionManager manager;
    private bool missionCompleted = false; // Флаг, чтобы избежать многократного вызова

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();
        if (manager == null)
        {
            Debug.LogError("MissionManager не найден в сцене!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null || missionCompleted) return;

        // Проверяем, что это объект с тегом "Truck" или "Car"
        if (other.CompareTag("Truck") || other.CompareTag("Car"))
        {
            // Убеждаемся, что миссия активна
            if (manager.CurrentState == MissionManager.MissionState.Active)
            {
                // Пометка, что миссия выполнена, и предотвращение повторных вызовов
                missionCompleted = true; 
                manager.CompleteMission();
                // Так как CompleteMission сбрасывает состояние и, предположительно, 
                // деактивирует этот Waypoint, missionCompleted сбросится в следующем вызове Start/OnEnable
                // Но для надежности лучше его сбросить только если он снова активируется.
                // Если объект деактивируется (как в MissionManager), 
                // флаг missionCompleted останется true. 
                // При следующем включении объекта нужно сбросить флаг в OnEnable.
            }
        }
    }
    
    // Используем OnEnable для сброса флага, так как объект будет деактивироваться/активироваться
    private void OnEnable()
    {
        missionCompleted = false;
    }
}