using UnityEngine;

public class FlashLight : MonoBehaviour, Iusable
{
    [SerializeField] private Light _light;
    
    public void Use(Transform point = null, Properties properties = null)
    {
        _light.enabled = !_light.enabled;
    }
}
