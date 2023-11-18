using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColor : EditColor {
    [SerializeField] private Tilemap walls;


    public override void ChangeColor(Color color)
    {
        walls.color = ColorSwitcher.instance.Bright;
    }
    
}