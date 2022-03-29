using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private Image heartImage;
    [SerializeField] private Health health;
    [SerializeField] private Gradient gradient;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("ColorsHeartChanged")]
    public Color defaultHeartColor;
    public Color changedHeartColor;

    [Header("Tweens")] 
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenScale;
    private TweenerCore<Color, Color, ColorOptions> _tweenColor;

    private void Start()
    {
        defaultHeartColor = heartImage.color;
    }

    private void Awake()
    {
        health.HealthChanged += OnHealthChanged;
    }
    private void OnDestroy()
    {
        health.HealthChanged -= OnHealthChanged;
        _tweenColor?.Kill();
        _tweenScale?.Kill();
    }
    
    private void OnHealthChanged(float valuePercantage)
    {
        if ( healthBarFilling.fillAmount > valuePercantage)
            HeartMinusAnimationChanged();
        healthBarFilling.fillAmount = valuePercantage;
        healthBarFilling.color = gradient.Evaluate(valuePercantage);
        healthText.text = health.currentHealth.ToString();
    }

    private void HeartMinusAnimationChanged()
    {
        _tweenColor?.Kill();
        _tweenScale?.Kill();
        _tweenScale =heartImage.transform.DOScale(1.5f, 0.2f).OnComplete(() => heartImage.transform.DOScale(1.7f, 0.2f));
        _tweenColor = heartImage.DOColor(changedHeartColor, 0.2f).OnComplete(() => heartImage.DOColor(defaultHeartColor, 0.2f));
    }
}
