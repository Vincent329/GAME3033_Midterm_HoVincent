using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public bool hitEnemy;
    [SerializeField] float m_fExplosiveForce;
    [SerializeField] Renderer renderComp;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        renderComp = GetComponent<Renderer>();
        rb.isKinematic = true;
        hitEnemy = true;
    }

    public void ToggleHitEnemy()
    {
        hitEnemy = false;
        renderComp.material.color = Color.blue;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision other)
    {
        // check for rigid body objects or enemy objects
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            if (hitEnemy && other.gameObject.GetComponent<EnemyAI>() != null)
            {
                other.gameObject.GetComponent<EnemyAI>().Blowback(transform.position, m_fExplosiveForce);
                
            }
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            { 

                other.gameObject.GetComponent<PlayerMovement>().BounceBack(transform.position, m_fExplosiveForce);
            }
            
        }
    }
}
