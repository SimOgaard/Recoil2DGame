using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float lifeTime = 0.75f;

    [SerializeField]
    private LayerMask whatIsSolid;
    [SerializeField]
    private float distance;

    private void Start()
    {
        Invoke("Destroy", lifeTime);        
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Destroyable Ground"))
            {
                hitInfo.collider.GetComponent<Destroyable>().destroyWall();
            }
            Destroy();
        }

        transform.position += transform.right.normalized * speed * Time.deltaTime;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
