using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    float m_fRotationSpeed = 3.0f;
    [SerializeField]
    float m_fMovementSpeed = 3.0f;
    [SerializeField]
    float m_fVerticalBlowbackForce = 3.0f;
    [SerializeField]
    bool isChasingPlayer;
    Rigidbody rb;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        isChasingPlayer = true;
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasingPlayer)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), m_fRotationSpeed * Time.deltaTime);
            rb.AddForce(transform.forward * m_fMovementSpeed);
        }
    }

    public void Blowback(Vector3 blastOrigin, float blowbackForce)
    {
        isChasingPlayer = false;
        rb.useGravity = true;
        Vector3 distToBlast = transform.position - blastOrigin;
        distToBlast.y = m_fVerticalBlowbackForce;
        Vector3 distToBlastNormalized = distToBlast.normalized;
        rb.AddForce(distToBlast * blowbackForce, ForceMode.Impulse);
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bounds") && !isChasingPlayer) 
        {
            // increase score as well
            GameManager.Instance.UpdateEnemyCount();
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.GetComponent<PlayerMovement>().BounceBack(transform.position, 2.25f);
        }
    }
}
