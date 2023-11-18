using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    [SerializeField] private Image image;
    public Sprite pauseSprite, pauseSpriteToggled, playSprite, playSpriteToggled;

    private void Awake() {
        image.sprite = pauseSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetPaused())
        {
            image.sprite = playSpriteToggled;
        }
        else 
        {
            image.sprite = pauseSpriteToggled;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetPaused())
        {
            image.sprite = playSprite;
        }
        else
        {
            image.sprite = pauseSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (image.sprite == pauseSpriteToggled) image.sprite = playSpriteToggled;

        else image.sprite = pauseSpriteToggled;
    }

    private bool GetPaused()
    {
        return Time.timeScale == 0;
    }
}