using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColor : EditColor {
    [SerializeField] private Tilemap floor, walls;


    public override void ChangeColor(Color color)
    {
        floor.color = ColorSwitcher.instance.Dark;
        walls.color = ColorSwitcher.instance.Bright;
    }
    
}