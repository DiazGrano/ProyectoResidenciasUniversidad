using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SudokuManager : MonoBehaviour {

    public static SudokuManager sharedInstance;


    int N = 0;
    public int[,] grid;
    public int[,] auxGridDif;
    int[,] aux2Grid;

    public int numOfCellsToFill = 0;

    public GameObject container;


    private void Awake()
    {
        sharedInstance = this;   
    }

    public void StartSudoku(GameObject sudoku, int difficulty)
    {

        container = sudoku;
        ClearSudoku();


        N = (int)Mathf.Sqrt(container.transform.childCount);
        grid = new int[N, N];
        auxGridDif = new int[N, N];
        aux2Grid = new int[N, N];

        int x = 0;

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                grid[i, j] = 0;
                auxGridDif[i, j] = 0;
                aux2Grid[i, j] = 0;
                container.transform.GetChild(x).gameObject.GetComponent<InputField>().image.color = Color.white;
                container.transform.GetChild(x).gameObject.GetComponent<InputField>().enabled = true;
                x++;
            }

        }
        x = 0;

        switch (difficulty)
        {
            case 0:
                numOfCellsToFill = container.transform.childCount / 3;
                break;
            case 1:
                numOfCellsToFill = container.transform.childCount / 4;
                break;
            case 2:
                numOfCellsToFill = container.transform.childCount / 5;
                break;
        }

        while (!GenerateSudokuDifficulty())
        {

        }
       
    }

    private bool GenerateRandomInitialNumbers()
    {
        int x = 0, condiImp = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                    if (x <= numOfCellsToFill)
                    {
                        int auxRandomNumber = Random.Range(1, 9);
                        if (auxGridDif[i, j] == 0)
                        {
                            if (IsSafe(auxGridDif, i, j, auxRandomNumber))
                            {
                                auxGridDif[i, j] = auxRandomNumber;
                                x++;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                
                
                condiImp++;
            }
        }
        return false;
    }
  
    private bool GenerateSudokuDifficulty()
    {
        while (!GenerateRandomInitialNumbers())
        {

        }

            aux2Grid = auxGridDif;


            int x = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (auxGridDif[i, j] != 0)
                    {
                        container.transform.GetChild(x).gameObject.GetComponent<InputField>().text = auxGridDif[i, j].ToString();
                        container.transform.GetChild(x).gameObject.GetComponent<InputField>().image.color = Color.gray;
                        container.transform.GetChild(x).gameObject.GetComponent<InputField>().enabled = false;
                    }
                    x++;
                }
            }

            if (!SolveSudoku(aux2Grid))
            {

                
                auxGridDif = new int[N, N];
                ClearSudoku();

                
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        auxGridDif[i, j] = 0;
                        
                }
                }
                return false;
            }
            else
            {
                grid = auxGridDif;
            }

        return true;
    }


    public EstadoRespuesta CheckSudoku()
    {
        bool emptyCondition = false;

        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).gameObject.GetComponent<InputField>().text  == null || container.transform.GetChild(i).gameObject.GetComponent<InputField>().text == "" || container.transform.GetChild(i).gameObject.GetComponent<InputField>().text == string.Empty)
            {
                emptyCondition = true;
                break;
            }
        }

        if (emptyCondition)
        {
            return EstadoRespuesta.Incorrecta;
            
        }
        else
        {
            if (!SolveSudoku(grid))
            {
                return EstadoRespuesta.Incorrecta;
            }
        }
        return EstadoRespuesta.Correcta;
    }




    public void ClearSudoku()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.GetComponent<InputField>().text = null;
            container.transform.GetChild(i).gameObject.GetComponent<InputField>().image.color = Color.white;
            container.transform.GetChild(i).gameObject.GetComponent<InputField>().enabled = true;
        }
            
    }

    public void Solve()
    {
        int x = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                string str = container.transform.GetChild(x).gameObject.transform.GetComponentInChildren<Text>().text;

                if (str != string.Empty)
                {
                    grid[i, j] = int.Parse(str);
                }
                else
                {
                    grid[i, j] = 0;
                }

                x++;
            }
        }

        if (SolveSudoku(grid))
            PrintGrid(ref grid);
        else
            Debug.Log("!No Solution Exists");
    }

    private void PrintGrid(ref int[,] grid)
    {
        int[] tempGrid = new int[container.transform.childCount];
        int x = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                tempGrid[x] = grid[i, j];
                x++;
            }
        }
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.GetComponent<InputField>().text = tempGrid[i].ToString();
        }
    }

    private bool FindUnassignedLocation(int[,] grid, ref int row, ref int col)
    {
        for (row = 0; row < N; row++)
            for (col = 0; col < N; col++)
                if (grid[row, col] == 0)
                    return true;
        return false;
    }

    private bool UsedInRow(int[,] grid, int row, int num)
    {
        for (int col = 0; col < N; col++)
            if (grid[row, col] == num)
                return true;
        return false;
    }

    private bool UsedInCol(int[,] grid, int col, int num)
    {
        for (int row = 0; row < N; row++)
            if (grid[row, col] == num)
                return true;
        return false;
    }

    private bool UsedInBox(int[,] grid, int BoxStartRow, int BoxStartCol, int num)
    {
        for (int row = 0; row < (int)Mathf.Sqrt(N); row++)
            for (int col = 0; col < (int)Mathf.Sqrt(N); col++)
                if (grid[row + BoxStartRow, col + BoxStartCol] == num)
                    return true;
        return false;
    }

    private bool IsSafe(int[,] grid, int row, int col, int num)
    {
        return !UsedInRow(grid, row, num)
            && !UsedInCol(grid, col, num)
            && !UsedInBox(grid, row - row % (int)Mathf.Sqrt(N), col - col % (int)Mathf.Sqrt(N), num);
    }

    private bool SolveSudoku(int[,] grid)
    {
        int row = new int();
        int col = new int();

        if (!FindUnassignedLocation(grid, ref row, ref col))
            return true;

        for (int num = 1; num <= 9; num++)
        {
            if (IsSafe(grid, row, col, num))
            {
                grid[row, col] = num;
                if (SolveSudoku(grid))
                    return true;
                grid[row, col] = 0;
            }
        }

        return false;
    }


}
