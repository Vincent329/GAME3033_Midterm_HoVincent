using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public bool hitEnemy;
    [SerializeField] float m_fExplosiveForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hitEnemy = true;
    }

    public void ToggleHitEnemy()
    {
        hitEnemy = false;
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

                Debug.Log("Push enemy");
            }
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                other.gameObject.GetComponent<PlayerMovement>().BounceBack(transform.position, m_fExplosiveForce);
            }
            
        }
    }
}
