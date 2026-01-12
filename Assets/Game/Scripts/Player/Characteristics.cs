using System.Collections;
using TMPro;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    enum TypesTemperatureLogic {substract, adding}
    private Properties propeties => GetComponent<Properties>();
    [SerializeField] private int substractTemperature = 1;
    [SerializeField] private float updateTemperatureSeconds = 0.1f;
    [SerializeField] private float takingDamageSeconds = 0.6f;
    [SerializeField] private bool IsWarming = false;
    [SerializeField] private bool AllGood = true;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text temperatureText;

    private bool isTakingDamage = false;

    private void temperature(TypesTemperatureLogic _type)
    {
        switch (_type)
        {
            case TypesTemperatureLogic.adding:
            propeties.Temperature += substractTemperature;
            break;
            case TypesTemperatureLogic.substract:
            propeties.Temperature -= substractTemperature;
            break;
        }
        temperatureText.text = $"Temperature: {propeties.Temperature}";
    }

    IEnumerator feezing()
    {
        while (propeties.Temperature > 0 && !IsWarming)
        {
            temperature(TypesTemperatureLogic.substract);
            yield return new WaitForSeconds(updateTemperatureSeconds);
        }
    }
    IEnumerator warming(int _temperature)
    {
        while (propeties.Temperature != 100 && _temperature != 0 && IsWarming)
        {
            temperature(TypesTemperatureLogic.adding);
            yield return new WaitForSeconds(updateTemperatureSeconds);
        }
    }

    IEnumerator takingDamage()
    {
        while (propeties.health > 0 && !AllGood)
        {
            propeties.health -= 1;
            healthText.text = $"Health: {propeties.health}";
            healthText.text = $"YOU DIED!!!";
            isTakingDamage = true;
            yield return new WaitForSeconds(takingDamageSeconds);
        }
    }

    IEnumerator CheckingState()
    {
        while (propeties.health > 0)
        {
            
            if(propeties.Temperature <= 0 && !isTakingDamage)
            {
                StartCoroutine(takingDamage());
                
                AllGood = false;
            }
            if (propeties.Temperature > 0)
            {
                AllGood = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        cold();
        StartCoroutine(CheckingState());
    }



    public void cold()
    {
        IsWarming = false;
        StartCoroutine(feezing());
    }
    public void warm(int temperature)
    {
        IsWarming = true;
    } 
}
