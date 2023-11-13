using System;
using UnityEngine;

public abstract class EditColor : MonoBehaviour {

    public ColorSwitcher.GameColor gameColor = ColorSwitcher.GameColor.Bright;

    protected virtual void Start() {
        if (gameColor == ColorSwitcher.GameColor.Bright) ChangeColor(ColorSwitcher.instance.Bright);

        else ChangeColor(ColorSwitcher.instance.Dark);
    }

    public abstract void ChangeColor(Color color);
}