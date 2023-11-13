using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColor : EditColor {
    private SpriteRenderer spriteRenderer;

    protected override void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Start();
    }

    public override void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    
}