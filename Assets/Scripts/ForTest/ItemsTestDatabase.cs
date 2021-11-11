using System.Collections.Generic;
using UnityEngine;

public class ItemsTestDatabase : MonoBehaviour
{
    public List<Item> items;
    
    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }
}
