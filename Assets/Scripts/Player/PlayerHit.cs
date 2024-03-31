using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [Header("Variables de détection des ennemis")]
    public Transform player; // Référence à la balle
    public float detectionRadius = 5f; // Rayon de détection des ennemis
    public LayerMask enemyLayer; // Layer des ennemis

    private void Update()
    {
        // Déplacement de l'objet de détection vers la position de la balle
        transform.position = player.position;
        
        // Détection des ennemis dans le rayon de détection
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        // Si des ennemis sont détectés, vous pouvez implémenter une logique ici
        foreach (Collider2D enemy in hitEnemies)
        {
            // Faites quelque chose avec chaque ennemi détecté, par exemple :
            Debug.Log("Ennemi détecté : " + enemy.name);
            // Ajoutez ici votre logique de gestion des ennemis détectés
        }
    }

    // Cette méthode dessine le rayon de détection dans l'éditeur Unity pour faciliter le réglage
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
