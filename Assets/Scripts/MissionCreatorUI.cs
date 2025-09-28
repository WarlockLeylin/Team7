using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using TMPro; 

public class MissionCreatorUI : MonoBehaviour
{
    [Header("Основные поля миссии")]
    public TMP_InputField missionNameInput; 
    public TMP_InputField rewardInput; 
    
    [Header("Поля требуемых ресурсов")]
    public TMP_InputField metalAmountInput; 
    public TMP_InputField gunpowderAmountInput; 
    
    public Button createMissionButton; 

    void Start()
    {
        createMissionButton.onClick.AddListener(CreateMission);
    }

    public void CreateMission()
    {
        string missionName = missionNameInput.text;
        float reward = 0f;
        int metalAmount = 0;
        int gunpowderAmount = 0;

        if (!float.TryParse(rewardInput.text, out reward) || reward <= 0)
        {
            Debug.LogError("Неверный формат или значение награды!");
            return;
        }

        if (!int.TryParse(metalAmountInput.text, out metalAmount) || metalAmount < 0)
        {
            Debug.LogError("Неверное количество Металла!");
            return;
        }
        
        if (!int.TryParse(gunpowderAmountInput.text, out gunpowderAmount) || gunpowderAmount < 0)
        {
            Debug.LogError("Неверное количество Пороха!");
            return;
        }

        List<ResourceRequirement> requiredResources = new List<ResourceRequirement>();
        
        if (metalAmount > 0)
        {
            requiredResources.Add(new ResourceRequirement { resourceName = "Металл", amount = metalAmount });
        }
        
        if (gunpowderAmount > 0)
        {
            requiredResources.Add(new ResourceRequirement { resourceName = "Порох", amount = gunpowderAmount });
        }

        MissionManager manager = FindObjectOfType<MissionManager>();
        if (manager != null)
        {
            Mission mission = new Mission
            {
                missionName = missionName,
                reward = reward,
                requiredResources = requiredResources,
                deliveryLocation = Vector3.zero,
                requesterInventory = Inventory.instance
            };
            manager.availableMissions.Add(mission);
            Debug.Log($"Миссия '{missionName}' создана и добавлена в список доступных!");
        }
        
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}