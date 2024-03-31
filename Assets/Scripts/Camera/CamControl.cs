using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Transform cible;
    public GameObject joueur;
    public float lissage = 0.1f;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    void Start()
    {
        joueur = GameObject.Find("Player");
        cible = joueur.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (cible != null)
        {
            Vector3 PositionCible = new Vector3(cible.position.x, cible.position.y, transform.position.z);

            PositionCible.x = Mathf.Clamp(PositionCible.x, minPosition.x, maxPosition.x);
            PositionCible.y = Mathf.Clamp(PositionCible.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, PositionCible, lissage);
        }
    }
}
