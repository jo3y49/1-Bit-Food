using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonColor : UIColor, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI tm;
    private Button button;
    // private Image backdrop;
    private TextColor tmScript;
    public bool flipColor, fliptext, hoverChange = true;
    private bool flipped = false;
    private bool selected = false;

    protected override void OnEnable()
    {
        tm = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

        // backdrop = GetComponentInChildren<Image>();

        if (tm != null) tmScript = tm.GetComponent<TextColor>();

        base.OnEnable();
    }
    
    private void OnDisable() {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 255);

        // if (!flipColor) return;

        // selected = false;

        // SetColors();
        // ResetTextColor();
    }

    public override void ChangeColor(Color color)
    {
        // if (selected) return;

        base.ChangeColor(color);

        ColorSwitcher.GameColor tempColor = gameColor;

        // if (flipped)
        // {
        //     if (gameColor == ColorSwitcher.GameColor.Bright)
        //     {
        //         tempColor = ColorSwitcher.GameColor.Dark;
        //     }
        //     else 
        //     {
        //         tempColor = ColorSwitcher.GameColor.Bright;
        //     }
        // }

        // if (backdrop != null)
        // {
        //     if (tempColor == ColorSwitcher.GameColor.Bright)
        //     {
        //         backdrop.color = ColorSwitcher.instance.Dark;
        //     }
        //     else 
        //     {
        //         backdrop.color = ColorSwitcher.instance.Bright;
        //     }
        // }
            

        if (tm != null && tmScript == null)
        {
            tm.color = color;

            // if (tempColor == ColorSwitcher.GameColor.Bright)
            // {
            //     tm.color = ColorSwitcher.instance.Dark;
            // }
            // else 
            // {
            //     tm.color = ColorSwitcher.instance.Bright;
            // }
        }

        // flipped = false;
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
        if (hoverChange && button!= null && button.transition != Selectable.Transition.SpriteSwap) image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
        // if (!flipColor) return;

        // FlipColor();
        // FlipTextColor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (hoverChange && button!= null && button.transition != Selectable.Transition.SpriteSwap) image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
        // if (!flipColor) return;

        // SetColors();
        // ResetTextColor();
    }

    private void FlipColor()
    {
        flipped = true;

        if (gameColor == ColorSwitcher.GameColor.Bright)
        {
            ChangeColor(ColorSwitcher.instance.Dark);
            // if (backdrop != null) backdrop.color = ColorSwitcher.instance.Bright;
        }

        else 
        {
            ChangeColor(ColorSwitcher.instance.Dark);
            // if (backdrop != null) backdrop.color = ColorSwitcher.instance.Dark;
        }
    }
}