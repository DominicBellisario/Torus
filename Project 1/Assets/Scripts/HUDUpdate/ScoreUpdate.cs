using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    //the text box
    [SerializeField]
    Text text;

    //the score
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        //set the text
        text.text = "Score: " + score;
    }

    /// <summary>
    /// updates score by the determined amount
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        this.score += score;
        //updates text
        text.text = "Score: " + this.score;
    }
}
