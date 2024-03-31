using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chickenControl : MonoBehaviour
{
    public float vitesse;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform[] pointsPatrouille;
    public Transform joueur;
    public int indexPoints;
    public bool isFacingRight = false;
    public bool poulePause;
    public bool joueurDetecte;
    public float tempsDePause;
    public float sphereRadiusPoints;
    public float champVision;
    public float champAttaque;
    public LayerMask playerLayer;
    public float rayonVertical;
    private GameMaster gm;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PoulePatrouille();
        FollowPlayer();
        HitPLayer();
    }

    void PoulePatrouille()
    {
        if (pointsPatrouille.Length == 0)
            return;

            if (poulePause || joueurDetecte)
            {
            return;

            } else
            {
            direction = pointsPatrouille[indexPoints].position - transform.position;
            if (direction.x < 0 && isFacingRight)
            {
                Flip();
            } else if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            rb.velocity = direction.normalized * vitesse * Time.fixedDeltaTime;
            if (rb.velocity.x != 0f)
            {
                animator.SetBool("isRunning", true);
            } else
            {
                animator.SetBool("isRunning", false);
            }

            if (Vector3.Distance(transform.position, pointsPatrouille[indexPoints].position) < 0.1f)
            {
                StartCoroutine(PouleStopCoroutine());
                indexPoints = (indexPoints + 1) % pointsPatrouille.Length;
            }
                
            }

        
    }

    IEnumerator PouleStopCoroutine()
    {
        poulePause = true;
        rb.velocity = Vector3.zero;
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(tempsDePause);
        poulePause = false;
        
    }

        void FollowPlayer()
    {
        RaycastHit2D rayon = Physics2D.Raycast(transform.position, isFacingRight ? Vector2.right : Vector2.left, champVision, playerLayer);

        if (rayon.collider != null && rayon.collider.CompareTag("Player"))
        {
                joueurDetecte = true;
                StopCoroutine(PouleStopCoroutine());
                
                direction = joueur.position - transform.position;
                rb.velocity = direction.normalized * vitesse * Time.fixedDeltaTime;
                animator.SetBool("isRunning", true);

            
        } else
        {
            joueurDetecte = false;
            if (rb.velocity.x == 0f)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    void HitPLayer()
    {
        RaycastHit2D hitRaycast = Physics2D.Raycast(transform.position, isFacingRight ? Vector2.right : Vector2.left, champAttaque, playerLayer);

        if (hitRaycast.collider != null && hitRaycast.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        foreach (Transform point in pointsPatrouille)
        {
            Gizmos.DrawWireSphere(point.position, sphereRadiusPoints);
        }

        for (int i = 0; i < pointsPatrouille.Length - 1; i++)
        {
            Debug.DrawLine(pointsPatrouille[i].position, pointsPatrouille[i + 1].position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (isFacingRight ? Vector3.right : Vector3.left) * champVision);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * rayonVertical);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (isFacingRight ? Vector3.right : Vector3.left) * champAttaque);

    }
}
