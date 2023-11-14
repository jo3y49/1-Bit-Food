using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColor : EditColor {
    private Camera cam;

    protected override void OnEnable() {
        cam = GetComponent<Camera>();

        base.OnEnable();
    }

    public override void ChangeColor(Color color)
    {
        cam.backgroundColor = color;
    }
}