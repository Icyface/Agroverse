using TMPro;
using UnityEngine;

public class ObjectivesUI : MonoBehaviour
{
    [Header("Textos UI")]
    public TMP_Text eggsText;
    public TMP_Text feedText;
    public TMP_Text pigText;
    public TMP_Text cowText;

    private int eggs = 0;
    private int feed = 0;
    private int pig = 0;
    private int cow = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        eggsText.text = (eggs >= 3 ? "☑" : "☐") + " Huevos: " + eggs + "/3";
        feedText.text = (feed >= 3 ? "☑" : "☐") + " Alimentar animales: " + feed + "/3";
        pigText.text = (pig >= 1 ? "☑" : "☐") + " Cerdo limpio: " + pig + "/1";
        cowText.text = (cow >= 1 ? "☑" : "☐") + " Vaca ordeñada: " + cow + "/1";
    }

    public void AddEgg()
    {
        if (eggs < 3)
        {
            eggs++;
            UpdateUI();
        }
    }

    public void FeedAnimal()
    {
        if (feed < 3)
        {
            feed++;
            UpdateUI();
        }
    }

    public void CleanPig()
    {
        pig = 1;
        UpdateUI();
    }

    public void MilkCow()
    {
        cow = 1;
        UpdateUI();
    }
}