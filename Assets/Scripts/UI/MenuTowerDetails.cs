using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuTowerDetails : MonoBehaviour
{
    [Header("UIDocument")]
    public UIDocument uIDocument;


    void OnEnable()
    {
        Button[] buttons = uIDocument.rootVisualElement
            .Query<Button>()
            .ToList()
            .Where(b => b.name.StartsWith("TS_"))
            .ToArray();

        foreach (Button button in buttons)
        {  
            button.clicked += () => changePriority(button);
        }
    }

    public void changePriority(Button button) {
        int index = int.Parse(button.name[3].ToString());
        Tower tower = LevelManager.main.selectedTower;
        tower.targetPrio = (TargetingPriority) Enum.GetValues(typeof(TargetingPriority)).GetValue(index);
        showTowerInfos(uIDocument);
    }

    public void showTowerInfos(UIDocument doc)
    {
        if (doc == null)
        {
            Debug.LogError("UIDocument is not selected in LevelManager");
            return;
        }

        Tower tower = LevelManager.main.selectedTower;
        Label TowerLabelName = doc.rootVisualElement.Q<Label>("Label_Tower_Information");
        Label TowerLabelDMG = doc.rootVisualElement.Q<Label>("Damage_Value");
        Label TowerLabelPriority = doc.rootVisualElement.Q<Label>("Priority_value");
        VisualElement bar = doc.rootVisualElement.Q<VisualElement>("Tower_Information_Bottom");

        if (tower == null)
        {
            TowerLabelName.text = "No Tower selected";
            bar.AddToClassList("hidden");
            return;
        }
        bar.RemoveFromClassList("hidden");
        TowerLabelName.text = tower.name;
        TowerLabelDMG.text = tower.baseDMG.ToString();
        int nextCapitalIndex = -1;
        for (int i = 1; i < tower.targetPrio.ToString().Length; i++)
        {
            if (char.IsUpper(tower.targetPrio.ToString()[i]))
            {
                nextCapitalIndex = i;
                break;
            }
        }
        TowerLabelPriority.text = nextCapitalIndex != -1 ? tower.targetPrio.ToString().Substring(0, nextCapitalIndex) : tower.targetPrio.ToString();
    }
}