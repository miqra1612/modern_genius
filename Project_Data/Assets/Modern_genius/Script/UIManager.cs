using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
/// <summary>
/// This script is used to control user interface of the game suach as game panel, animation, user input and warning
/// </summary>

public class UIManager : MonoBehaviour
{
    public GameData gameData;

   
    [Header("This area is for all hint and puzzle panel")]
    public Animator clue1;
    public Animator clue2;
    public Animator clue3;
    public Animator clue4;

   
    [Header("This area is for all correct panel")]
    public Animator correct1;
    public Animator correct2;
   

    [Header("This area is for the puzzle input")]
    public InputField puzzleInput;
    public Text notificationDisplay;
    public InputField puzzleInput2;
    public Text notificationDisplay2;

    [Header("This part is for the puzzle answer, make sure you add the answer for your puzzle")]
   
    public string[] puzzle1Answers;
    public string[] puzzle2Answers;

    [Header("Check this variable if this is the last puzzle room in the game")]
    public bool lastRoom = false;

    private bool clue1On = false;
    private bool clue2On = false;
    private bool clue3On = false;
    private bool clue4On = false;
    private bool clue5On = false;

    //new system here

    public List<RectTransform> openWindow = new List<RectTransform>();

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
       
        var a = 0;

        for (int i = 0; i < puzzle1Answers.Length; i++)
        {
            if (puzzleInput.text.Equals(puzzle1Answers[i], StringComparison.OrdinalIgnoreCase))
            {
                a++;
                break;
            }
        }

        if (a > 0)
        {
            SwitchCorrect1(true);
            AnswerCorrect(canAdvance);
            GameData.instance.room1PuzzleOpen[0] = true;
        }
        else
        {
            StartCoroutine(AnswerFalse());
        }
       
    }

    // This is a fungtion to check puzzle 2 answer, if correct a new clue will be open and player can advance to the new map
    public void CheckingAnswer2(bool canAdvance)
    {
            var a = 0;

            for (int i = 0; i < puzzle2Answers.Length; i++)
            {
                if (puzzleInput2.text.Equals( puzzle2Answers[i],StringComparison.OrdinalIgnoreCase))
                {
                    a++;
                    break;
                }
            }

            if (a > 0)
            {
                SwitchCorrect2(true);
                AnswerCorrect(canAdvance);
                GameData.instance.room1PuzzleOpen[1] = true;
            }
            else
            {
                StartCoroutine(AnswerFalse2());
            }
       
    }
    
    // This is a fungtion to activate a clue window 1
    public void SwitchClue1(bool isActive)
    {
        if (isActive)
        {
            clue1.Play("Window Enter");
        }
        else
        {
            clue1.Play("Window Exit");
        }
    }

    // This is a fungtion to activate a clue window 2
    public void SwitchClue2(bool isActive)
    {
        bool isComplete = GameData.instance.room1PuzzleOpen[0];

        if (isComplete)
        {
            SwitchCorrect1(true);
        }
        else
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

    // This is a fungtion to activate a clue window 4
    public void SwitchClue4(bool isActive)
    {
        bool isComplete = GameData.instance.room1PuzzleOpen[1];

        if (isComplete)
        {
            SwitchCorrect2(true);
            Debug.Log("aa");
        }
        else
        {
            if (isActive)
            {
                clue4.Play("Window Enter");
            }
            else
            {
                clue4.Play("Window Exit");
            }
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
        
        if (clue2On)
        {
            clue2On = false;
            clue2.Play("Window Exit");
        }
        else if (clue4On)
        {
            clue4On = false;
            clue4.Play("Window Exit");
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

    //This coroutine is used to add a message for the player when player put a wrong answer in puzzle 2 input
    IEnumerator AnswerFalse2()
    {
        string prev = notificationDisplay2.text;

        notificationDisplay2.text = "Nothing happened";
        notificationDisplay2.color = Color.red;

        yield return new WaitForSeconds(1);
        notificationDisplay2.text = prev;
        notificationDisplay2.color = Color.black;
    }
    
    // This function is used to activate a clue when puzzle 1 answer is correct
    public void SwitchCorrect1(bool isActive)
    {
        if (isActive)
        {
            correct1.Play("Window Enter");
            clue2On = true;
        }
        else
        {
            correct1.Play("Window Exit");
        }
        
    }

    // This function is used to activate a clue when puzzle 2 answer is correct
    public void SwitchCorrect2(bool isActive)
    {
        if (isActive)
        {
            correct2.Play("Window Enter");
            clue4On = true;
        }
        else
        {
            correct2.Play("Window Exit");
        }
    }

    //new system here

    public float tweenDelay = 1f;

    public void OpenPanel(RectTransform rt)
    {
        PanelCheck(rt.name, rt);
    }

    public void ClosePanel()
    {
        openWindow[0].DOAnchorPos(new Vector2(0, 2000), tweenDelay);
        openWindow.Clear();
    }

    public List<RectTransform> panelList = new List<RectTransform>();

    void PanelCheck(string panelName, RectTransform rt)
    {
        int itemNum = 0;

        for(int i = 0; i < panelList.Count; i++)
        {
            
            var newName = panelName.Replace("Panel"," ");
            Debug.Log(newName);

            if (panelList[i].name.Contains(newName))
            {
                panelList[i].DOAnchorPos(Vector2.zero, tweenDelay);
                openWindow.Add(rt);
                break;
            }
            else
            {
                itemNum++;
            }
        }

        if(itemNum >= panelList.Count)
        {
            rt.DOAnchorPos(Vector2.zero, tweenDelay);
            openWindow.Add(rt);
        }
    }
    
    public void AdditionalPanel(RectTransform rt)
    {
        panelList.Add(rt);
        ClosePanel();
        PanelCheck(rt.name, rt);
    }
}
