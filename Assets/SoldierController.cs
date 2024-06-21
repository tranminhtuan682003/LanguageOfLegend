using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Animator animator;
    private bool isAttacking;
    private Transform attackTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home"))
        {
            Debug.Log("Soldier entered Home trigger.");
            gameObject.SetActive(false);
        }
    }
}
