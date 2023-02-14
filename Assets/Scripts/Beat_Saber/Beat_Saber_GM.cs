using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Beat_Saber_GM : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    private int score = 0;
    private int combo = 0;

    [SerializeField] CubeSpawner cubeSpawner;
    [SerializeField] AudioSource music;

    public void UpdateScore(int scoreDifference)
    {
        score += scoreDifference;
        scoreText.text = "Score: " + score;

        if (scoreDifference > 0)
        {
            combo++;
            if (combo >= 3) comboText.text = "x" + combo;
        }
        else
        {
            combo = 0;
            comboText.text = "";
        }
    }

    private void Update()
    {
        if (!music.isPlaying) Destroy(cubeSpawner);
    }
}
