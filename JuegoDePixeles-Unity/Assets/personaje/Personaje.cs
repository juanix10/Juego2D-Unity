using UnityEngine;
using UnityEngine.InputSystem;

public class personaje : MonoBehaviour
{
    public Rigidbody2D rigibody2d;
    public float Velocidad = 5f;
    public float fuerzaSalto = 8f;
    private Vector2 movimiento;
    private int jump = 2;
    public Animator animator;

    private Vector3 posicionInicial;
    public BarraVida barraVida;
    private bool atacando;
    private bool recibiendoDanio = false;
    public float fuerzaRebote = 5f;

    private bool estaMuerto = false;

    void Start()
    {
        rigibody2d = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (estaMuerto) return; // Evita movimiento si está muerto
        if (recibiendoDanio) return;

        movimiento = Vector2.zero;

        if (Keyboard.current.aKey.isPressed)
            movimiento.x = -1;
        else if (Keyboard.current.dKey.isPressed)
            movimiento.x = 1;

        animator.SetFloat("movement", Mathf.Abs(movimiento.x));
        animator.SetBool("Atacando", atacando);

        if (movimiento.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movimiento.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jump > 0)
        {
            rigibody2d.linearVelocity = new Vector2(rigibody2d.linearVelocity.x, fuerzaSalto);
            jump--;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Atacando();
        }
    }

    void FixedUpdate()
    {
        if (!recibiendoDanio && !estaMuerto)
            rigibody2d.linearVelocity = new Vector2(movimiento.x * Velocidad, rigibody2d.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            jump = 2;
        }

        //  Muerte instantánea por picos
        if (collision.gameObject.CompareTag("muerte"))
        {
            Morir();
        }
    }

    public void Atacando()
    {
        if (!atacando)
        {
            atacando = true;
            animator.SetTrigger("Atacando");
            Invoke(nameof(DesactivaAtaque), 0.4f);
        }
    }

    public void DesactivaAtaque()
    {
        atacando = false;
    }

    // === Recibir daño con animación y rebote ===
    public void RecibeDanio(Vector2 origenDanio, float cantidad)
    {
        if (recibiendoDanio || estaMuerto)
            return;

        recibiendoDanio = true;
        animator.SetTrigger("recibeDanio");

        Vector2 direccionRebote = new Vector2(transform.position.x - origenDanio.x, 1).normalized;
        rigibody2d.AddForce(direccionRebote * fuerzaRebote, ForceMode2D.Impulse);

        if (barraVida != null)
        {
            barraVida.RestarVida(cantidad);

            // Si la barra llega a cero
            if (barraVida.EstaMuerto())
            {
                Morir();
                return;
            }
        }

        Invoke(nameof(DesactivaDanio), 0.5f);
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rigibody2d.linearVelocity = Vector2.zero;
        animator.ResetTrigger("recibeDanio");
        animator.SetFloat("movement", 0f);
    }

    // === NUEVO: Animación y reinicio de muerte ===
    private void Morir()
{
    if (estaMuerto) return;
    estaMuerto = true;
    recibiendoDanio = false;
    movimiento = Vector2.zero;

    // 🧍‍♂️ Activar animación de muerte
    animator.SetTrigger("muere");

    // Desactivar físicas y movimiento
    rigibody2d.linearVelocity = Vector2.zero;
    rigibody2d.isKinematic = true;
    GetComponent<Collider2D>().enabled = false;

    // Esperar animación antes de reaparecer
    Invoke(nameof(ReiniciarPosicion), 2f); // Ajusta el tiempo según la duración de la animación
}


	public void ReiniciarPosicion()
	{
	    estaMuerto = false;
	    recibiendoDanio = false;
	    transform.position = posicionInicial;
	    rigibody2d.isKinematic = false;
	    GetComponent<Collider2D>().enabled = true;
	    rigibody2d.linearVelocity = Vector2.zero;
	    jump = 2;
	    barraVida.RestaurarVida();
	    animator.ResetTrigger("muere");
	    animator.Play("idel"); // Asegúrate de usar el nombre correcto de tu animación idle
		
	    if (GameManager.instancia != null)
    	GameManager.instancia.ReiniciarJuego();
	    
	    // 🔹 Reinicia enemigos manualmente
		movEnemigo[] enemigos = FindObjectsOfType<movEnemigo>();
		foreach (movEnemigo enemigo in enemigos)
		{
		    enemigo.Reiniciar(transform); // <-- le pasamos el jugador como nuevo objetivo
		}
	}
	
	void ReiniciarJuego()
{
    // Reinicia al jugador aquí...
    transform.position = posicionInicial;

    // 🔹 Reinicia enemigos
    movEnemigo[] enemigos = FindObjectsOfType<movEnemigo>();
    foreach (movEnemigo enemigo in enemigos)
    {
        enemigo.Reiniciar();
    }
}


    
}
