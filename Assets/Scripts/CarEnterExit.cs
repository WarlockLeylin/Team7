using UnityEngine;

public class CarEnterExit : MonoBehaviour
{
    public GameObject PlayerRoot;
    public MonoBehaviour playerControllerScript;
    public Transform ExitPoint;
    public Transform InteractionPoint;
    public KeyCode InteractionKey = KeyCode.E;

    [Header("Camera Switching")]
    public Unity.Cinemachine.CinemachineVirtualCamera PlayerCM;
    public Unity.Cinemachine.CinemachineCamera TruckCM;

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

        if (playerControllerScript != null) playerControllerScript.enabled = false;
        if (playerCharController != null) playerCharController.enabled = false;

        PlayerRoot.transform.SetParent(ExitPoint);
        PlayerRoot.transform.localPosition = Vector3.zero;
        PlayerRoot.transform.localRotation = Quaternion.identity;

        truckControllerScript.enabled = true;

        if (TruckCM != null && PlayerCM != null)
        {
            TruckCM.Priority = 11;
            PlayerCM.Priority = 9;
        }
    }

    void ExitCar()
    {
        isPlayerInCar = false;

        PlayerRoot.transform.SetParent(null);

        if (playerControllerScript != null) playerControllerScript.enabled = true;
        if (playerCharController != null) playerCharController.enabled = true;

        PlayerRoot.transform.position = ExitPoint.position + (ExitPoint.right * -2.5f);

        truckControllerScript.enabled = false;

        if (truckRigidbody != null)
        {
            truckRigidbody.linearVelocity = Vector3.zero;
            truckRigidbody.angularVelocity = Vector3.zero;
        }

        if (TruckCM != null && PlayerCM != null)
        {
            TruckCM.Priority = 9;
            PlayerCM.Priority = 11;
        }
    }
}