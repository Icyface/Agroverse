using UnityEngine;

public class HUDSuaveSeguimiento : MonoBehaviour
{
    [Header("Configuración de Posición (Abajo a la Derecha)")]
    public Vector3 offsetHUD = new Vector3(0.5f, -0.4f, 1.5f);
    public float velocidadSeguimiento = 5.0f;
    public float distanciaMaximaTeleport = 3.0f;

    private void Update()
    {
        Transform camaraVR = Camera.main != null ? Camera.main.transform : null;

        if (camaraVR != null)
        {
            Vector3 forwardPlano = camaraVR.forward;
            forwardPlano.y = 0;
            forwardPlano.Normalize();

            Vector3 rightPlano = camaraVR.right;
            rightPlano.y = 0;
            rightPlano.Normalize();

            Vector3 posicionObjetivo = camaraVR.position
                + (forwardPlano * offsetHUD.z)
                + (rightPlano * offsetHUD.x)
                + (Vector3.up * offsetHUD.y);

            float distanciaActual = Vector3.Distance(transform.position, posicionObjetivo);
            if (distanciaActual > distanciaMaximaTeleport)
            {
                transform.position = posicionObjetivo;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * velocidadSeguimiento);
            }

            Vector3 direccionHaciaCamara = camaraVR.position - transform.position;
            direccionHaciaCamara.y = 0;

            if (direccionHaciaCamara != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(-direccionHaciaCamara);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadSeguimiento);
            }
        }
    }
}