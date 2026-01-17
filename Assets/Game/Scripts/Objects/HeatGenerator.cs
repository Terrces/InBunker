using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatGenerator : Socket
{
    [SerializeField] List<LightLogic> LightSource;
    [SerializeField] bool GeneratorEnabled = true;
    [SerializeField] HeatSource heat;
    [SerializeField] int FuelLoss = 5;
    public int Fuel;
    void Start()
    {
        StartCoroutine(CheckFuel());
    }

    private void toggleLightSource(bool _active = false)
    {
        if(LightSource.Count == 0) return;
        foreach (LightLogic light in LightSource)
        {
            light.SetActive(_active);
        }
    }

    IEnumerator CheckFuel()
    {
        while (GeneratorEnabled)
        {
            if(Fuel > 0)
            {
                Fuel -= FuelLoss;
                Fuel = Mathf.Clamp(Fuel,0,100);
            }
            else
            {
                GeneratorEnabled = false;
                heat.HeatEnabled = false;
                toggleLightSource(false);
            }

            yield return new WaitForSeconds(1);
        }
    }

    void generatorLogic(Collider collider)
    {
        if(!collider.TryGetComponent(out Ifuel component) || !heat) return;
        if(component.Fuel <= 0) return;
        if(Fuel + component.Fuel < 100) Fuel += component.Fuel;
        GetSocket(collider);

        Debug.Log(component.Fuel);
        
        component.Fuel -= 100 - Fuel;

        Fuel = Mathf.Clamp(Fuel,0,100);

        if(Fuel > 0)
        {
            GeneratorEnabled = true;
            heat.HeatEnabled = true;
            toggleLightSource(true);
        }

        StopCoroutine(CheckFuel());
        StartCoroutine(CheckFuel());

        if(component.Destroyable) Destroy(collider.gameObject);
    }

    void OnTriggerEnter(Collider other) => generatorLogic(other);
    void OnCollisionEnter(Collision collision) => generatorLogic(collision.collider);
}
