using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform[] ballEmplacements;

    [SerializeField] TextMeshProUGUI pointsTxt;

    [SerializeField] AudioSource pointsAudioSource;

    [SerializeField] AudioClip victoryAudioClip;

    [SerializeField] GameObject restartButton;

    private int points;
    private int nbreBallsCatched = 0;

    private GameObject[] balls;


    private void Start()
    {
        balls = new GameObject[ballEmplacements.Length];

        InstantiateBalls();

        pointsTxt.text = points.ToString();
    }



    public void ballCatched()
    {
        nbreBallsCatched++;

        if (nbreBallsCatched >= ballEmplacements.Length)
        {            
            StartCoroutine(WaitBeforeRestartButton());            
        }
    }

   

    public void InstantiateBalls()
    {
        pointsTxt.text = points.ToString();

        for (int i = 0; i < ballEmplacements.Length; i++)
        {
            balls[i] = Instantiate(ballPrefab, ballEmplacements[i].position, Quaternion.identity);
        }
    }



    public void DestroyBalls()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            Destroy(balls[i]);
        }
    }

    private IEnumerator WaitBeforeRestartButton()
    {
        yield return new WaitForSeconds(1);        
        restartButton.SetActive(true);
        nbreBallsCatched = 0;
    }

    public void QuilleTombee()
    {
        points++;
        pointsAudioSource.Play();
        pointsTxt.text = points.ToString();
        if (points >= 10)
        {
            pointsTxt.text += ": Victory!";
            pointsAudioSource.clip = victoryAudioClip;
            pointsAudioSource.Play();
        }
    }

}
