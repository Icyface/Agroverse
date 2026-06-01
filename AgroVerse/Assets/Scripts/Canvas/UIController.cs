using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Paneles Principales Interactivos")]
    public GameObject seccionListaTareas;
    public GameObject seccionAjustes;

    [Header("Paneles de Tareas Individuales")]
    public GameObject panelRecogerHuevos;
    public GameObject panelLimpiarCerdos;
    public GameObject panelAlimentarAnimales;
    public GameObject panelOrdeñarVaca;

    [Header("Configuración VR")]
    public float distanciaAlJugador = 2.0f;
    public float alturaFijaSuelo = 1.3f;

    public void CerrarPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void ToggleSeccionTareas()
    {
        if (seccionListaTareas != null)
        {
            if (seccionListaTareas.activeSelf)
            {
                seccionListaTareas.SetActive(false);
                OcultarTodasLasTareas();
            }
            else
            {
                if (seccionAjustes) seccionAjustes.SetActive(false);

                PosicionarFrenteAlJugador(seccionListaTareas);
                seccionListaTareas.SetActive(true);
            }
        }
    }

    public void ToggleSeccionAjustes()
    {
        if (seccionAjustes != null)
        {
            if (seccionAjustes.activeSelf)
            {
                seccionAjustes.SetActive(false);
                OcultarTodasLasTareas();
            }
            else
            {
                if (seccionListaTareas) seccionListaTareas.SetActive(false);
                OcultarTodasLasTareas();

                PosicionarFrenteAlJugador(seccionAjustes);
                seccionAjustes.SetActive(true);
            }
        }
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void MostrarTareaRecogerHuevos()
    {
        OcultarTodasLasTareas();
        if (panelRecogerHuevos != null)
        {
            PosicionarFrenteAlJugador(panelRecogerHuevos);
            panelRecogerHuevos.SetActive(true);
        }
    }

    public void MostrarTareaLimpiarCerdos()
    {
        OcultarTodasLasTareas();
        if (panelLimpiarCerdos != null)
        {
            PosicionarFrenteAlJugador(panelLimpiarCerdos);
            panelLimpiarCerdos.SetActive(true);
        }
    }

    public void MostrarTareaAlimentarAnimales()
    {
        OcultarTodasLasTareas();
        if (panelAlimentarAnimales != null)
        {
            PosicionarFrenteAlJugador(panelAlimentarAnimales);
            panelAlimentarAnimales.SetActive(true);
        }
    }

    public void MostrarTareaOrdeñarVaca()
    {
        OcultarTodasLasTareas();
        if (panelOrdeñarVaca != null)
        {
            PosicionarFrenteAlJugador(panelOrdeñarVaca);
            panelOrdeñarVaca.SetActive(true);
        }
    }

    private void OcultarTodasLasTareas()
    {
        if (panelRecogerHuevos) panelRecogerHuevos.SetActive(false);
        if (panelLimpiarCerdos) panelLimpiarCerdos.SetActive(false);
        if (panelAlimentarAnimales) panelAlimentarAnimales.SetActive(false);
        if (panelOrdeñarVaca) panelOrdeñarVaca.SetActive(false);
    }

    private void PosicionarFrenteAlJugador(GameObject panel)
    {
        Transform camaraVR = Camera.main != null ? Camera.main.transform : null;

        if (camaraVR != null && panel != null)
        {
            Vector3 direccionFrente = camaraVR.forward;
            direccionFrente.y = 0;
            direccionFrente.Normalize();

            Vector3 posicionFinal = camaraVR.position + (direccionFrente * distanciaAlJugador);

            Vector3 direccionDerecha = camaraVR.right;
            direccionDerecha.y = 0;
            direccionDerecha.Normalize();

            if (panel != seccionListaTareas && panel != seccionAjustes)
            {
                posicionFinal -= direccionDerecha * 1.4f;
            }

            posicionFinal.y = alturaFijaSuelo;

            panel.transform.position = posicionFinal;

            Vector3 posicionObjetivoMiras = new Vector3(camaraVR.position.x, panel.transform.position.y, camaraVR.position.z);
            panel.transform.LookAt(posicionObjetivoMiras);
            panel.transform.Rotate(0, 180, 0);
        }
    }
}