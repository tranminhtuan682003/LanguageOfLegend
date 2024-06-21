using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : Health
{
    [SerializeField] private GameInput gameInput;
    public Animator animator;
    public NavMeshAgent agent;
    private Coroutine attackCoroutine;
    public GameObject healthPlus;
    private bool activateMovement;
    private Transform target;
    public float damage;

    public AnimationClip attackClip;
    private bool canAttack = true;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        maxHealth = 200f;
        health = maxHealth;
        damage = 30f;
        CreateHealthSlider();

        gameInput.action.Player.Move.started += Move_started;
        gameInput.action.Player.Move.canceled += Move_canceled;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        activateMovement = true;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        activateMovement = false;
    }

    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Move();
        SetAnimation();
        UpdateHealthSliderPosition();
    }

    private void Move()
    {
        if (activateMovement)
        {
            Vector2 inputVector = gameInput.GetMoveInputVector();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
            if (moveDirection != Vector3.zero)
            {
                agent.SetDestination(moveDirection + transform.position);
            }
        }
    }

    private void SetAnimation()
    {
        animator.SetBool("isRun", agent.velocity.sqrMagnitude > 0);

        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            agent.isStopped = true;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }
    }

    public void ButtonAttack()
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        PerformAttack();
        yield return new WaitForSeconds(attackClip.length);
        canAttack = true;
    }

    private void PerformAttack()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= 3f)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }
            }
            else
            {
                RemoveTarget();
            }
        }
    }

    private void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void RemoveTarget()
    {
        target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home"))
        {
            health += 5f;
            UpdateHealthSlider();
        }
        else if (other.gameObject.CompareTag("HomeEnemy"))
        {
            health -= 50f;
            UpdateHealthSlider();
        }
        else if (other.gameObject.CompareTag("HealthPlus"))
        {
            health += 40f;
            healthPlus.SetActive(false);
            UpdateHealthSlider();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            RotateTowards(other.transform);
            SetTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            RemoveTarget();
        }
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;
        healthSlider.value = health / maxHealth;
        if (health <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        health = 0;
        healthSlider.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
