using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthPlayer : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider healthSliderPrefab;
    public Slider healthSlider;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        healthSlider.value -= amount/maxHealth;
        if(health <= 0)
        {
            Dead();
        }
    }
    public virtual void Dead() { }
    protected void CreateHealthSlider()
    {
        if (healthSliderPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                healthSlider = Instantiate(healthSliderPrefab, canvas.transform);
                healthSlider.maxValue = maxHealth;
                healthSlider.value = health;
            }
        }
    }
}
