using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI CheckScore;

    // Start is called before the first frame update
    void Start()
    {
        FinalScoreCheck();
        AppEvents.InvokeOnMouseCursorEnable(true);
    }

    void FinalScoreCheck()
    {

        CheckScore.text = "Enemies Remaining: " + ScoreHolder.ScoreTrack;

        if (ScoreHolder.ScoreTrack <= 0)
        {
            CheckScore.text += " \nYOU WIN";
        } else
        {
            CheckScore.text += " \nYOU LOSE, gotta get them all next time";

        }
    }

}
