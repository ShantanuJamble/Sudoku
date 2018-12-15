using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {


    // Setting up prefabs and links to game objets.
    [SerializeField]
    private GameObject gameBoard;
    [SerializeField]
    private GameObject optionsBoard;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private GameObject ansButtonPrefab;


    ///Level Related data
    private string[] levelStrings = {"Level1", "Level2", "Level3" };
    private int currentLevel = 0;
    private GameObject[,] buttonGrid = new GameObject[9, 9];
    private int[,] sudokuGrid = new int[9, 9];
    private int currentNumber = 1;


    //Movement stack for undo feature.
    Move last_move;
    MovementStack moves = new MovementStack();

    //Time for the sudoku solving

    /// <summary>
    /// Default start method.
    /// </summary>
    private void Start()
    {

        GenerateOptionsBoard();
        GenerateGameBoard(levelStrings[currentLevel]);

    }


    /// <summary>
    /// Sets up the buttons on very right of window.
    /// </summary>
    private void GenerateOptionsBoard()
    {
        //Setup options board
        GameObject tmp_gameObject;
        Button tmp_button;
        RectTransform rectTransform;
        Text text;
        for (int i = 1; i < 10; i++)
        {
            tmp_gameObject = Instantiate(ansButtonPrefab);
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
    /// Sets up level for us taking the file name.
    /// </summary>
    /// <param name="filename">String for the filename.</param>
    private void GenerateGameBoard(string filename)
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
                    buttonGrid[row_index, col_index] = Instantiate(buttonPrefab);
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
                sudokuGrid[row_index,col_index] = int.Parse(tmp_input_array[col_index]+"");
            }
        }
    }


    /// <summary>
    /// Action listners for the option buttons.
    /// </summary>
    public void ChangeCurrentNumber()
    {
        currentNumber =int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(currentNumber);
    }

    /// <summary>
    /// Action listner for the sudokuboard buttons.
    /// </summary>
    public void Play()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        gameObject.GetComponentInChildren<Text>().text = currentNumber+"";
        int row_index = int.Parse(gameObject.name.Split(',')[0]);
        int col_index = int.Parse(gameObject.name.Split(',')[1]);
        
        
        //Run check for the validity of move
        if (IsValidMove(row_index, col_index))
        {
            gameObject.GetComponentInChildren<Text>().color = Color.green;
            moves.Push(sudokuGrid[row_index, col_index], row_index, col_index, true);
        }
        else
        {
            gameObject.GetComponentInChildren<Text>().color = Color.red;
            moves.Push(sudokuGrid[row_index, col_index], row_index, col_index, false);
        }

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
        //Check for mini square
        int sq_start_row = (row_index / 3) * 3;
        int sq_start_col = (col_index / 3) * 3;
        for (int tmp_row_index = sq_start_row;tmp_row_index < sq_start_row + 3; tmp_row_index++)
        {
            for(int tmp_col_index = sq_start_col; tmp_col_index < sq_start_col + 3; tmp_col_index++)
            {
                Debug.Log(tmp_row_index + "  " + tmp_col_index+"  "+ sudokuGrid[tmp_row_index, tmp_col_index]);
                if (sudokuGrid[tmp_row_index, tmp_col_index] == currentNumber)
                {
                    return false;
                }
            }
        }
        return true;
    }



    public void Undo()
    {
        Debug.Log("in Undo");
        last_move = moves.Pop();
        if (last_move != null)
        {
            sudokuGrid[last_move.Row, last_move.Column] = last_move.Value;
            buttonGrid[last_move.Row, last_move.Column].GetComponentInChildren<Text>().text = last_move.Value +"";
            buttonGrid[last_move.Row, last_move.Column].GetComponentInChildren<Text>().color = (last_move.Value==0)?Color.black:
                                                                            (last_move.IsValid)?Color.green:Color.red;
            
        }
    }
}
