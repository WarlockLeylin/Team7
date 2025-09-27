using UnityEngine;

public class CarEnterExit : MonoBehaviour
{
    // --- Настройка в Инспекторе ---
    public GameObject PlayerRoot;
    public MonoBehaviour playerControllerScript;
    public Transform ExitPoint;
    public Transform InteractionPoint;
    public KeyCode InteractionKey = KeyCode.E;

    [Header("Camera Switching")]
    // Используем полные пути, чтобы избежать ошибки импорта:
    public Unity.Cinemachine.CinemachineVirtualCamera PlayerCM;
    public Unity.Cinemachine.CinemachineCamera TruckCM;

    // --- Скрипты и Состояние ---
    private MonoBehaviour truckControllerScript;
    private CharacterController playerCharController;
    private bool isPlayerInCar = false;
    private const float EnterDistance = 2.5f;
    private Rigidbody truckRigidbody;


    void Start()
    {
        truckControllerScript = GetComponent<TruckController>();
        playerCharController = PlayerRoot.GetComponent<CharacterController>();
        truckRigidbody = GetComponent<Rigidbody>();

        truckControllerScript.enabled = false;

        // Устанавливаем начальный приоритет камер
        if (PlayerCM != null) PlayerCM.Priority = 11;
        if (TruckCM != null) TruckCM.Priority = 9;
    }

    void Update()
    {
        if (Input.GetKeyDown(InteractionKey))
        {
            if (isPlayerInCar)
            {
                ExitCar();
            }
            else
            {
                TryEnterCar();
            }
        }
    }

    void TryEnterCar()
    {
        float distance = Vector3.Distance(InteractionPoint.position, ExitPoint.position);
        Debug.Log($"Дистанция: {distance:F2}. Требуется: < 2.5");

        if (distance < 2.5f)
        {
            EnterCar();
        }
    }

    void EnterCar()
    {
        isPlayerInCar = true;

        // 1. Отключение управления персонажем
        if (playerControllerScript != null) playerControllerScript.enabled = false;
        if (playerCharController != null) playerCharController.enabled = false;

        // 2. Привязка к сиденью
        PlayerRoot.transform.SetParent(ExitPoint);
        PlayerRoot.transform.localPosition = Vector3.zero;
        PlayerRoot.transform.localRotation = Quaternion.identity;

        // 3. Включение управления грузовиком
        truckControllerScript.enabled = true;

        // 4. Переключение камер
        if (TruckCM != null && PlayerCM != null)
        {
            TruckCM.Priority = 11;
            PlayerCM.Priority = 9;
        }
    }

    void ExitCar()
    {
        isPlayerInCar = false;

        // 1. Отсоединение от грузовика
        PlayerRoot.transform.SetParent(null);

        // 2. Включение управления персонажем
        if (playerControllerScript != null) playerControllerScript.enabled = true;
        if (playerCharController != null) playerCharController.enabled = true;

        // 4. Высаживание рядом с дверью
        PlayerRoot.transform.position = ExitPoint.position + (ExitPoint.right * -2.5f);

        // 3. Отключение управления грузовиком
        truckControllerScript.enabled = false;

        if (truckRigidbody != null)
        {
            truckRigidbody.linearVelocity = Vector3.zero;
            truckRigidbody.angularVelocity = Vector3.zero;
        }

        // 5. Возвращение камер
        if (TruckCM != null && PlayerCM != null)
        {
            TruckCM.Priority = 9;
            PlayerCM.Priority = 11;
        }
    }
}