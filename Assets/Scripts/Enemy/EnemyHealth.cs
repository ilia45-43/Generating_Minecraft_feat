using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
    public float timeToAttack = 2;

    public GameObject HealthPref;
    public GameObject CoinPref;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private WaitForSeconds waitForSeconds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(AttackPlayer(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator AttackPlayer(Collider other)
    {
        waitForSeconds = new WaitForSeconds(timeToAttack);

        while (true)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(10);
            yield return waitForSeconds;
        }
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Kill();
        }
    }
    public void Kill()
    {
        animator.SetTrigger("died");
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(navMeshAgent, 1);
        Destroy(gameObject, 1);
        SpawnAfterDead();
    }

    private void SpawnAfterDead()
    {
        System.Random rand = new System.Random();
        var num = rand.Next(0, 3);

        if (num == 2)
            Instantiate(HealthPref, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        else
            Instantiate(CoinPref, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
    }
}