using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    enum returnType {oneCount,moreCount}
    public List<StorageInspector> objects;
    [SerializeField] bool dropItems = true;

    private float getMaxRange()
    {
        float maxRange = 0;
        
        foreach (StorageInspector storage in objects) maxRange += storage.getChance();
        return maxRange;
    }

    private GameObject returnObject(float value, returnType type)
    {
        float currentValue = 0f;

        foreach(StorageInspector storage in objects)
        {
            if(type == returnType.oneCount && value <= storage.getChance()) return storage.getObject();
            else if(type == returnType.moreCount && (value <= (currentValue += storage.getChance()))) return storage.getObject();
        }

        return null;
    }

    public GameObject GetItem()
    {
        if (!dropItems) return null;

        if (objects.Count > 1)
        {
            return returnObject(Random.Range(0, getMaxRange()), returnType.moreCount);
        }
        else if (objects.Count == 1)
        {
            return returnObject(Random.Range(0,100),returnType.oneCount);
        }

        return null;
    }
}
