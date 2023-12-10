using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace yourAlias
{
    public class WordButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        GameLoopManager loopManager;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            { loopManager.AddGuessedWord(); }
            else if (eventData.button == PointerEventData.InputButton.Middle)
                Debug.Log("Middle click");
            else if (eventData.button == PointerEventData.InputButton.Right)
            { loopManager.AddSkippedWord(); }
        }
    }
}