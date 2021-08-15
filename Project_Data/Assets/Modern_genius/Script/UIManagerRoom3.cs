using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

/// <summary>
/// This script is used to control user interface of the game suach as game panel, animation, user input and warning
/// </summary>

public class UIManagerRoom3 : MonoBehaviour
{
    public GameData gameData;

    [Header("This area is for button clue")]
    public Button cluePuzzleBtn1;
    public Button cluePuzzleBtn2;
    public Button cluePuzzleBtn3;
    public Button clueBtn1;
    public Button clueBtn2;

    [Header("This area is for puzzle clue panel")]
    public RectTransform puzzleClue1;
    public RectTransform puzzleClue2;
    public RectTransform puzzleClue3;

    [Header("This area is for non puzzle clue panel")]
    public RectTransform clue1;
    public RectTransform clue2;

    [Header("This area is for all correct panel")]
    public RectTransform answerPanel1;
    public RectTransform answerPanel2;
    public RectTransform answerPanel3;

    [Header("This area is for the puzzle input")]
    public InputField puzzleInput1;
    public Text notificationDisplay1;
    public InputField puzzleInput2;
    public Text notificationDisplay2;
    public InputField puzzleInput3;
    public Text notificationDisplay3;

    [Header("This part is for the puzzle answer, make sure you add the answer for your puzzle")]
    public string[] puzzleAnswer1;
    public string[] puzzleAnswer2;
    public string[] puzzleAnswer3;

    [Header("Check this variable if this is the last puzzle room in the game")]
    public bool lastRoom = false;

    private bool clue1On = false;
    private bool clue2On = false;
    private bool clue3On = false;
    private bool clue4On = false;
    private bool clue5On = false;

    //new system here
    public float tweenDelay = 1f;
    public List<RectTransform> openWindow = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        // Find game data object in the begining of the scene
        gameData = GameObject.FindGameObjectWithTag("data").GetComponent<GameData>();

        cluePuzzleBtn1.onClick.AddListener(() => ClueWithPuzzle1(puzzleClue1));
        //puzzle button 2 only entered to variable for setting consistency;
        cluePuzzleBtn3.onClick.AddListener(() => ClueWithPuzzle3(puzzleClue3));
        clueBtn1.onClick.AddListener(() => NonPuzzleClue(clue1));
        clueBtn2.onClick.AddListener(() => NonPuzzleClue(clue2));

