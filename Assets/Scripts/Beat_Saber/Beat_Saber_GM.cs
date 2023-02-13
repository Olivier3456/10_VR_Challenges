using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Beat_Saber_GM : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    public void UpdateScore(int scoreDifference)
    {
        score += scoreDifference;
        scoreText.text = "Score: " + score;
    }
}
