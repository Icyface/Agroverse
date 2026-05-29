using UnityEngine;

public class CloseTutorial : MonoBehaviour
{
   public GameObject panel;

    public void Close()
    {
        panel.SetActive(false);
    }
}