using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    // Variables de déplacement horizontal
    [Header("Variables de déplacement horizontal")]
    public float speed = 5f;
    public Vector2 movement;
    float moveHorizontal;

    // Variables de saut
    [Header("Variables de saut")]
    public float jumpForce = 10f;
    public float groundRadius;
    public bool isGrounded = false;
    public Transform groundCheck; 
    public LayerMask groundLayer;
    private Vector2 groundCheckOffset;

    // Variables de friction des pentes
    [Header("Variables de friction des pentes")]
    public float penteFriction;
    public float rayDistance;

    private GameMaster gm;
    private Rigidbody2D rb;

    // Méthode Start : récupère les références des composants et initialise la position de départ
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundCheckOffset = groundCheck.position - transform.position;

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }

    // Méthode Update : met à jour le jeu à chaque frame
    private void Update() 
    {
        // Appel des fonctions de saut et de détection du sol
        GroundCheck();
        PlayerJump();
        Move();
        if (transform.position.y < -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    // Méthode FixedUpdate : gère le mouvement de la balle en utilisant la physique du corps rigide 2D (RigidBody2D)
    private void FixedUpdate()
    {
        //Appel de la fonction Move
        Move();
    }

    // Méthode Move : gère le déplacement horizontal et la friction des pentes
    public void Move()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        float horizontalVelocity = moveHorizontal * speed * Time.fixedDeltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        if (hit.collider != null && Vector2.Angle(hit.normal, Vector2.up) > 0 && Vector2.Angle(hit.normal, Vector2.up) < 90)
        {
            float slopeModifier = Mathf.Lerp(1f, penteFriction, Mathf.Abs(Vector2.Angle(hit.normal, Vector2.up) / 90f));
            horizontalVelocity *= slopeModifier;
        }

        movement = new Vector2(horizontalVelocity, rb.velocity.y);
        rb.velocity = movement;

        groundCheck.position = (Vector2)transform.position + groundCheckOffset;
    }

    // Méthode PlayerJump : gère le saut de la balle
    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    // Méthode GroundCheck : détecte si la balle est au sol
    private void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
    }
    

    // Méthode OnDrawGizmosSelected : dessine les gizmos pour le sol et les pentes dans l'éditeur
    // PS : Tu verra quand tu sélectionne la balle dans la scène et que tu zoom, tu verra un petit cercle vert et un trait rouge.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
