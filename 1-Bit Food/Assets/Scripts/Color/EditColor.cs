using System;
using UnityEngine;

public abstract class EditColor : MonoBehaviour {

    public ColorSwitcher.GameColor gameColor = ColorSwitcher.GameColor.Bright;

    protected virtual void OnEnable() {
        SetColors();
    }

    protected virtual void Start() {
        SetColors();
    }

    protected virtual void SetColors()
    {
        if (ColorSwitcher.instance == null) return;

        if (gameColor == ColorSwitcher.GameColor.Bright) ChangeColor(ColorSwitcher.instance.Bright);

        else ChangeColor(ColorSwitcher.instance.Dark);
    }

    public abstract void ChangeColor(Color color);
}