using UnityEngine;

public class CoinScr : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerScore>().AddScore(1);
            Destroy(gameObject);
        }
    }
}
