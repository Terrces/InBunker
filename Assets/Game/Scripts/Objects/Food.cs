using UnityEngine;

public class Food : InteractiveObject, Iusable
{
    public int FoodValue = 1;
    public void Use(Transform transform, Properties properties)
    {
        bool available = properties.AddHealth(FoodValue);
        if(!available) return;
        properties.Interaction.DropObject(0);
        Destroy(gameObject);
    }
}
