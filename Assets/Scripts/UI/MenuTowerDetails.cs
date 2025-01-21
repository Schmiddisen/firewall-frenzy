using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuTowerDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text detailsText;
    [SerializeField] TMP_Dropdown dropdown;
    void Start()
    {

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        foreach (TargetingPriority prio in Enum.GetValues(typeof(TargetingPriority)))
        {
             dropdownOptions.Add(new TMP_Dropdown.OptionData(prio.ToString()));
        }

        if (this.dropdown) {
             dropdown.AddOptions(dropdownOptions);
             dropdown.onValueChanged.AddListener(changePriority);
             dropdown.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        
    }

    public void changePriority(int index) {
        Tower tower = LevelManager.main.selectedTower;
        tower.targetPrio = (TargetingPriority) Enum.GetValues(typeof(TargetingPriority)).GetValue(index);
    }

    public void showTowerInfos() {
        Tower tower = LevelManager.main.selectedTower;
        bool isTowerSelected = tower != null;

        dropdown.gameObject.SetActive(isTowerSelected);

        if (!isTowerSelected) {
            detailsText.text = "Select Tower";
            return;
        }

        int index = 0;
        foreach (TargetingPriority prio in Enum.GetValues(typeof(TargetingPriority)))
        {
            if (prio == tower.targetPrio) break;
            index++;
        }
        dropdown.value = index;
        dropdown.RefreshShownValue();

        detailsText.text = tower.name;
    }
}
