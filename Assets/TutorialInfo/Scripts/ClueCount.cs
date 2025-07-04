using TMPro;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public TMP_Text clueText;
    private int clueCount = 0;

    public void AddClue()
    {
        clueCount++;
        clueText.text = "CLUE COUNT\n" + clueCount;
    }
}
