using UnityEngine;

public class Healing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeHeal(10);
            Destroy(gameObject);
        }
    }
}