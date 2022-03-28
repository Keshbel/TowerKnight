using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private Health health;
    [SerializeField] private Gradient gradient;
    [SerializeField] private TextMeshProUGUI healthText;
    

    private void Awake()
    {
        health.HealthChanged += OnHealthChanged;
    }
    private void OnDestroy()
    {
        health.HealthChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged(float valuePercantage)
    {
        Debug.Log(valuePercantage);
        healthBarFilling.fillAmount = valuePercantage;
        healthBarFilling.color = gradient.Evaluate(valuePercantage);
        healthText.text = health.currentHealth.ToString();
    }
}
