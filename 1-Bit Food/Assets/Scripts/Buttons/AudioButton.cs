using UnityEngine;
using UnityEngine.EventSystems;

public class AudioButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler {

    public int audioIndex = 5;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayUIClip(audioIndex);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.instance.PlayUIClip(audioIndex);
    }
}