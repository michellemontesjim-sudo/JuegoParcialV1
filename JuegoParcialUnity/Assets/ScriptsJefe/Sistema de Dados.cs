using UnityEngine;

public static class SistemaDados
{
    // ============ TIRADA DE ÉXITO (1D10) ============
    public static ResultadoExito RealizarTiradaExito()
    {
        int dado = Random.Range(0, 10); // 0-9
        ResultadoExito resultado = new ResultadoExito { valor = dado };

        // 1,2,3 = PIFIA; 4-9 = ÉXITO
        if (dado >= 1 && dado <= 3)
        {
            resultado.esPifia = true;
            resultado.mensaje = $"¡PIFIA! Sacó {dado}";
        }
        else
        {
            resultado.esExito = true;
            resultado.mensaje = $"ÉXITO! Sacó {dado}";
        }

        return resultado;
    }

    // ============ TIRADA DE DAÑO (2D10) ============
    public static ResultadoDanio RealizarTiradaDanio(int fuerzaPersonaje)
    {
        int dado1 = Random.Range(0, 10);
        int dado2 = Random.Range(0, 10);
        int valorTotal = int.Parse(dado1.ToString() + dado2.ToString());

        ResultadoDanio resultado = new ResultadoDanio
        {
            dado1 = dado1,
            dado2 = dado2,
            valorTotal = valorTotal
        };

        if (valorTotal > 70 && valorTotal <= 99)
        {
            resultado.tipo = TipoResultadoDanio.Pifia;
            resultado.danioFinal = 0;
            resultado.mensaje = $"PIFIA: {valorTotal} > 70";
        }
        else if (valorTotal <= 70 && valorTotal >= fuerzaPersonaje)
        {
            resultado.tipo = TipoResultadoDanio.Exito;
            resultado.mensaje = $"ÉXITO: {valorTotal} >= Fuerza({fuerzaPersonaje})";
        }
        else if (valorTotal < fuerzaPersonaje)
        {
            resultado.tipo = TipoResultadoDanio.Debil;
            resultado.danioFinal = 0;
            resultado.mensaje = $"DÉBIL: {valorTotal} < Fuerza({fuerzaPersonaje})";
        }

        return resultado;
    }

    // ============ CALCULAR DAÑO DE ATAQUE ============
    public static int CalcularDanioAtaque(int[] dadosAtaque)
    {
        int danioTotal = 0;
        foreach (int dado in dadosAtaque)
        {
            danioTotal += Random.Range(1, dado + 1);
        }
        return danioTotal;
    }

    // ============ TIRADA ESPECIAL LUPUS ============
    public static ResultadoLupus RealizarTiradaLupus(bool helenaMuerta)
    {
        int dado = Random.Range(0, 10);
        ResultadoLupus resultado = new ResultadoLupus { valorDado = dado };

        if (dado > 7 && dado <= 9) // 8-9 = PIFIA
        {
            resultado.esPifia = true;
            resultado.danio = 0;
            resultado.mensaje = helenaMuerta ? "Lupus falla ataque" : "Helena no responde";
        }
        else if (dado >= 0 && dado < 7) // 0-6 = ÉXITO
        {
            resultado.esExito = true;
            if (helenaMuerta)
            {
                resultado.danio = Random.Range(1, 9); // 1D8
                resultado.mensaje = $"Lupus ataca: {resultado.danio} daño";
            }
            else
            {
                resultado.danio = Random.Range(1, 5); // 1D4
                resultado.atacaHelena = true;
                resultado.mensaje = $"Helena ayuda: {resultado.danio} daño";
            }
        }

        return resultado;
    }

    // ============ MÉTODO AUXILIAR ============
    public static int LanzarDado(int caras)
    {
        return Random.Range(1, caras + 1);
    }
}

// ============ CLASES DE RESULTADO ============

public class ResultadoExito
{
    public int valor;
    public bool esExito;
    public bool esPifia;
    public string mensaje;
}

public class ResultadoDanio
{
    public int dado1;
    public int dado2;
    public int valorTotal;
    public TipoResultadoDanio tipo;
    public int danioFinal;
    public string mensaje;
}

public enum TipoResultadoDanio
{
    Pifia,
    Exito,
    Debil
}

public class ResultadoLupus
{
    public int valorDado;
    public bool esExito;
    public bool esPifia;
    public bool atacaHelena;
    public int danio;
    public string mensaje;
}