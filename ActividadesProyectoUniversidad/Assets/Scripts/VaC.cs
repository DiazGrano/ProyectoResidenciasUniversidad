using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaC : MonoBehaviour {

    public List<GameObject> sudokuTypes;

    public Text campoTiempoActual;

    public Text campoMejorTiempo;


    public float timeElapsed;

    public int auxSeconds;
    public int seconds;
    public int minutes;


    public GameObject finishButton;



    private void Awake()
    {
        finishButton.GetComponent<Button>().enabled = false;
    }



    public int sudokuType;

    public int sudokuDifficulty;

    public void SetSudokuType(int type)
    {
        this.sudokuType = type;
    }

    public void SetSudokuDifficulty(int difficulty)
    {
        this.sudokuDifficulty = difficulty;
    }


    public void StartGame()
    {


        StopAllCoroutines();
        seconds = 0;
        minutes = 0;
        timeElapsed = 0f;
        
        for (int i = 0; i < sudokuTypes.Count; i++)
        {
            if (i != sudokuType)
            {
                sudokuTypes[i].gameObject.SetActive(false);
            }
            else
            {
                sudokuTypes[i].gameObject.SetActive(true);
                SudokuManager.sharedInstance.StartSudoku(sudokuTypes[sudokuType], sudokuDifficulty);
                StartCoroutine(VaCGameTimeManager());

                finishButton.GetComponent<Button>().enabled = true;
                
            }
        }
        

        

    }


    IEnumerator VaCGameTimeManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            auxSeconds++;

            minutes = (int)(auxSeconds / 60);
            seconds = auxSeconds - (minutes * 60);

            campoTiempoActual.text = minutes + " : " + seconds;
        }
    }


    public void VaCCalificar()
    {
        if (SudokuManager.sharedInstance.CheckSudoku())
        {
            Debug.Log("Sudoku resuelto correctamente");
        }
        else
        {
            Debug.Log("Sudoku resuelto incorrectamente");
        }



    }

    public void VaCMejorPuntuacion()
    {




    }



}
