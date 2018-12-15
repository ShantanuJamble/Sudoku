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
        for (int i = 0; i < 9; i++)
        {
            tmp_input_array = inputLines[i].ToCharArray();
            for (int j = 0; j < 9; j++)
            {
                //Stops code from creating new buttons while switching levels
                if (buttonGrid[i, j] == null)
                {
                    buttonGrid[i, j] = Instantiate(buttonPrefab);
                    tmp_button = buttonGrid[i, j].GetComponent<Button>();
                    tmp_button.onClick.AddListener(Play);
                }
                rectTransform = buttonGrid[i, j].GetComponent<RectTransform>();
                rectTransform.SetParent(gameBoard.transform, false);
                text = buttonGrid[i, j].GetComponentInChildren<Text>();
                text.text = tmp_input_array[j] + "";
                sudokuGrid[i,j] = (int)tmp_input_array[j];


            }
        }
    }

    public void ChangeCurrentNumber()
    {
        currentNumber =int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(currentNumber);
    }

    public void Play()
    {
        Debug.Log("You played");
    }
}
