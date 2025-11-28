using UnityEngine;

public class movEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo; // El jugador

    [Header("Ajustes de movimiento")]
    public float velocidad = 2f;
    public float rangoDeteccion = 8f;
    public float rangoPerdida = 10f;
    public float fuerzaEmpuje = 5f; // Fuerza del empuje al recibir golpe

    [Header("Sistema de vida")]
    public int vidaMaxima = 3;
    private int vidaActual;
    private bool estaMuerto = false;
    private bool siguiendo = false;
    private bool enMovimiento;

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
	
	    // Si no tiene objetivo asignado en el Inspector, busca al jugador
	    if (objetivo == null)
	        objetivo = GameObject.FindGameObjectWithTag("personaje")?.transform;
    }

    void Update()
    {
        if (estaMuerto || objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (!siguiendo && distancia <= rangoDeteccion)
        {
            siguiendo = true;
            enMovimiento = true;
        }
        else if (siguiendo && distancia > rangoPerdida)
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

        if (direccion.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direccion.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaMuerto) return;

        if (collision.CompareTag("Espada"))
        {
            Vector2 direccionEmpuje = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
            RecibirDaño(1);
        }
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual <= 0)
            Muerte();
    }

    private void Muerte()
    {
        estaMuerto = true;
        enMovimiento = false;
        siguiendo = false;

        animator.SetBool("muerto", true);
        animator.SetBool("enMovimiento", false);

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        col.enabled = false;
    }

    // 🔹 Llamar cuando el jugador muere
   public void Reiniciar(Transform nuevoObjetivo = null)
	{
	    transform.position = posicionInicial;
	    vidaActual = vidaMaxima;
	    estaMuerto = false;
	    enMovimiento = false;
	    siguiendo = false;
	
	    // 🔹 Reactivar físicas
	    rb.isKinematic = false;
	    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	    rb.linearVelocity = Vector2.zero;
	
	    // 🔹 Reactivar colisión
	    col.enabled = true;
	
	    // 🔹 Resetear animaciones
	    animator.Rebind();
	    animator.Update(0f);
	    animator.SetBool("muerto", false);
	    animator.SetBool("enMovimiento", false);
	
	    // 🔹 Asignar de nuevo el objetivo (por si se perdió)
	    if (nuevoObjetivo != null)
	        objetivo = nuevoObjetivo;
	}


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoPerdida);
    }
}
