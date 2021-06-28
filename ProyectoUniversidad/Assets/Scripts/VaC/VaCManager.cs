using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaCManager : MonoBehaviour {
    public static VaCManager sharedInstance;

    private IdentificadorTema identificadorTema = IdentificadorTema.VaC;
    private TipoResultado tipoResultado = TipoResultado.Tiempo;


    public List<GameObject> sudokuTypes;

    public Text campoTiempoActual;
    public Text campoMejorTiempo;

    public int auxSeconds;

    private string VaCTiempoActual = "0 : 0";

    public GameObject finishButton;

    [Header("Instrucciones")]
    public GameObject objetoInstrucciones;

    private void Awake()
    {
        sharedInstance = this;
        finishButton.GetComponent<Button>().enabled = false; 
    }

    

    private void Start()
    {
        PanelInstrucciones.sharedInstance.ModificarInstrucciones(objetoInstrucciones);
        AbrirInstrucciones();

        ManagerCalificar.sharedInstance.ActualizarVariablesCalificar(identificadorTema, campoMejorTiempo, campoTiempoActual, tipoResultado);

        GameManager.sharedInstance.EscenaActividadCargada();
    }


    public void AbrirInstrucciones()
    {
        PanelInstrucciones.sharedInstance.AbrirInstrucciones();
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
        if (GameManager.sharedInstance.Jugando())
        {
            StopAllCoroutines();
            Reiniciar();

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

                ManagerPuntuaciones.sharedInstance.ActualizarVariablesRegistro(identificadorTema, sudokuType.ToString() + sudokuDifficulty.ToString(), tipoResultado);
            Reiniciar();
        }   
    }


    IEnumerator VaCGameTimeManager()
    {
            while (true)
            {
            yield return new WaitWhile(() => GameManager.sharedInstance.Jugando() != true);

            yield return new WaitForSeconds(1);

            if (GameManager.sharedInstance.Jugando())
            {
                auxSeconds++;

                VaCTiempoActual = GameManager.sharedInstance.ConversionTiempo(auxSeconds.ToString());
                campoTiempoActual.text = VaCTiempoActual;
            }
            
            }  
    }







    public void VaCCalificar()
    {
        if (GameManager.sharedInstance.Jugando())
        {
            StopAllCoroutines();

            ManagerCalificar.sharedInstance.Calificar(SudokuManager.sharedInstance.CheckSudoku(), auxSeconds);
            sudokuTypes[sudokuType].gameObject.SetActive(false);
            finishButton.GetComponent<Button>().enabled = false;
        }
    }

    public void Reiniciar()
    {
        VaCTiempoActual = "0 : 0";
        campoTiempoActual.text = VaCTiempoActual;
        auxSeconds = 0;
        campoMejorTiempo.text = GameManager.sharedInstance.ConversionTiempo(ManagerPuntuaciones.sharedInstance.ConsultarMejorRegistro());
    }


    public void BotonRegresar()
    {

        if (GameManager.sharedInstance.Jugando())
        {
            GameManager.sharedInstance.CerrarEscenaActividad();
        }

        
    }

}
