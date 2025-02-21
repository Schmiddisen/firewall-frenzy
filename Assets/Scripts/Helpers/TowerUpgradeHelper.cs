using UnityEngine;

public class TowerUpgradeHelper : MonoBehaviour
{
    // Folder path for the sprites
    private const string spriteFolder = "Sprites/";

    // Method to upgrade the tower
    public void UpgradeTower(Tower tower, string upgradePath, int level)
    {
        // Check if the upgrade path and level are valid
        if (upgradePath != "PathA" && upgradePath != "PathB" || level < 1 || level > 3)
        {
            Debug.LogError("Invalid upgrade path or level");
            return;
        }

        // Get the name of the tower
        string towerName = tower.name;

        // convert upgrade path from string to in
        int upgradePathInt = upgradePath == "PathA" ? 1 : 2;

        // Set the sprites for the child components
        SetBaseSprite(upgradePathInt, level, towerName);
        SetGunSprite(upgradePathInt, level, towerName);
    }

    private void SetBaseSprite(int upgradePath, int level, string towerName)
    {
        // Create the path to the sprite
        string spritePath = spriteFolder + towerName + "/" + towerName + " " + upgradePath + "." + level;

        // Load all sprites from the texture
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);

        // Check if the sprites were loaded
        if (sprites != null && sprites.Length > 0)
        {
            // Find the base sprite renderer
            SpriteRenderer baseSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];

            // Set the base sprite
            baseSpriteRenderer.sprite = sprites[0];
        }
        else
        {
            Debug.LogError("Sprites not found: " + spritePath);
        }
    }

    private void SetGunSprite(int upgradePath, int level, string towerName)
    {
        // Create the path to the sprite
        string spritePath = spriteFolder + towerName + "/" + towerName + " " + upgradePath + "." + level;

        // Load all sprites from the texture
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);

        // Check if the sprites were loaded
        if (sprites != null && sprites.Length > 1)
        {
            // Find the gun sprite renderer
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            SpriteRenderer gunSpriteRenderer = spriteRenderers[1];

            // Set the gun sprite
            gunSpriteRenderer.sprite = sprites[1];
        }
        else
        {
            Debug.LogError("Sprites not found: " + spritePath);
        }
    }
}
