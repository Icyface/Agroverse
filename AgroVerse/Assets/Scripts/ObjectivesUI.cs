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
    private bool pigCleaned = false;
    private bool cowMilked = false;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        eggsText.text = (eggs >= 3 ? "☑" : "☐") + " Huevos: " + eggs + "/3";
        feedText.text = (feed >= 3 ? "☑" : "☐") + " Alimentar animales: " + feed + "/3";
        pigText.text = (pigCleaned ? "☑" : "☐") + " Cerdo limpio: " + (pigCleaned ? "1/1" : "0/1");
        cowText.text = (cowMilked ? "☑" : "☐") + " Vaca ordeñada: " + (cowMilked ? "1/1" : "0/1");
    }

    public void AddEgg()
    {
        if (eggs < 3)
        {
            eggs++;

            if (eggs >= 3)
                TaskManager.Instance.CompleteTask("recoger_huevo");

            UpdateUI();
        }
    }

    public void FeedAnimal()
    {
        if (feed < 3)
        {
            feed++;

            if (feed >= 3)
                TaskManager.Instance.CompleteTask("alimentar_chicken");

            UpdateUI();
        }
    }

    public void CleanPig()
    {
        if (!pigCleaned)
        {
            pigCleaned = true;

            TaskManager.Instance.CompleteTask("limpiar_cerdo");

            UpdateUI();
        }
    }

    public void MilkCow()
    {
        if (!cowMilked)
        {
            cowMilked = true;

            TaskManager.Instance.CompleteTask("munyir_vaca");

            UpdateUI();
        }
    }
}