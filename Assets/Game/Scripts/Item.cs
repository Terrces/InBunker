using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private bool collectable;

    public void Collect()
    {
        if(!collectable) return;
    }
}
