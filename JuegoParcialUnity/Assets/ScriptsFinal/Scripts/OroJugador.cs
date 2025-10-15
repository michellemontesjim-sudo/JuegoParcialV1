using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OroJugador : MonoBehaviour
{
    public int oroActual = 0;

    public void AgregarOro(int cantidad)
    {
        oroActual += cantidad;
        Debug.Log("Oro actual: " + oroActual);
    }
}
