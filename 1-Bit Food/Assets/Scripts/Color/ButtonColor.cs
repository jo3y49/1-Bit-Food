using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonColor : UIColor, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI tm;
    private TextColor tmScript;
    public bool flipColor, fliptext = true;

    protected override void OnEnable()
    {
        tm = GetComponentInChildren<TextMeshProUGUI>();

        if (tm != null) tmScript = tm.GetComponent<TextColor>();

        base.OnEnable();
    }

    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);

        if (tm == null || tmScript != null) return;

        tm.color = color;
    }

    private void FlipTextColor()
    {
        if (tmScript == null || !fliptext) return;

        if (tmScript.gameColor == ColorSwitcher.GameColor.Bright) tmScript.ChangeColor(ColorSwitcher.instance.Dark);

        else tmScript.ChangeColor(ColorSwitcher.instance.Bright);
    }

    private void ResetTextColor()
    {
        if (tmScript == null || !fliptext) return;

        if (tmScript.gameColor == ColorSwitcher.GameColor.Bright) tmScript.ChangeColor(ColorSwitcher.instance.Bright);

        else tmScript.ChangeColor(ColorSwitcher.instance.Dark);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!flipColor) return;

        FlipColor();
        FlipTextColor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!flipColor) return;

        SetColors();
        ResetTextColor();
    }

    private void FlipColor()
    {
        if (gameColor == ColorSwitcher.GameColor.Bright) ChangeColor(ColorSwitcher.instance.Dark);

        else ChangeColor(ColorSwitcher.instance.Dark);
    }
}