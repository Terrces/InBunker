using System.Collections.Generic;
using UnityEngine;

public class ItemsList:MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();

    public Item GetItemByIndex(int index)
    {
        if (index > items.Count) {Debug.Log("Out of bounds"); return null;}
        return items[index];
    }
    public Item GetItemByName(string name)
    {
        name = name.ToLower();
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].GetName().ToLower() == name)
            {
                return items[i];
            }
        }

        Debug.Log("Item By Name not found");

        return null;
    }
}