        CheckButton();
    }

    void CheckButton()
    {
        bool isComplete = GameData.instance.room3PuzzleOpen[1];

        if (isComplete)
        {
            clueBtn2.GetComponent<Image>().color = Color.white;
            clueBtn2.interactable = true;
        }
        
    }
    
    // This is a fungtion to check puzzle 1 answer, if correct a new clue will be open and player can advance to the new map
    public void CheckingAnswer1(bool canAdvance)
    {
        var a = 0;

        for (int i = 0; i < puzzleAnswer1.Length; i++)
        {
            if (puzzleInput1.text.Equals(puzzleAnswer1[i], StringComparison.OrdinalIgnoreCase))
            {
                a++;
                break;
            }
        }

        if (a > 0)
        {
            GameData.instance.room3PuzzleOpen[0] = true;
            SwitchCorrect1(true);
            AnswerCorrect(canAdvance);
        }
        else
        {
            StartCoroutine(AnswerFalse1());
        }
    }

    public void CheckingAnswer2(bool canAdvance)
    {
        var a = 0;

        for (int i = 0; i < puzzleAnswer2.Length; i++)
        {
            if (puzzleInput2.text.Equals(puzzleAnswer2[i], StringComparison.OrdinalIgnoreCase))
            {
                a++;
                break;
            }
        }

        if (a > 0)
        {
            GameData.instance.room3PuzzleOpen[1] = true;
            SwitchCorrect2(true);
            AnswerCorrect(canAdvance);

            clueBtn2.GetComponent<Image>().color = Color.white;
            clueBtn2.interactable = true;
            
        }
        else
        {
            StartCoroutine(AnswerFalse2());
        }
    }

    public void CheckingAnswer3(bool canAdvance)
    {
        var a = 0;

        Debug.Log(puzzleInput3.text);
        for (int i = 0; i < puzzleAnswer3.Length; i++)
        {
            if (puzzleInput3.text.Equals(puzzleAnswer3[i], StringComparison.OrdinalIgnoreCase))
            {
                a++;
                break;
            }
        }

        if (a > 0)
        {
            GameData.instance.room3PuzzleOpen[2] = true;
            SwitchCorrect3(true);
            AnswerCorrect(canAdvance);
        }
        else
        {
            StartCoroutine(AnswerFalse3());
        }
    }


    // This is a fungtion to activate a clue window 1
    public void ClueWithPuzzle1(RectTransform panel)
    {
        bool isComplete = GameData.instance.room3PuzzleOpen[0];

        if (isComplete)
        {
            SwitchCorrect1(true);
        }
        else
        {
            ClosePanel();
            OpenPanel(panel);
        }
    }

    // This is a function to activate a clue window 2
    public void ClueWithPuzzle2(RectTransform panel)
    {
        bool isComplete = GameData.instance.room3PuzzleOpen[1];

        if (isComplete)
        {
            SwitchCorrect2(true);
        }
        else
        {
            ClosePanel();
            OpenPanel(panel);
        }
    }

    public void ClueWithPuzzle3(RectTransform panel)
    {
        bool isComplete = GameData.instance.room3PuzzleOpen[2];

        if (isComplete)
        {
            SwitchCorrect3(true);
        }
        else
        {
            ClosePanel();
            OpenPanel(panel);
        }
    }

    // This is a fungtion to activate a clue window 3
    public void NonPuzzleClue(RectTransform panel)
    {
        ClosePanel();
        OpenPanel(panel);
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
        }
        else if (clue2On)
        {
            clue2On = false;
        }
        else if (clue3On)
        {
            clue3On = false;
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
    IEnumerator AnswerFalse1()
    {
        string prev = notificationDisplay1.text;

        notificationDisplay1.text = "Nothing happened";
        notificationDisplay1.color = Color.red;

        yield return new WaitForSeconds(1);
        notificationDisplay1.text = prev;
        notificationDisplay1.color = Color.black;
    }

    IEnumerator AnswerFalse2()
    {
        string prev = notificationDisplay2.text;

        notificationDisplay2.text = "Nothing happened";
        notificationDisplay2.color = Color.red;

        yield return new WaitForSeconds(1);
        notificationDisplay2.text = prev;
        notificationDisplay2.color = Color.black;
    }

    IEnumerator AnswerFalse3()
    {
        string prev = notificationDisplay3.text;

        notificationDisplay3.text = "Nothing happened";
        notificationDisplay3.color = Color.red;

        yield return new WaitForSeconds(1);
        notificationDisplay3.text = prev;
        notificationDisplay3.color = Color.black;
    }

    // This function is used to activate a clue when puzzle 1 answer is correct
    public void SwitchCorrect1(bool isActive)
    {
        if (isActive)
        {
            bool isComplete1 = GameData.instance.room3PuzzleOpen[0];
            bool isComplete2 = GameData.instance.room3PuzzleOpen[1];

            ClosePanel();
            Debug.Log("if 1: " + isComplete1);

            if (isComplete1 && !isComplete2)
            {
                
                OpenPanel(puzzleClue2);
            }
            else if (isComplete1 && isComplete2)
            {
                Debug.Log("if 2: " + isComplete2);
                SwitchCorrect2(isActive);
            }
            
            clue1On = true;
        }
        else
        {
            ClosePanel();
        }
    }

    public void SwitchCorrect2(bool isActive)
    {
        if (isActive)
        {
            ClosePanel();
            OpenPanel(answerPanel2);
            clue2On = true;
        }
        else
        {
            ClosePanel();
        }
    }

    public void SwitchCorrect3(bool isActive)
    {
        if (isActive)
        {
            ClosePanel();
            OpenPanel(answerPanel3);
            clue3On = true;
        }
        else
        {
            ClosePanel();
        }
    }

    void OpenPanel(RectTransform rt)
    {
        rt.DOAnchorPos(Vector2.zero, tweenDelay);
        openWindow.Add(rt);
    }

    public void ClosePanel()
    {
        for (int i = 0; i < openWindow.Count; i++)
        {
            openWindow[i].DOAnchorPos(new Vector2(0, 2000), tweenDelay);
        }

        openWindow.Clear();
    }

}
