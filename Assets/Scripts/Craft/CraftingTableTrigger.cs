// ���������� ���� ������ � ������ Crafting Table
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CraftingTableTrigger : MonoBehaviour
{
    private bool playerIsInZone = false;
    public KeyCode interactionKey = KeyCode.H;

    void Update()
    {
        if (playerIsInZone && Input.GetKeyDown(interactionKey))
        {
            if (CraftingManager.Instance != null)
            {
                // ��������� UI. ��������� ���������, ����� ����� ���� ������� ���
                bool currentState = CraftingManager.Instance.craftingUI.activeSelf;
                CraftingManager.Instance.ToggleCraftingUI(!currentState);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            Debug.Log($"[���������] ������� {interactionKey}, ����� ������������ ��������� ����.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            // ������� UI, ���� ����� ������ ����
            if (CraftingManager.Instance != null && CraftingManager.Instance.craftingUI.activeSelf)
            {
                CraftingManager.Instance.ToggleCraftingUI(false);
            }
        }
    }
}