using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColor : EditColor {
    private SpriteRenderer spriteRenderer;

    protected override void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.OnEnable();
    }

    public override void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    
}