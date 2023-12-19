using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDisplay : MonoBehaviour {
    [SerializeField] private GameObject itemDisplayPrefab;
    private List<GameObject> itemDisplays = new();

    public void SetItem(InventoryItem item) {
        GameObject itemDisplay = Instantiate(itemDisplayPrefab, transform);
        SpriteRenderer spriteRenderer = itemDisplay.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
        // make the gameobject size equal to the sprite size divided by the parent's scale
        itemDisplay.transform.localScale = Vector2.one * 1.5f / transform.parent.localScale.x;
        itemDisplays.Add(itemDisplay);

        SortItems();
    }

    public void RemoveItem(InventoryItem item) {
        foreach (GameObject itemDisplay in itemDisplays) {
            if (itemDisplay.GetComponent<SpriteRenderer>().sprite == item.sprite) {
                Destroy(itemDisplay);
                itemDisplays.Remove(itemDisplay);
                SortItems();
                break;
            }
        }
    }

    private void SortItems()
    {
        float itemGap = 2f;
        float totalItemOffset = (itemDisplays.Count - 1) * (itemGap / 2f);

        for (int i = 0; i < itemDisplays.Count; i++)
        {
            itemDisplays[i].transform.position = new Vector3(transform.position.x + (i * itemGap) - totalItemOffset, transform.position.y, transform.position.z);
        }
    }

    public void ClearItems() {
        foreach (GameObject itemDisplay in itemDisplays) {
            Destroy(itemDisplay);
        }
        itemDisplays.Clear();
    }
    
}