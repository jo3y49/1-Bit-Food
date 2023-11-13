using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorSwitcher : MonoBehaviour {
    public static ColorSwitcher instance;
    [SerializeField] private Tilemap floor, walls;
    public Color bright { get; private set; }
    public Color dark { get; private set; }

    private void Awake() {
        instance = this;

        bright = Color.white;
        dark = Color.black;
    }

    public void SetColors(Color bright, Color dark)
    {
        this.bright = bright;
        this.dark = dark;

        SwitchColor();
    }

    private void SwitchColor()
    {
        walls.color = bright;
        floor.color = dark;

        foreach (CharacterBattle character in FindObjectsOfType<CharacterBattle>())
        {
            character.gameObject.GetComponent<SpriteRenderer>().color = bright;
        }
    }

    public void FlipColors()
    {
        SetColors(dark, bright);
    }
}