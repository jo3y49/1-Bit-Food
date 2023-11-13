using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColor : EditColor {
    private Camera cam;

    protected override void Start() {
        cam = GetComponent<Camera>();

        base.Start();
    }

    public override void ChangeColor(Color color)
    {
        cam.backgroundColor = color;
    }

    
}