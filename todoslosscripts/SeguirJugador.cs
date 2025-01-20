using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    [SerializeField]
    public Transform jugador; 
    [SerializeField]
    public float distancia = 5f; 
    [SerializeField]
    public float altura = 2f; 
    public float sensibilidad = 1f;

    private float anguloHorizontal = 0f;

    private void Start()
    {
        // Pa que no se vea el cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        anguloHorizontal += Input.GetAxis("Mouse X") * sensibilidad;

        // El puto quaternion de euler pa que rote en el angulo horizontal 
        Quaternion rotacion = Quaternion.Euler(0, anguloHorizontal, 0);

        // ponemos la camara detras del jugador
        Vector3 posicion = jugador.position - (rotacion * Vector3.forward * distancia) + Vector3.up * altura;

        // Aplicar la posición y la rotación
        transform.position = posicion;
        transform.LookAt(jugador);

        //La gilipollez esta me ha costado 2 horas de mi vida
    }
}
