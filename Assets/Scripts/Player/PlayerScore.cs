using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI score_text;
    public float score;

    public void AddScore(float amount)
    {
        score += amount;
        score_text.text = ":" + score.ToString();
    }
}