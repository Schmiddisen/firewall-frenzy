using TMPro;
using UnityEngine;

public class PacketDefenderBasicBulletAnimator : MonoBehaviour
{
    [SerializeField] public TextMeshPro tmpText;
    [SerializeField] private int bitSeriesLength;
    [SerializeField] private float animationFrequency;

    private float timePassed = 0f;
    void Start()
    {
        tmpText.text = randomBitSeries();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= animationFrequency) {
            timePassed = 0;
            tmpText.text = randomBitSeries();
        }

    }

    private string randomBitSeries() {
        string bitString = "";
        
        
        for (int i = 0; i < bitSeriesLength; i++)
        {
            bitString += Random.Range(0,2).ToString();
        }
        //Debug.Log(bitString);
        return bitString;
    }
}
