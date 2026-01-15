using UnityEngine;

public class Charger : Socket
{
    
    public void ChargeBattery(Collider collider)
    {
        GetSocket(collider);
    }

    void OnCollisionEnter(Collision collision) => ChargeBattery(collision.collider);
    void OnTriggerEnter(Collider other) => ChargeBattery(other);
}
