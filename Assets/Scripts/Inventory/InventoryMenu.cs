using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryCanvas; // сюда перетащи Canvas инвентаря

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab — клавиша для открытия
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}
