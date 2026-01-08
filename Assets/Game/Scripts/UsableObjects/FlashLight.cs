using UnityEngine;

public class FlashLight : MonoBehaviour, Iusable
{
    [SerializeField] private Light _light;
    
    public void Use(Transform point = null)
    {
        _light.gameObject.SetActive(!_light.gameObject.activeInHierarchy);
    }
}
