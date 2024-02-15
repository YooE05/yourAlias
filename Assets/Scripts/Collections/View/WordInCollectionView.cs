using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordInCollectionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI discriptionTMP;
    [SerializeField] private Button removeButton;



    internal void SetName(string wordName)
    {
        nameTMP.text = wordName;
    }

    internal void SetDiscription(string wordDiscription)
    {
        discriptionTMP.text = wordDiscription;
    }

    internal Button GetRemoveButton()
    {
        return removeButton;
    }
}
