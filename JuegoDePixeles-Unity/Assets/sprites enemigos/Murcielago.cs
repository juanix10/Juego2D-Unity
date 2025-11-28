using UnityEngine;

public class Murcielago : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo; // El jugador

    [Header("Movimiento")]
    public float velocidad = 3f;
    public float rangoDeteccion = 8f;
    public float rangoPerdida = 10f;
    private bool siguiendo = false;
    private bool enMovimiento = false;

    [Header("Vida")]
    public int vidaMaxima = 2;
    private int vidaActual;
    private bool estaMuerto = false;
    public float fuerzaEmpuje = 5f;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 posicionInicial;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        vidaActual = vidaMaxima;
        posicionInicial = transform.position;

        if (objetivo == null)
            objetivo = GameObject.FindGameObjectWithTag("personaje")?.transform;

        rb.gravityScale = 0f; // No cae
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (estaMuerto || objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (distancia <= rangoDeteccion)
        {
            siguiendo = true;
            enMovimiento = true;
        }
        else if (distancia > rangoPerdida)
        {
            siguiendo = false;
            enMovimiento = false;
        }

        if (siguiendo)
            SeguirJugador();

        animator.SetBool("enMovimiento", enMovimiento);
    }

    void SeguirJugador()
    {
        Vector2 direccion = (objetivo.position - transform.position).normalized;
        transform.position += (Vector3)(direccion * velocidad * Time.deltaTime);

        // Cambiar orientación
        if (direccion.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direccion.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    // 💥 Solo recibe golpes aquí
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (estaMuerto) return;

        if (other.CompareTag("Espada"))
        {
            StartCoroutine(ReaccionAlGolpe(other));
        }
    }

    //  CORUTINA: reacciona al golpe, se empuja y reanuda seguimiento
    private System.Collections.IEnumerator ReaccionAlGolpe(Collider2D golpe)
    {
        // Detiene el movimiento temporalmente
        enMovimiento = false;
        siguiendo = false;

        // Aplica empuje físico
        Vector2 direccionEmpuje = (transform.position - golpe.transform.position).normalized;
        rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);

        // Espera un momento para el efecto del golpe
        yield return new WaitForSeconds(0.2f);

        // Detiene el movimiento físico
        rb.linearVelocity = Vector2.zero;

        // Si sigue vivo, reanuda el seguimiento
        if (!estaMuerto)
        {
            siguiendo = true;
            enMovimiento = true;
        }

        // Aplica el daño después del golpe
        RecibirDanio(1);
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log("Murciélago recibe daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
            Muerte();
    }

    private void Muerte()
    {
        // Desactiva su movimiento y colisiones
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        col.enabled = false;
        estaMuerto = true;
        enMovimiento = false;
        siguiendo = false;

        animator.SetBool("muerto", true);
        animator.SetBool("enMovimiento", false);

        // Solo desactivarlo (así puedes reactivarlo después)
        gameObject.SetActive(false);
    }

    public void Reiniciar(Transform nuevoObjetivo = null)
	{
	    transform.position = posicionInicial;
	    vidaActual = vidaMaxima;
	    estaMuerto = false;
	    enMovimiento = false;
	    siguiendo = false;
	
	    rb.isKinematic = false;
	    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	    rb.linearVelocity = Vector2.zero;
	    col.enabled = true;
	
	    animator.Rebind();
	    animator.Update(0f);
	    animator.SetBool("muerto", false);
	    animator.SetBool("enMovimiento", false);
	
	    // 🔧 Asegurar que siempre tenga el objetivo correcto
	    if (nuevoObjetivo != null)
	        objetivo = nuevoObjetivo;
	    else
	        objetivo = GameObject.FindGameObjectWithTag("personaje")?.transform;
	
	    // 🔧 Reactivar si estaba desactivado
	    if (!gameObject.activeSelf)
	        gameObject.SetActive(true);
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoPerdida);
    }
}
