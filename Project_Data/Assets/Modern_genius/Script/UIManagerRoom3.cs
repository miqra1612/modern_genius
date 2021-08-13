using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// This script is used to control user interface of the game suach as game panel, animation, user input and warning
/// </summary>

public class UIManagerRoom3 : MonoBehaviour
{
    public GameData gameData;

   

    [Header("This area is for all hint and puzzle panel")]
    public Animator clue1;
    public Animator clue2;
    public Animator clue3;
    

    [Header("This area is for all correct panel")]
    public Animator correct1;
    
    [Header("This area is for the puzzle input")]
    public InputField puzzleInput;
    public Text notificationDisplay;
   
    [Header("This part is for the puzzle answer, make sure you add the answer for your puzzle")]
    public string puzzleAnswer;
   
    [Header("Check this variable if this is the last puzzle room in the game")]
    public bool lastRoom = false;

    private bool clue1On = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find game data object in the begining of the scene
        gameData = GameObject.FindGameObjectWithTag("data").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is a fungtion to check puzzle 1 answer, if correct a new clue will be open and player can advance to the new map
    public void CheckingAnswer1(bool canAdvance)
    {
        if (puzzleInput.text.Equals( puzzleAnswer, StringComparison.OrdinalIgnoreCase))
        {
            AnswerCorrect(canAdvance);
            SwitchCorrect1(true);
            GameData.instance.room3PuzzleOpen[0] = true;
        }
        else
        {
            StartCoroutine(AnswerFalse());
        }
       
    }

    

    // This is a fungtion to activate a clue window 1
    public void SwitchClue1(bool isActive)
    {
        bool isComplete = GameData.instance.room3PuzzleOpen[0];

        if (isComplete)
        {
            SwitchCorrect1(true);
        }
        else
        {
            if (isActive)
            {
                clue1.Play("Window Enter");
                clue1On = true;
            }
            else
            {
                clue1.Play("Window Exit");
            }
        }
    }

    // This is a fungtion to activate a clue window 2
    public void SwitchClue2(bool isActive)
    {
        if (isActive)
        {
            clue2.Play("Window Enter");
        }
        else
        {
            clue2.Play("Window Exit");
        }
    }

    // This is a fungtion to activate a clue window 3
    public void SwitchClue3(bool isActive)
    {
        if (isActive)
        {
            clue3.Play("Window Enter");
        }
        else
        {
            clue3.Play("Window Exit");
        }
    }

   

    //This function is used to increase the game progression data and close all clue window the is open.
    //It is also serve to check if the last puzzle already solve or not, if yes then the game finish time will be calculated
    public void AnswerCorrect(bool continueMap)
    {
        if (continueMap && !lastRoom)
        {
            gameData.gamePhase++;
        }

        if (clue1On)
        {
            clue1On = false;
            clue1.Play("Window Exit");
        }
    }

    public void CalculateFinishTime()
    {
        if (lastRoom)
        {
            gameData.CalculateFinishTime();
        }
    }

    //This coroutine is used to add a message for the player when player put a wrong answer in puzzle 1 input
    IEnumerator AnswerFalse()
    {
        string prev = notificationDisplay.text;

        notificationDisplay.text = "Nothing happened";
        notificationDisplay.color = Color.red;

        yield return new WaitForSeconds(1);
        notificationDisplay.text = prev;
        notificationDisplay.color = Color.black;
    }

    // This function is used to activate a clue when puzzle 1 answer is correct
    public void SwitchCorrect1(bool isActive)
    {
        if (isActive)
        {
            correct1.Play("Window Enter");
        }
        else
        {
            correct1.Play("Window Exit");
        }
    }

    
}
