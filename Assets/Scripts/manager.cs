/*
  * Course: CS4242 ONLINE Spring 2019
  * Student name: Selena Guillen
  * Student ID: 000514390
  * Assignment #: #2
  * Due Date: March 7, 2019

    This script will solve the 8 puzzle game using the A* search algorithm
    The methods called are as follows:
	    createBoard() : creates a matrix with values ranging from 0-8
        buildBoard() : fills matrix(board) with gameObjects(tiles)
        solvePuzzle() : Takes each possible state, calculates the heuristic value for each state, finds the minimum heuristic value,
                        and executes the instruction for the smallest value.



    Supporting Methods: 
        copy() : copies a matrix/board
        calcH(): calculates heuristic value
        findMin(): finds minimum heuristic value and assigns a string instruction to it.
        animate(): animates the GameObjects 
 */



using System;
using System.Collections;
using UnityEngine;

public class manager : MonoBehaviour
{
    /*Initialization of Data */

    //initialize GameObjects (cubes) as tiles
    public GameObject tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8;

    //intializes the rows, can set intial placement using these
    public int[] row1 = new int[3];
    public int[] row2 = new int[3];
    public int[] row3 = new int[3];

    //initializes the board as matrix
    int[,] board = new int[3, 3];

    //initialize blank space position [i,j]
    int blank_row, blank_col;

    //initialize count for iterations/moves
    int count;

    //initialize tileIns and dirIns, records tiles that move 
    int[] tileIns = new int[1000];
    string[] dirIns = new string[1000];

    // Start is called before the first frame update
    // Call methods here
    void Start()
    {
        createBoard();
        buildBoard();
        solvePuzzle();

        StartCoroutine(animate());

    }

    /* Methods */

