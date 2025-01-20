using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    Rigidbody rb;
    float masa;
    [SerializeField] float aceleracion = 15f;
    [SerializeField] Transform camara;
    Vector3 vectorFuerza;
    float porcentajeRealentizacion = 0f;
    bool enSuelo = false;
    private float cooldownSalto = 5f;
    private float tiempoInicial = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        masa = rb.mass;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo && Time.time > tiempoInicial + cooldownSalto)
        {
            tiempoInicial = Time.time;
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
        vectorFuerza = Vector3.zero;
        Vector3 direccionMovimiento = CalcularDireccionMovimiento();
        vectorFuerza = direccionMovimiento * masa * aceleracion;
        if (enSuelo)
        {
            Vector3 velocidadActual = rb.GetPointVelocity(rb.position);
            Debug.Log("MAgnitud: " + velocidadActual.magnitude);
            if (velocidadActual.magnitude > 3.5f)
            {
                vectorFuerza *= (100 - porcentajeRealentizacion) / 100;
                rb.AddForce(-velocidadActual * (100 - porcentajeRealentizacion) / 100);
            } else
            {
                Debug.Log("Velocidad Antes: " + rb.GetPointVelocity(rb.position));
                velocidadActual = rb.GetPointVelocity(rb.position);
                rb.AddForce(-velocidadActual);
               
                Debug.Log("Velocidad Despues: " + rb.GetPointVelocity(rb.position));
            }
        }
        else
            vectorFuerza *= 0.5f;





        

        
        
        rb.AddForce(vectorFuerza, ForceMode.Force);
       
        Debug.Log("VectorFuerza: " + vectorFuerza);  
    }
    Vector3 CalcularDireccionMovimiento()
    {
        //YA FUNCIONA JODER
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
    











    private void OnCollisionStay(Collision collision)
    {
        SueloInfo suelo = collision.gameObject.GetComponent<SueloInfo>();
        if (suelo != null) 
        { 
            porcentajeRealentizacion = suelo.CoeficienteRozamiento;
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
            porcentajeRealentizacion = 0f;
            enSuelo = false;
            Debug.Log("No en suelo");
        }
    }
}
