using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    Rigidbody rb;
    float masa;
    [SerializeField] float aceleracion = 15f;
    [SerializeField] Transform camara;
    Vector3 vectorFuerza;
    float coeficienteRozamientoActual = 0f;
    bool enSuelo = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        masa = rb.mass;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GestionarMovimiento();

    }
    void GestionarMovimiento()
    {
        Vector3 direccionMovimiento = CalcularDireccionMovimiento();




        vectorFuerza = direccionMovimiento * masa * aceleracion;
        rb.AddForce(vectorFuerza, ForceMode.Force);
        
        Debug.Log(rb.GetPointVelocity(rb.position));


        GestionarRozamiento();

    }
    Vector3 CalcularDireccionMovimiento()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //LA DIRECCION DE MOVIMIENTO EN FUNCION DE LA CAMARA
        Vector3 forward = camara.forward;
        Vector3 right = camara.right;

        // Asegurarse de que solo se muevan en el plano horizontal
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calcular la dirección deseada
        Vector3 direccionMovimiento = forward * vertical + right * horizontal;
        return direccionMovimiento;
    }
    void GestionarRozamiento()
    {

        Vector3 velocidadActual = rb.GetPointVelocity(rb.position);
        Vector3 fuerzaFrenado = - velocidadActual * coeficienteRozamientoActual;
        rb.AddForce(fuerzaFrenado, ForceMode.Force);

    }











    private void OnCollisionEnter(Collision collision)
    {
        SueloInfo suelo = collision.gameObject.GetComponent<SueloInfo>();
        if (suelo != null) 
        { 
            coeficienteRozamientoActual = suelo.CoeficienteRozamiento;
            enSuelo = true;
            Debug.Log("En suelo");
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        //Cuando deje estar en contacto con el suelo, no tendra coeficiente de rozamiento y 
        SueloInfo suelo = collision.gameObject.GetComponent<SueloInfo>();
        if (suelo != null)
        { 
            coeficienteRozamientoActual = 0f;
            enSuelo = false;
            Debug.Log("No en suelo");
        }
    }
}