    //Fills board matrix with int values (0-8)
    void createBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            board[2, i] = row1[i];
            board[1, i] = row2[i];
            board[0, i] = row3[i];
        }
    }

    //Fill board[,] with gameObjects corresponding to int value. 
    //if int value = 1, tile1's position is assigned
    //if int value = 0, blank_row and blank_col are recorded as blank space's postion
    void buildBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                switch (board[i, j])
                {
                    case 1:
                        tile1.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 2:
                        tile2.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 3:
                        tile3.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 4:
                        tile4.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 5:
                        tile5.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 6:
                        tile6.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 7:
                        tile7.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    case 8:
                        tile8.transform.position = new Vector3((float)j, (float)i, 0f);
                        break;
                    //blank space
                    case 0:
                        blank_row = i;
                        blank_col = j;
                        break;
                }
            }
        }
    }

    //This is a copy method to copy the current state of the puzzle in order to modifiy it.
    int[,] copy(int[,] state)
    {
        int[,] temp = new int[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                temp[i, j] = state[i, j];
            }
        }
        return temp;
    }

    //Calculate heuristic value of state
    //heuristic rule: tiles out of place
    int calcH(int[,] state)
    {
        int h = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                switch (state[i, j])
                {
                    //h = h + distance
                    case 1:
                        h += Mathf.Abs(i - 2) + Mathf.Abs(j - 0); //(0,2)
                        break;
                    case 2:
                        h += Mathf.Abs(i - 2) + Mathf.Abs(j - 1); //(1,2)
                        break;
                    case 3:
                        h += Mathf.Abs(i - 2) + Mathf.Abs(j - 2); //(2,2)
                        break;
                    case 4:
                        h += Mathf.Abs(i - 1) + Mathf.Abs(j - 2); //(2,1)
                        break;
                    case 5:
                        h += Mathf.Abs(i - 0) + Mathf.Abs(j - 2); //(2,0)
                        break;
                    case 6:
                        h += Mathf.Abs(i - 0) + Mathf.Abs(j - 1); //(1,0)
                        break;
                    case 7:
                        h += Mathf.Abs(i - 0) + Mathf.Abs(j - 0); //(0,0)
                        break;
                    case 8:
                        h += Mathf.Abs(i - 1) + Mathf.Abs(j - 0); //(0,1)
                        break;
                }
            }
        }
        return h;
    }

    //Return a state string with the lowest heuristic value
    //Places heuristic values into an array, sorts and takes the first element (smallest value) 
    //Assigns state of the smallest value
    string findMin(int h_U, int h_D, int h_L, int h_R)
    {
        string state = "state";
        int[] compare_array = new int[] { h_U, h_D, h_L, h_R };
        int min_val;

        Array.Sort(compare_array);
        min_val = compare_array[0];

        if (min_val == h_U)
        {
            state = "up";
        }
        if (min_val == h_D)
        {
            state = "down";
        }
        if (min_val == h_L)
        {
            state = "left";
        }
        if (min_val == h_R)
        {
            state = "right";
        }

        return state;

    }

    //Takes each possible state, calculates the heuristic value for each state, finds the minimum heuristic value, 
    //and executes the instruction for the smallest value.
    void solvePuzzle()
    {
        //intialize minState("up", "down", "left", "right")
        //This is going to be used to assign a string value to the move with the smallest heuristic value.
        string minState = "i";

        //initialize states
        int[,] stateUP = new int[3, 3];
        int[,] stateDOWN = new int[3, 3];
        int[,] stateLEFT = new int[3, 3];
        int[,] stateRIGHT = new int[3, 3];

        //initialize heuristic values
        int h_UP, h_DOWN, h_LEFT, h_RIGHT;

        //Maximum moves: 1000
        for (count = 0; count < 1000; count++)
        {
            stateUP = copy(board);
            stateDOWN = copy(board);
            stateLEFT = copy(board);
            stateRIGHT = copy(board);

            /* stateUP */

            //if stateUP moves blank outside of the puzzle, set heuristic to 1000.
            if (blank_row + 1 > 2)
            {
                h_UP = 1000;
            }

            //if backtrack
            else if (minState == "down")
            {
                h_UP = 999;
            }

            else
            {
                //Blank is moved up and its value is set to 0.
                stateUP[blank_row, blank_col] = stateUP[blank_row + 1, blank_col];
                stateUP[blank_row + 1, blank_col] = 0;

                h_UP = calcH(stateUP);
            }

            /* stateDOWN */

            //if stateDOWN moves blank outside of the puzzle, set heuristic to 1000.
            if (blank_row - 1 < 0)
            {
                h_DOWN = 1000;
            }

            //if backtrack
            else if (minState == "up")
            {
                h_DOWN = 999;
            }

            else
            {
                //Blank is moved down and its value is set to 0.
                stateDOWN[blank_row, blank_col] = stateDOWN[blank_row - 1, blank_col];
                stateDOWN[blank_row - 1, blank_col] = 0;

                h_DOWN = calcH(stateDOWN);
            }

            /* stateLEFT */

            //if stateLEFT moves blank outside of the puzzle, set heuristic to 1000.
            if (blank_col - 1 < 0)
            {
                h_LEFT = 1000;
            }

            //if backtrack
            else if (minState == "right")
            {
                h_LEFT = 999;
            }

            else
            {
                //Blank is moved left and its value is set to 0.
                stateLEFT[blank_row, blank_col] = stateLEFT[blank_row, blank_col - 1];
                stateLEFT[blank_row, blank_col - 1] = 0;

                h_LEFT = calcH(stateLEFT);
            }

            /* stateRIGHT */

            //if stateRIGHT moves blank outside of the puzzle, set heuristic to 1000.
            if (blank_col + 1 > 2)
            {
                h_RIGHT = 1000;
            }

            //if backtrack
            else if (minState == "left")
            {
                h_RIGHT = 999;
            }

            else
            {
                //Blank is moved right and its value is set to 0.
                stateRIGHT[blank_row, blank_col] = stateRIGHT[blank_row, blank_col + 1];
                stateRIGHT[blank_row, blank_col + 1] = 0;

                h_RIGHT = calcH(stateRIGHT);
            }

            //uses findMin function to find min heuristic value and sets state equal to it
            minState = findMin(h_UP, h_DOWN, h_LEFT, h_RIGHT);

            //Takes the min heuristic move and adds to instruction list
            switch (minState)
            {
                case "up":
                    tileIns[count] = board[blank_row + 1, blank_col];
                    dirIns[count] = "down";
                    board = stateUP;
                    blank_row++;
                    break;
                case "down":
                    tileIns[count] = board[blank_row - 1, blank_col];
                    dirIns[count] = "up";
                    board = stateDOWN;
                    blank_row--;
                    break;
                case "left":
                    tileIns[count] = board[blank_row, blank_col - 1];
                    dirIns[count] = "right";
                    board = stateLEFT;
                    blank_col--;
                    break;
                case "right":
                    tileIns[count] = board[blank_row, blank_col + 1];
                    dirIns[count] = "left";
                    board = stateRIGHT;
                    blank_col++;
                    break;

            }

            if (calcH(board) == 0)
                break;
        }
    }

    /*Animation*/
    public IEnumerator animate()
    {
        float elapsedTime;
        Vector3 startingPos;
        Vector3 endPos;
        Transform tile;
        for (int i = 0; i <= count; i++)
        {
            elapsedTime = 0f;
            switch (tileIns[i])
            {
                case 1:
                    tile = tile1.transform;
                    break;
                case 2:
                    tile = tile2.transform;
                    break;
                case 3:
                    tile = tile3.transform;
                    break;
                case 4:
                    tile = tile4.transform;
                    break;
                case 5:
                    tile = tile5.transform;
                    break;
                case 6:
                    tile = tile6.transform;
                    break;
                case 7:
                    tile = tile7.transform;
                    break;
                case 8:
                    tile = tile8.transform;
                    break;
                default:
                    tile = tile1.transform;
                    break;
            }
            startingPos = tile.position;

            switch (dirIns[i])
            {
                case "up":
                    endPos = new Vector3(startingPos.x, startingPos.y + 1f, 0f);
                    break;
                case "down":
                    endPos = new Vector3(startingPos.x, startingPos.y - 1f, 0f);
                    break;
                case "left":
                    endPos = new Vector3(startingPos.x - 1f, startingPos.y, 0f);
                    break;
                case "right":
                    endPos = new Vector3(startingPos.x + 1f, startingPos.y, 0f);
                    break;
                default:
                    endPos = startingPos;
                    break;
            }

            while (elapsedTime < 0.1f)
            {
                tile.position = Vector3.Lerp(startingPos, endPos, elapsedTime / 0.1f);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            tile.position = endPos;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
