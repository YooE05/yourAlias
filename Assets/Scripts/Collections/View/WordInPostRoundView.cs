using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WordInPostRoundView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI discriptionTMP;
    [SerializeField] private Button toggleButton;



    internal void SetName(string wordName)
    {
        nameTMP.text = wordName;
    }

    internal void SetDiscription(string wordDiscription)
    {
        discriptionTMP.text = wordDiscription;
    }

    internal Button GetToggleButton()
    {
        return toggleButton;
    }
}
