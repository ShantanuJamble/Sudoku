using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {


    // Read the levle structre from file 
    //update the grid with the values and make cells unmodifiable

    [SerializeField]
    private GameObject gameBoard;
    [SerializeField]
    private GameObject buttonPrefab;

    ///Level Related data
    private string[] levelStrings = {"Level1", "Level2", "Level3" };
    private int currentLevel = 0;
    private GameObject[,] buttonGrid = new GameObject[9, 9];
    
    //Time for the sudoku solving

    /// <summary>
    /// Default start method.
    /// </summary>
    private void Start()
    {
      
        GenerateBoard(levelStrings[currentLevel]);
    }


    /// <summary>
    /// Sets up level for us taking the file name.
    /// </summary>
    /// <param name="filename">String for the filename.</param>
    private void GenerateBoard(string filename)
    {
        TextAsset data = Resources.Load(filename) as TextAsset;
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
                }
                rectTransform = buttonGrid[i, j].GetComponent<RectTransform>();
                rectTransform.SetParent(gameBoard.transform, false);
                text = buttonGrid[i, j].GetComponentInChildren<Text>();
                text.text = tmp_input_array[j] + "";


            }
        }
    }
}
