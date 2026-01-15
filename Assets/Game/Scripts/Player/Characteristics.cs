using System.Collections;
using TMPro;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    private Properties properties => GetComponent<Properties>();

    [Header("Temperature")]
    [SerializeField] private float temperature = 100f;   // 0–100
    [SerializeField] private float coldPerSecond = 2f;    // холод среды
    [SerializeField] private float minTemp = 0f;
    [SerializeField] private float maxTemp = 100f;

    [Header("Damage")]
    [SerializeField] private float damageInterval = 0.6f;
    [SerializeField] private int damagePerTick = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text temperatureText;

    private bool isTakingDamage;

    void Start()
    {
        StartCoroutine(DamageFromCold());
        UpdateUI();
    }

    void Update()
    {
        float heat = CalculateHeat();

        temperature += (heat - coldPerSecond) * Time.deltaTime;
        temperature = Mathf.Clamp(temperature, minTemp, maxTemp);

        properties.Temperature = Mathf.RoundToInt(temperature);
        UpdateUI();
    }

    float CalculateHeat()
    {
        HeatSource[] sources = FindObjectsOfType<HeatSource>();
        float total = 0f;

        foreach (HeatSource s in sources)
        {
            float dist = Vector3.Distance(transform.position, s.transform.position);
            if (dist > s.radius) continue;
            if(!s.HeatEnabled) continue;

            float factor = 1f - dist / s.radius;
            total += s.heatPower * factor;
        }

        return total;
    }

    IEnumerator DamageFromCold()
    {
        while (properties.health > 0)
        {
            if (temperature <= 0f)
            {
                isTakingDamage = true;

                properties.health -= damagePerTick;
                UpdateUI();
            }
            else if (temperature > 0f)
            {
                isTakingDamage = false;
            }

            yield return new WaitForSeconds(damageInterval);
        }

        healthText.text = "YOU DIED!";
    }

    void UpdateUI()
    {
        if (temperatureText)
            temperatureText.text = $"Temperature: {Mathf.RoundToInt(temperature)}";

        if (healthText)
            healthText.text = $"Health: {properties.health}";
    }
}
