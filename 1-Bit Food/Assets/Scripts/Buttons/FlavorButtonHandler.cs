using UnityEngine;
using UnityEngine.EventSystems;

public class FlavorButtonHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
    public Flavor flavor;
    [SerializeField] private FeedbackUIManager feedbackManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (feedbackManager != null) feedbackManager.UpdateTotalMenu(flavor);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (feedbackManager != null) feedbackManager.UpdateTotalMenu(flavor);
    }
}