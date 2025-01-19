using UnityEngine;

public class SueloInfo : MonoBehaviour
{

    [SerializeField] float coeficienteRozamiento = 0.9f;

    public float CoeficienteRozamiento
    {
        get { return coeficienteRozamiento; }
    }
}
