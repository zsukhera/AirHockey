using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallScript : MonoBehaviour
{
    [Header("Puck Interaction")]
    [SerializeField] private float puckHitForce = 15f;
    [SerializeField] private float puckHitRadius = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("puck"))
        {
            ApplyForceToPuck(collision.gameObject);
        }
    }

    private void ApplyForceToPuck(GameObject puck)
    {
        Rigidbody2D puckRb = puck.GetComponent<Rigidbody2D>();

        if (puckRb != null)
        {
            Vector2 direction = (puck.transform.position - transform.position).normalized;
            puckRb.AddForce(direction * puckHitForce, ForceMode2D.Impulse);
        }
    }
}
