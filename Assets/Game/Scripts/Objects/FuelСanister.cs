using UnityEngine;

public class FuelСanister : InteractiveObject, Ifuel
{
    [Header("Fuel")]
    [Space (1)]
    [SerializeField] private bool destroyable = true;
    [SerializeField] private int fuel = 5;
    public bool Destroyable {get => destroyable; set => destroyable = value;}
    public int Fuel {get => fuel; set => fuel = value;}
}
