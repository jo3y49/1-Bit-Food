using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Flavor")]
public class Flavor : ScriptableObject{
    public Color bright = Color.white;
    public Color dark = Color.black;
    public int bonus = 0;
}