using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    enum returnType {oneCount,moreCount}
    public List<StorageInspector> objects = new List<StorageInspector>();
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
            if(type == returnType.oneCount && value <= 100) return storage.getObject();
            else if(type == returnType.moreCount && (value <= (currentValue += storage.getChance()))) return storage.getObject();
            else return null;
        }

        return null;
    }

    public GameObject GetItem()
    {
        if (!dropItems) return null;

        if (objects.Count > 1) returnObject(Random.Range(0, getMaxRange()), returnType.moreCount);
        else if (objects.Count == 1) returnObject(Random.Range(0,100),returnType.oneCount);

        return null;
    }
}
