using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Health
{
    private Animator animator;
    public GameObject player;
    private bool isPlayerInRange = false;
    private bool isDead = false;
    private Coroutine damageCoroutine;
    private Coroutine recuperateCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;

        CreateHealthSlider();
    }

    void Update()
    {
        UpdateHealthSliderPosition();

        if (isPlayerInRange && !isDead)
        {
            RotateTowards(player.transform);
        }
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;
        healthSlider.value = health / maxHealth;

        if (health <= 0 && !isDead)
        {
            Dead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (recuperateCoroutine != null)
            {
                StopCoroutine(recuperateCoroutine);
                recuperateCoroutine = null;
            }
            damageCoroutine = StartCoroutine(AttackPlayer(other));
        }
        else if (other.gameObject.CompareTag("Kamehameha"))
        {
            TakeDamage(100);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
            animator.SetFloat("Blend", 0f);
            recuperateCoroutine = StartCoroutine(Recuperate());
        }
    }

    private IEnumerator AttackPlayer(Collider playerCollider)
    {
        while (isPlayerInRange && !isDead)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= 3)
            {
                animator.SetFloat("Blend", 0.5f);
                player.GetComponent<Health>().TakeDamage(10f);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator Recuperate()
    {
        while (!isPlayerInRange && health < maxHealth && !isDead)
        {
            health += 15f;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            healthSlider.value = health / maxHealth;
            yield return new WaitForSeconds(2f);
        }
    }

    public override void Dead()
    {
        isDead = true;
        healthSlider.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().health += 20f;
        StartCoroutine(WaitForDie());
    }

    private IEnumerator WaitForDie()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3f);
        DeactivateGameObject();
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
