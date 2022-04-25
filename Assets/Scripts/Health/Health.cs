using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health stats")] 
    public int maxHealth = 1;
    public int currentHealth;
    
    private SpriteRenderer _characterSpriteRenderer;
    [Header("Change color")] 
    private TweenerCore<Color, Color, ColorOptions> _tweenColor;
    private Color _defaultColor;
    private Color _redColor;

    public event Action<float> HealthChanged;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ChangeHealth(0);
        _characterSpriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _characterSpriteRenderer.color;
        _redColor = Color.red;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //cheatcode
            ChangeHealth(-1);
    }

    private void OnDestroy()
    {
        _tweenColor?.Kill();
    }

    public void ChangeHealth(int value)
    {
        if (currentHealth + value > maxHealth) return;
        var prevHealth = currentHealth;
        currentHealth += value;

        if (currentHealth <= 0) //смерть
        {
            Death();
        }
        else
        {
            if (prevHealth>currentHealth) //при получении урона
                GetDamage();
            float currentHealthPercantage = (float) currentHealth / maxHealth;
            HealthChanged?.Invoke(currentHealthPercantage);
        }
    }

    private void Death()
    {
        HealthChanged?.Invoke(0);
        _tweenColor?.Kill();
        _tweenColor = _characterSpriteRenderer.DOColor(_redColor, 0.1f);
    }

    private void GetDamage()
    {
        //change color
        _tweenColor?.Kill();
        _tweenColor = _characterSpriteRenderer.DOColor(_redColor, 0.2f).
            OnComplete(() => _characterSpriteRenderer.DOColor(_defaultColor, 0.1f));
    }
}
