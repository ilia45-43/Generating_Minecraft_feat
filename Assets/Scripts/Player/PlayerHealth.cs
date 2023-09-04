using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public Slider slider;
    public GameObject deathPanel;

    public void TakeDamage(float amount)
    {
        health -= amount;
        slider.value = health;

        if (health <= 0)
        {
            DeadPlayer();
        }
    }

    private void DeadPlayer()
    {
        Time.timeScale = 0;
        deathPanel.SetActive(true);
    }

    public void TakeHeal(float amount)
    {
        health += amount;
        if (health >= 100)
        {
            health = 100;
        }
        slider.value = health;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
