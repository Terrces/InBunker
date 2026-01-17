using System.Collections.Generic;
using UnityEngine;

public class LightLogic : MonoBehaviour
{
    [SerializeField] List<Light> lightSources;
    public void SetActive(bool active = false)
    {
        foreach (Light source in lightSources)
        {
            source.enabled = active;
        }
    }
    public void ToggleActive()
    {
        foreach (Light source in lightSources)
        {
            source.enabled = !source.enabled;
        }
    }
}
