using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] placesForSpawn;
    public GameObject enemyPref;

    public float delayForSpawn = 2;

    private void Start()
    {
        placesForSpawn = GameObject.FindGameObjectsWithTag("PlaceForSpawn");
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayForSpawn);

        while (true)
        {
            System.Random rand = new System.Random();
            var place = rand.Next(0, placesForSpawn.Length);
            Instantiate(enemyPref, placesForSpawn[place].transform.position, Quaternion.identity);

            yield return waitForSeconds;
        }
    }
}
