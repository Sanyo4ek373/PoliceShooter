using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    private Vector3 touchPosition;
    private Vector3 direction;
    private Rigidbody2D rb;
    public bool gun = false;
    public GameObject destroyEffect;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Destroy", lifetime);
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                
            }

            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<Player>().TakeDamage(damage);
            }

            Destroy();
        }
        
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public void Button()
    {
        gun = !gun;
    }

    void Destroy()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}   
