using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class TowerJSON
{
    public string name;
    public string imagePath;
    public string infoText;
    public Upgrade[] upgrades;
}

[Serializable]
public class Upgrade
{
    public string description;
}

[Serializable]
public class TowerDatabase
{
    public TowerJSON[] towers;
}

public class MenuTowerDetails : MonoBehaviour
{
    [Header("UIDocument")]
    public UIDocument uIDocument;

    [Header("JSON")]
    public TextAsset towerInfos;
    private TowerDatabase towerData;


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

        Button[] upgradeButtons = uIDocument.rootVisualElement
            .Query<Button>()
            .ToList()
            .Where(b => b.name.StartsWith("UP_"))
            .ToArray();
        foreach (Button button in upgradeButtons)
        {  
            button.clicked += () => upgrade(button);
        }
    }

    public void upgrade(Button button) {
        String btnName = button.name;

        UpgradePath path = btnName == "UP_1_Button" ? UpgradePath.PathA : UpgradePath.PathB;

        Tower tower = LevelManager.main.selectedTower;
        if(tower == null) return;

        tower.upgrade(path);


        showTowerInfos(this.uIDocument, this.towerInfos);
    }

    public void changePriority(Button button) {
        int index = int.Parse(button.name[3].ToString());
        Tower tower = LevelManager.main.selectedTower;
        tower.targetPrio = (TargetingPriority) Enum.GetValues(typeof(TargetingPriority)).GetValue(index);
        showTowerInfos(this.uIDocument, this.towerInfos);
    }

    public void updateCurrency(UIDocument doc, int currency)
    {
        Label currencyLabel = doc.rootVisualElement.Q<Label>("Currency_value");
        currencyLabel.text = currency.ToString();
    }

    public void showTowerInfos(UIDocument doc, TextAsset towerInfos)
    {
        towerData = JsonUtility.FromJson<TowerDatabase>(towerInfos.text);

        if (doc == null)
        {
            Debug.LogError("UIDocument is not selected in LevelManager");
            return;
        }

        //UI References
        Label UP_1_Info = doc.rootVisualElement.Q<Label>("UP_1_Info");
        Label UP_2_Info = doc.rootVisualElement.Q<Label>("UP_2_Info");
        Label Tower_Informations_Text = doc.rootVisualElement.Q<Label>("Tower_Informations_Text");
        Tower tower = LevelManager.main.selectedTower;
        Label TowerLabelName = doc.rootVisualElement.Q<Label>("Label_Tower_Information");
        Label TowerLabelDMG = doc.rootVisualElement.Q<Label>("Damage_Value");
        Label TowerLabelAPS = doc.rootVisualElement.Q<Label>("Firerate_Value");
        Label TowerLabelPriority = doc.rootVisualElement.Q<Label>("Priority_value");
        VisualElement bar = doc.rootVisualElement.Q<VisualElement>("Tower_Information_Bottom");
        VisualElement Tower_preview = doc.rootVisualElement.Q<VisualElement>("Tower_preview");
        VisualElement Tower_preview_Image = doc.rootVisualElement.Q<VisualElement>("Tower_preview_Image");

        Button btnPathA = doc.rootVisualElement.Q<Button>("UP_1_Button");
        Button btnPathB = doc.rootVisualElement.Q<Button>("UP_2_Button");

        if (tower == null)
        {
            TowerLabelName.text = "No Tower selected";
            bar.AddToClassList("hidden");
            Tower_Informations_Text.AddToClassList("hidden");
            Tower_preview.AddToClassList("hidden");
            return;
        }
        bar.RemoveFromClassList("hidden");
        Tower_Informations_Text.RemoveFromClassList("hidden");
        Tower_preview.RemoveFromClassList("hidden");
        TowerLabelName.text = tower.name;
        TowerLabelDMG.text = tower.currentDMG.ToString();
        TowerLabelAPS.text = tower.currentAPS.ToString();


        //Priority
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
        //Upgrade Path
        //Debug.Log(tower.name);
        TowerJSON towerInfo = towerData.towers.FirstOrDefault(t => t.name == tower.name);
        if (towerInfo != null)
        {
            UP_1_Info.text = towerInfo.upgrades[0].description;
            UP_2_Info.text = towerInfo.upgrades[1].description;
        }
        Tower_Informations_Text.text = towerInfo.infoText;
        Texture2D texture = Resources.Load<Texture2D>(towerInfo.imagePath);
        if (texture == null)
        {
            Debug.LogError($"Failed to load texture: {towerInfo.imagePath}. Make sure the file is in 'Resources' and is a PNG/JPG.");
            return;
        }
        else
        {
            Tower_preview_Image.style.backgroundImage = new StyleBackground(texture);

        }
        //If tower is not yet active (not placed), disable the buttons
        if (!tower.isActiv) {
            btnPathA.SetEnabled(false);
            btnPathB.SetEnabled(false);
        }
        else if(tower.upgradePath != UpgradePath.Base) {
            //If a path is already selected, enable the correct path, and disable the other
            bool pathA = tower.upgradePath == UpgradePath.PathA;
            btnPathA.SetEnabled(pathA);
            btnPathB.SetEnabled(!pathA);
        }else {
            btnPathA.SetEnabled(true);
            btnPathB.SetEnabled(true);
        }
    }
}