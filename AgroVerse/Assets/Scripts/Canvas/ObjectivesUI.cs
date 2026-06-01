using TMPro;
using UnityEngine;

public class ObjectivesUI : MonoBehaviour
{
    [Header("Textos UI")]
    public TMP_Text eggsText;
    public TMP_Text feedText;
    public TMP_Text pigText;
    public TMP_Text cowText;

    public int eggs { get; private set; } = 0;
    public int feed { get; private set; } = 0;
    public bool pigCleaned { get; private set; } = false;
    public bool cowMilked { get; private set; } = false;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        eggsText.text = (eggs >= 3 ? "☑" : "☐") + " Recoger huevos: " + eggs + "/3";
        feedText.text = (feed >= 2 ? "☑" : "☐") + " Alimentar gallinas: " + feed + "/2";
        pigText.text = (pigCleaned ? "☑" : "☐") + " Cerdo limpio: " + (pigCleaned ? "1/1" : "0/1");
        cowText.text = (cowMilked ? "☑" : "☐") + " Vaca ordeñada: " + (cowMilked ? "1/1" : "0/1");
    }

    public void AddEgg()
    {
        if (eggs < 3)
        {
            eggs++;
            if (eggs >= 3)
            {
                if (TaskManager.Instance != null) TaskManager.Instance.CompleteTask("recoger_huevo");
            }
            UpdateUI();
        }
    }

    public void FeedAnimal()
    {
        if (feed < 2)
        {
            feed++;
            if (feed >= 2)
            {
                if (TaskManager.Instance != null) TaskManager.Instance.CompleteTask("alimentar_chicken");
            }
            UpdateUI();
        }
    }

    public void CleanPig()
    {
        if (!pigCleaned)
        {
            pigCleaned = true;
            if (TaskManager.Instance != null) TaskManager.Instance.CompleteTask("limpiar_cerdo");
            UpdateUI();
        }
    }

    public void MilkCow()
    {
        if (!cowMilked)
        {
            cowMilked = true;
            if (TaskManager.Instance != null) TaskManager.Instance.CompleteTask("munyir_vaca");
            UpdateUI();
        }
    }

    public void SetForcedValues(int forcedEggs, int forcedFeed, bool forcedPig, bool forcedCow)
    {
        eggs = forcedEggs;
        feed = forcedFeed;
        pigCleaned = forcedPig;
        cowMilked = forcedCow;
        UpdateUI();
    }
}