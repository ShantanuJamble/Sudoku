using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move
{
    public int Row;
    public int Column;
    public int Value;
    public bool IsValid;
}

/// <summary>
/// Stack for tracking moves. Can be used for undo feature.
/// </summary>
public class MovementStack {

    int stacktop;
    Move[] moveArray;

    public MovementStack() {
        stacktop = -1;
        moveArray= new Move[5];
    }
    
    /// <summary>
    /// Pushes the data of current move into array of moves.Maintains last five moves at max at any point of time.
    /// </summary>
    /// <param name="value">Last value at the cell</param>
    /// <param name="row">Row of cell.</param>
    /// <param name="col">Col of cell.</param>
    /// <param name="isValid">Was the move valid.</param>
    public void Push(int value,int row,int col,bool isValid=true)
    {
        Debug.Log("In push");
        //Objects will be created only once and then we will keep resuing them so as we don't trigger garbahe collection.
        if (stacktop == 4)
        {
            for(int index = 0; index < 4; index++)
            { 
                moveArray[index].Value = moveArray[index + 1].Value;
                moveArray[index].Row = moveArray[index + 1].Row;
                moveArray[index].Column = moveArray[index + 1].Column;
                moveArray[index].IsValid = moveArray[index + 1].IsValid;
            }
            moveArray[4].Value = value;
            moveArray[4].Row = row;
            moveArray[4].Column = col;
            moveArray[4].IsValid = isValid;
        }
        else
        {
            stacktop++;
            if (moveArray[stacktop] == null)
            {
                moveArray[stacktop] = new Move();
            }
            moveArray[stacktop].Value = value;
            moveArray[stacktop].Row = row;
            moveArray[stacktop].Column = col;
            moveArray[stacktop].IsValid = isValid;
        }

    }


    /// <summary>
    /// returns the latest move. Null if the array is empty.
    /// </summary>
    /// <returns></returns>
    public Move Pop()
    {
        if (stacktop == -1)
        {
            return null;
        }
        else
        {
            return moveArray[stacktop--];
        }
    }
	
}
