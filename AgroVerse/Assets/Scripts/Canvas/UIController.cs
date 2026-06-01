using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Paneles Principales")]
    public GameObject panelMenuPrincipal;
    public GameObject panelListaTareas;
    public GameObject panelAjustes;

    [Header("Paneles de Tareas Individuales")]
    public GameObject panelRecogerHuevos;
    public GameObject panelLimpiarCerdos;
    public GameObject panelAlimentarAnimales;
    public GameObject panelOrdeñarVaca;

    [Header("Configuración VR")]
    [Tooltip("Distancia horizontal a la que aparecerá el cartel frente al jugador")]
    public float distanciaAlJugador = 2.0f;

    [Tooltip("Altura fija del cartel respecto al suelo del mapa (Y = 0). Intenta valores entre 1.1 y 1.4")]
    public float alturaFijaSuelo = 1.3f;

    public void CerrarPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void ToggleAjustes()
    {
        if (panelAjustes != null)
        {
            if (panelAjustes.activeSelf)
            {
                panelAjustes.SetActive(false);
            }
            else
            {
                if (panelMenuPrincipal) panelMenuPrincipal.SetActive(false);
                OcultarTodasLasTareas();

                PosicionarFrenteAlJugador(panelAjustes);
                panelAjustes.SetActive(true);
            }
        }
    }

    public void ToggleListaTareas()
    {
        if (panelListaTareas != null)
        {
            if (panelListaTareas.activeSelf)
            {
                panelListaTareas.SetActive(false);
                OcultarTodasLasTareas();
            }
            else
            {
                if (panelMenuPrincipal) panelMenuPrincipal.SetActive(false);

                PosicionarFrenteAlJugador(panelListaTareas);
                panelListaTareas.SetActive(true);
            }
        }
    }

    public void VolverAlMenu()
    {
        if (panelAjustes) panelAjustes.SetActive(false);
        if (panelListaTareas) panelListaTareas.SetActive(false);
        OcultarTodasLasTareas();

        if (panelMenuPrincipal)
        {
            PosicionarFrenteAlJugador(panelMenuPrincipal);
            panelMenuPrincipal.SetActive(true);
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

            if (panel != panelListaTareas && panel != panelAjustes && panel != panelMenuPrincipal)
            {
                posicionFinal -= direccionDerecha * 1.2f;
            }
            else if (panel == panelAjustes && panelListaTareas != null && panelListaTareas.activeSelf)
            {
                posicionFinal += direccionDerecha * 1.2f;
            }

            posicionFinal.y = alturaFijaSuelo;

            panel.transform.position = posicionFinal;

            Vector3 posicionObjetivoMiras = new Vector3(camaraVR.position.x, panel.transform.position.y, camaraVR.position.z);
            panel.transform.LookAt(posicionObjetivoMiras);
            panel.transform.Rotate(0, 180, 0);
        }
    }
}