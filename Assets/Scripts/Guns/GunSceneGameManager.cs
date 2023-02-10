using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunSceneGameManager : MonoBehaviour
{
    public static GunSceneGameManager Instance;

    [SerializeField] TextMeshProUGUI _scoreTxt;
    private int score;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Une instance de GunSceneGameManager est déjà présente dans la scène !");
    }

    public void ShotFired()
    {
        score --;
        _scoreTxt.text = "Score: " + score;
    }

    public void OnePointMore()
    {        
        score+= 2;
        _scoreTxt.text = "Score: " + score;
    }
}
