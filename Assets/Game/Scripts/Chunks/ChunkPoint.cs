using System.Collections.Generic;
using UnityEngine;

public class ChunkPoint : MonoBehaviour
{
    public enum chunkPointsReturnType { oneCount, moreCount }

    public Transform Point => transform;
    public List<ChunkSubObjectInspector> gameObjects = new List<ChunkSubObjectInspector>();

    void Start()
    {
        ChunkSubObjectInspector data =
            ReturnObjectData(Random.Range(0f, GetMaxRange()), chunkPointsReturnType.moreCount);

        if (data == null || data._gameObject == null)
            return;

        GameObject instance = Instantiate(
            data._gameObject,
            Point.position,
            Quaternion.identity,
            transform
        );

        ApplyManipulations(instance, data);
    }

    public float GetMaxRange()
    {
        float maxRange = 0f;
        foreach (var obj in gameObjects)
            maxRange += obj.chance;
        return maxRange;
    }

    private void ApplyManipulations(GameObject instance, ChunkSubObjectInspector data)
    {
        if (!data.DefaultScale) instance.transform.localScale = Vector3.one * data.Scale;
        if (data.RandomScale) instance.transform.localScale = Vector3.one * Random.Range(data.MinScale,data.Scale);
        if (instance.TryGetComponent(out Rigidbody rigidbody))
        {
            if (!data.DefaultMass) rigidbody.mass = data.Mass;
            if (data.RandomMass) rigidbody.mass = Random.Range(data.MinMass,data.Mass);
        }

        Vector3 rotation = data.Rotation;

        if (data.RandomRotation)
        {
            switch (data.lockObjectRotation)
            {
                case ChunkSubObjectInspector.axisObjectRandomRotation.Both:
                    rotation = new Vector3(
                        Random.Range(0, 360),
                        Random.Range(0, 360),
                        Random.Range(0, 360));
                    break;

                case ChunkSubObjectInspector.axisObjectRandomRotation.X:
                    rotation.x = Random.Range(0, 360);
                    break;

                case ChunkSubObjectInspector.axisObjectRandomRotation.Y:
                    rotation.y = Random.Range(0, 360);
                    break;

                case ChunkSubObjectInspector.axisObjectRandomRotation.Z:
                    rotation.z = Random.Range(0, 360);
                    break;
            }
        }

        instance.transform.rotation = Quaternion.Euler(rotation);
    }

    public ChunkSubObjectInspector ReturnObjectData(float value, chunkPointsReturnType type)
    {
        float currentValue = 0f;

        foreach (var obj in gameObjects)
        {
            if (type == chunkPointsReturnType.oneCount)
            {
                if (value <= 100f)
                    return obj;
            }
            else
            {
                currentValue += obj.chance;
                if (value <= currentValue)
                    return obj;
            }
        }

        return null;
    }
}
