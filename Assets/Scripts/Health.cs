using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    public float health;
    [SerializeField] protected float maxHealth;
    public Slider healthSliderPrefab;
    protected Slider healthSlider;

    public virtual void TakeDamage(float amount)
    {
    }

    public virtual void Dead()
    {
    }

    protected void CreateHealthSlider()
    {
        if (healthSliderPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            healthSlider = Instantiate(healthSliderPrefab, canvas.transform);
            healthSlider.maxValue = 1;
            healthSlider.value = health / maxHealth;
            healthSlider.interactable = false;
        }
    }

    protected void UpdateHealthSliderPosition()
    {
        if (healthSlider != null)
        {
            Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            healthSlider.transform.position = screenPosition;
        }
    }

    protected void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }
    }
}
