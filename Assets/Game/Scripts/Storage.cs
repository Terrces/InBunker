using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public List<StorageInspector> objects = new List<StorageInspector>();
    [SerializeField] bool dropItems = true;

    private float getMaxRange()
    {
        float maxRange = 0;
        
        foreach (StorageInspector storage in objects) maxRange += storage.getChance();
        return maxRange;
    }

    public GameObject GetItem()
    {
        if (!dropItems) return null;

        float randomValue;     
        
        if (objects.Count > 1)
        {
            float currentValue = 0f;
            
            randomValue = Random.Range(0, getMaxRange());
            foreach(StorageInspector storage in objects)
            {
                if(randomValue <= (currentValue += storage.getChance()) ) return storage.getObject();
            }
        }
        else if (objects.Count == 1)
        {
            randomValue = Random.Range(0,100);
            if (randomValue <= objects[0].getChance()) return objects[0].getObject();
            return null;
        }
        else return null;

        return null;
    }
}
