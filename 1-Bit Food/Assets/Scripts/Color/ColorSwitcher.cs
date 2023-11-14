using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorSwitcher : MonoBehaviour {
    public static ColorSwitcher instance;
    [SerializeField] private Tilemap floor, walls;
    public Color Bright { get; private set; }
    public Color Dark { get; private set; }

    private Color prevBright, prevDark;

    public enum GameColor{
        Bright,
        Dark,
    }

    private void Awake() {
        instance = this;

        Bright = prevBright = Color.white;
        Dark = prevDark = Color.black;
    }

    public void SetColors(Color bright, Color dark)
    {
        if (bright == null || dark == null) 
        {
            ResetFlavor();
            return;
        }

        Bright = bright;
        Dark = dark;

        SwitchColor();
    }

    public void SetFlavor(Flavor flavor)
    {
        prevBright = Bright;
        prevDark = Dark;

        SetColors(flavor.bright, flavor.dark);
    }

    public void ResetFlavor()
    {
        SetColors(prevBright, prevDark);
    }

    private void SwitchColor()
    {
        if (walls != null && floor != null)
        {
            walls.color = Bright;
            floor.color = Dark;
        }

        foreach (EditColor c in FindObjectsOfType<EditColor>())
        {
            if (c.gameColor == GameColor.Bright)
                c.ChangeColor(Bright);
            else 
                c.ChangeColor(Dark);
        }
    }

    public void FlipColors()
    {
        SetColors(Dark, Bright);
    }
}