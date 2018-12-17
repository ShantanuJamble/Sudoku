using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Deleget for moving move status to stack.
public delegate void PushMovesToStack();



public class SudokuBoard
{
    private int[,] sudokuGrid = new int[9, 9];
    private int currentNumber;
    private Move lastMove = new Move();
    //Listeners for the buttons on grid
    public PushMovesToStack MoveToStack;
    private int cellsFilled = 0;


    /// <summary>
    /// Setter for the currently selected option. 
    /// </summary>
    public int CurrentNumber
    {
        set 
        {
            this.currentNumber = value;
        }
    }

    /// <summary>
    /// Getter for the last move played by user.
    /// </summary>
    public Move LastMove
    {
        get
        {
            return lastMove;
        }
    }

    /// <summary>
    /// Getter and Setter for the Cellsfilled count.
    /// </summary>
    public int CellsFilled
    {
        get
        {
            return cellsFilled;
        }
        set
        {
            cellsFilled += value;
        }
    }

    /// <summary>
    /// Updates the cell value.
    /// </summary>
    /// <param name="value"> New value to be updated</param>
    /// <param name="row">Index of row.</param>
    /// <param name="col">Index of column.</param>
    public void SetCellValue(int value,int row,int col)
    {
        try
        {
            sudokuGrid[row, col] = value;
        }
        catch (Exception e)
        {
            Debug.Log("Invalid indices for sudokugrid.\n"+ e.Message);
        }
    }

    /// <summary>
    /// Sets up level for us taking the file name.
    /// </summary>
    /// <param name="filename">String for the filename.</param>
    public void GenerateGameBoard(string filename,GameObject gameBoard,GameObject buttonPrefab,ref GameObject[,] buttonGrid)
    {
        TextAsset data = Resources.Load(filename) as TextAsset;
        Button tmp_button;
        string[] inputLines = data.text.Split('\n');
        Vector2 pos = new Vector2(-180f, 180f);
        RectTransform rectTransform;
        Text text;
        char[] tmp_input_array = null;
        for (int row_index = 0; row_index < 9; row_index++)
        {
            tmp_input_array = inputLines[row_index].ToCharArray();
            for (int col_index = 0; col_index < 9; col_index++)
            {
                //Stops code from creating new buttons while switching levels
                if (buttonGrid[row_index, col_index] == null)
                {
                    buttonGrid[row_index, col_index] = MonoBehaviour.Instantiate(buttonPrefab);
                    //gives us location of the button in terms of grid
                    buttonGrid[row_index, col_index].name = row_index + "," + col_index;
                }
                rectTransform = buttonGrid[row_index, col_index].GetComponent<RectTransform>();
                rectTransform.SetParent(gameBoard.transform, false);

                tmp_button = buttonGrid[row_index, col_index].GetComponent<Button>();
                tmp_button.onClick.AddListener(Play);
                text = buttonGrid[row_index, col_index].GetComponentInChildren<Text>();
                if (tmp_input_array[col_index] != '0')
                    tmp_button.interactable = false;
                text.text = tmp_input_array[col_index] + "";
                sudokuGrid[row_index, col_index] = int.Parse(tmp_input_array[col_index] + "");
                CellsFilled += (sudokuGrid[row_index, col_index] != 0) ? 1 : 0;
            }
        }
    }


    

    /// <summary>
    /// Action listner for the sudokuboard buttons.
    /// </summary>
    public void Play()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        gameObject.GetComponentInChildren<Text>().text = currentNumber + "";
        int row_index = int.Parse(gameObject.name.Split(',')[0]);
        int col_index = int.Parse(gameObject.name.Split(',')[1]);
        lastMove.Value = sudokuGrid[row_index, col_index];
        lastMove.Row = row_index;
        lastMove.Column = col_index;
    

        //Run check for the validity of move
        if (IsValidMove(row_index, col_index))
        {
            gameObject.GetComponentInChildren<Text>().color = Color.green;
            lastMove.IsValid = true;
            CellsFilled++;
        }
        else
        {
            gameObject.GetComponentInChildren<Text>().color = Color.red;
            lastMove.IsValid = false;
        }
        MoveToStack();
        sudokuGrid[row_index, col_index] = currentNumber;
        
    }


    /// <summary>
    /// Checks for the move validity by checking the input with row,column and box.
    /// </summary>
    /// <param name="row_index"></param>
    /// <param name="col_index"></param>
    /// <returns></returns>
    private bool IsValidMove(int row_index, int col_index)
    {
        Debug.Log("In here");
        for (int counter = 0; counter < 9; counter++)
        {
            if (sudokuGrid[row_index, counter] == currentNumber || sudokuGrid[counter, col_index] == currentNumber)
            {
                return false;
            }
        }
        //Check for mini square using spatial hashing
        int sq_start_row = (row_index / 3) * 3;
        int sq_start_col = (col_index / 3) * 3;
        for (int tmp_row_index = sq_start_row; tmp_row_index < sq_start_row + 3; tmp_row_index++)
        {
            for (int tmp_col_index = sq_start_col; tmp_col_index < sq_start_col + 3; tmp_col_index++)
            {
                Debug.Log(tmp_row_index + "  " + tmp_col_index + "  " + sudokuGrid[tmp_row_index, tmp_col_index]);
                if (sudokuGrid[tmp_row_index, tmp_col_index] == currentNumber)
                {
                    return false;
                }
            }
        }
        return true;
    }







}
