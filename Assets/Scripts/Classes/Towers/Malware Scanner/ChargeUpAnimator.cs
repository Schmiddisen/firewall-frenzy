using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering;

public class ChargeUpAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer sp;
    [SerializeField] Tower tower;
    void Start()
    {
        
    }

    void Update()
    {
        if (!tower) return;
        
        float currentAPS = tower.currentAPS;
        float accumulatedStagger = (float) tower.accumulatedStagger;

        float alpha = math.remap(0, (1f / currentAPS) * (1 + accumulatedStagger), 0, 1, tower.timeUntilFire);

        Color newAlphaColor = sp.color;
        newAlphaColor.a = alpha;
        sp.color = newAlphaColor;
    }
}
