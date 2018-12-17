using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine.SceneManagement;
//using Sudoku.Jobs;

/// <summary>
/// This is a listner which will be implemented by the level manager to listen from sudoku board.
/// </summary>
public interface IManager
{
    void MaintainMoves();
}

public class LevelManager : MonoBehaviour, IManager {

    //SudokuBoard related data
    SudokuBoard board = new SudokuBoard();

    // Setting up prefabs and links to game objets.
    [SerializeField]
    private GameObject gameBoard;
    [SerializeField]
    private GameObject optionsBoard;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private GameObject optionsButtonPrefab;

    ///Level Related data
    GameObject[,] buttonGrid = new GameObject[9, 9];
    private string[] levelStrings = { "Level1", "Level2", "Level3" };
    private int currentLevel = 0;

    //Play related data
    private int currentNumber = 1;

    //Movement stack for undo feature.
    Move lastMove;
    MovementStack moves = new MovementStack();

    //Data related timer
    [SerializeField]
    private Text timerText;
    private static double timeRemaninig;
    private double[] timeLimit = { 5, 7000, 1000 };
    

    //Timer Job  related data
    JobHandle timerHandle;
    TimerJob timerJob;

    public static double TimeRemaninig
    {
        get
        {
            return timeRemaninig;
        }

        set
        {
            timeRemaninig = value;
        }
    }

  

    /// <summary>
    /// Default start method.
    /// </summary>
    private void Start()
    {

        GenerateOptionsBoard();
        board.GenerateGameBoard(levelStrings[currentLevel],gameBoard,buttonPrefab,ref buttonGrid);
        MaintainMoves();

        timerJob = new TimerJob
        {
            StartTime = 0,
            MaxTime = timeLimit[currentLevel]
        };
        timerHandle = timerJob.Schedule();
        JobHandle.ScheduleBatchedJobs();
    }

    /// <summary>
    /// Default generic method. Updates the timer text.
    /// </summary>
    private void Update()
    {
        Debug.Log(timeRemaninig);
        timerText.text = ((int)timeRemaninig).ToString();
        if (timeRemaninig >= timeLimit[currentLevel])
        {
            timerHandle.Complete();
            SceneManager.LoadScene("GameOver");
        }
        //deltaTime = Time.deltaTime;
    }
    /// <summary>
    /// Sets up the buttons on very right of window.
    /// </summary>
    public void GenerateOptionsBoard()
    {
        //Setup options board
        GameObject tmp_gameObject;
        Button tmp_button;
        RectTransform rectTransform;
        Text text;
        for (int i = 1; i < 10; i++)
        {
            tmp_gameObject = MonoBehaviour.Instantiate(optionsButtonPrefab);
            tmp_button = tmp_gameObject.GetComponent<Button>();
            tmp_button.name = i + "";
            tmp_button.onClick.AddListener(ChangeCurrentNumber);
            rectTransform = tmp_button.GetComponent<RectTransform>();
            rectTransform.SetParent(optionsBoard.transform, false);
            text = tmp_button.GetComponentInChildren<Text>();
            text.text = i + "";
        }
    }


    /// <summary>
    /// Action listners for the option buttons.
    /// </summary>
    public void ChangeCurrentNumber()
    {
        currentNumber = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(currentNumber);
        board.CurrentNumber= currentNumber;
    }


    


    /// <summary>
    /// Implements the Undo functionality with the help of movement stack.
    /// </summary>
    public void Undo()
    {
        Debug.Log("in Undo");
        lastMove = moves.Pop();
        if (lastMove != null)
        {
            board.SetCellValue(lastMove.Row, lastMove.Column, lastMove.Value);
            buttonGrid[lastMove.Row, lastMove.Column].GetComponentInChildren<Text>().text = lastMove.Value + "";
            buttonGrid[lastMove.Row, lastMove.Column].GetComponentInChildren<Text>().color = (lastMove.Value == 0) ? Color.black :
                                                                            (lastMove.IsValid) ? Color.green : Color.red;

        }
    }

    /// <summary>
    /// Implementation of interface method,which tracks the moves on sudoku board.
    /// </summary>
    public void MaintainMoves()
    {
        board.MoveToStack += delegate
        {
            try
            {
                moves.Push(board.LastMove.Value, board.LastMove.Row, board.LastMove.Column, board.LastMove.IsValid);
            }
            catch (Exception e)
            {
                Debug.LogError("Last move has invalid data or null value.");
            }
        };
    }


    

}
