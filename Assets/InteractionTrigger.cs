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
                // ��������� ��������� ����� ������
                manager.GenerateNewMissions();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            Debug.Log($"[���������] ������� {interactionKey}, ����� �������� ����� ������ � ������������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            Debug.Log("��������� ������.");
        }
    }
}