using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Datos de los 4 Héroes")]
    public List<DatosHeroe> datosHeroes = new List<DatosHeroe>();

    [Header("Inventario")]
    public Dictionary<string, int> inventario = new Dictionary<string, int>();
    public int dinero = 0;

    [Header("Estado del Juego")]
    public bool helenaMuerta = false;
    public bool lupusMuerto = false;
    public bool jefeDerrotado = false;

    [Header("Configuración")]
    public bool modoDebug = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InicializarDatos();

        if (modoDebug)
            Debug.Log("[GameManager] Inicializado correctamente");
    }

    private void InicializarDatos()
    {
        datosHeroes.Clear();
        inventario.Clear();
        dinero = 0;

        // HÉROE 1 - Stats exactas del PDF
        datosHeroes.Add(new DatosHeroe
        {
            numeroHeroe = 1,
            nombre = "Héroe 1",
            fuerza = 19,
            resistenciaMaxima = 68f,
            resistenciaActual = 68f,
            vivo = true,
            ataques = new List<AtaqueData>
            {
                new AtaqueData { nombre = "Espada Rápida", dados = new int[] { 10, 4 } }, // 1D10 + 1D4
                new AtaqueData { nombre = "Golpe Poderoso", dados = new int[] { 6, 6 } }  // 2D6
            }
        });

        // HÉROE 2 - Stats exactas del PDF
        datosHeroes.Add(new DatosHeroe
        {
            numeroHeroe = 2,
            nombre = "Héroe 2",
            fuerza = 23,
            resistenciaMaxima = 87f,
            resistenciaActual = 87f,
            vivo = true,
            ataques = new List<AtaqueData>
            {
                new AtaqueData { nombre = "Ataque Básico", dados = new int[] { 10 } },        // 1D10
                new AtaqueData { nombre = "Combo Veloz", dados = new int[] { 10, 4 } },       // 1D10 + 1D4
                new AtaqueData { nombre = "Asalto Final", dados = new int[] { 10, 4, 4 } }    // 1D10 + 2D4
            }
        });

        // HÉROE 3 - Stats exactas del PDF
        datosHeroes.Add(new DatosHeroe
        {
            numeroHeroe = 3,
            nombre = "Héroe 3",
            fuerza = 17,
            resistenciaMaxima = 50f,
            resistenciaActual = 50f,
            vivo = true,
            ataques = new List<AtaqueData>
            {
                new AtaqueData { nombre = "Flecha Certera", dados = new int[] { 10, 6 } } // 1D10 + 1D6
            }
        });

        // HÉROE 4 - Stats exactas del PDF
        datosHeroes.Add(new DatosHeroe
        {
            numeroHeroe = 4,
            nombre = "Héroe 4",
            fuerza = 16,
            resistenciaMaxima = 38f,
            resistenciaActual = 38f,
            vivo = true,
            ataques = new List<AtaqueData>
            {
                new AtaqueData { nombre = "Hechizo Arcano", dados = new int[] { 6, 8 } } // 1D6 + 1D8
            }
        });

        // Inicializar inventario base
        inventario.Add("Pociones", 3);
        inventario.Add("Cofres", 0);

        if (modoDebug)
            Debug.Log($"[GameManager] {datosHeroes.Count} héroes inicializados");
    }

    // ============ MÉTODOS PÚBLICOS ============

    public DatosHeroe ObtenerDatosHeroe(int numero)
    {
        return datosHeroes.Find(h => h.numeroHeroe == numero);
    }

    public void ActualizarVidaHeroe(int numero, float nuevaVida)
    {
        DatosHeroe heroe = ObtenerDatosHeroe(numero);
        if (heroe != null)
        {
            heroe.resistenciaActual = Mathf.Clamp(nuevaVida, 0, heroe.resistenciaMaxima);

            if (heroe.resistenciaActual <= 0 && heroe.vivo)
            {
                heroe.vivo = false;
                if (modoDebug) Debug.Log($"[Héroe {numero}] Ha muerto");
            }
        }
    }

    public void AgregarObjeto(string nombre, int cantidad)
    {
        if (inventario.ContainsKey(nombre))
            inventario[nombre] += cantidad;
        else
            inventario.Add(nombre, cantidad);
    }

    public bool UsarPocion(int numeroHeroe)
    {
        if (inventario.ContainsKey("Pociones") && inventario["Pociones"] > 0)
        {
            DatosHeroe heroe = ObtenerDatosHeroe(numeroHeroe);
            if (heroe != null && heroe.vivo)
            {
                inventario["Pociones"]--;
                heroe.resistenciaActual = Mathf.Min(heroe.resistenciaActual + 25, heroe.resistenciaMaxima);
                return true;
            }
        }
        return false;
    }

    public void AgregarDinero(int cantidad)
    {
        dinero += cantidad;
    }

    public bool TodosHeroesMuertos()
    {
        return datosHeroes.TrueForAll(h => !h.vivo);
    }

    public void ReiniciarEstadoBoss()
    {
        helenaMuerta = false;
        lupusMuerto = false;
        jefeDerrotado = false;
    }
}

[System.Serializable]
public class DatosHeroe
{
    public int numeroHeroe;
    public string nombre;
    public int fuerza;
    public float resistenciaMaxima;
    public float resistenciaActual;
    public bool vivo;
    public List<AtaqueData> ataques;

    public float PorcentajeVida => resistenciaMaxima > 0 ? resistenciaActual / resistenciaMaxima : 0f;
}

[System.Serializable]
public class AtaqueData
{
    public string nombre;
    public int[] dados;

    public string DescripcionDados()
    {
        if (dados == null || dados.Length == 0) return "";

        string desc = "";
        for (int i = 0; i < dados.Length; i++)
        {
            desc += $"1D{dados[i]}";
            if (i < dados.Length - 1) desc += " + ";
        }
        return desc;
    }
}