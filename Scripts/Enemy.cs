using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public float dieTime;
    public float timeToDie;
    private Animator anim;
    private Transform Player;
    public Transform shotPoint;
    public GameObject bullet;
    public float offset;
    private float target;
    public float left;
    private Vector3 point;
    private Vector3 direction;
    private Rigidbody2D rb;
    int count;
    int lucky;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (health <= 0)
        {
            anim.SetBool("die", true);

            if (timeToDie <= 0)
            {
                anim.SetBool("die", false);
                timeToDie = dieTime;
                Destroy(gameObject);
            }

            else
            {
                timeToDie -= Time.deltaTime;
            }
        }

        if (transform.position.x > Player.transform.position.x)
        {
            if (transform.position.x - Player.transform.position.x > 330)
            {
                anim.SetBool("gun", false);
                direction = (transform.position - Player.transform.position);

                point.x = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                rb.velocity = new Vector2(direction.x, direction.y) / point.x * speed * left;
            }

            else if (transform.position.x - Player.transform.position.x <= 330)
            {
                anim.SetBool("gun", true);
                rb.velocity = Vector2.zero;

                if (timeBtwShots <= 0)
                {
                    count = Random.Range(0, 2);
                    Vector3 difference = Player.transform.position - transform.position;
                    float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                    if (count == 0 || lucky == 2)
                    {
                        lucky = 0;
                        count = Random.Range(0, 2);
                        if (count == 0) { target = 0f; }
                        else { target = -2f; }
                        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset + target);
                    }

                    else if (count == 1 && lucky < 2)
                    {
                        lucky++;
                        target = 4f;
                        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset + target);
                    }

                    Instantiate(bullet, shotPoint.position, shotPoint.transform.rotation);
                    timeBtwShots = startTimeBtwShots;
                }

                else
                {
                    timeBtwShots -= Time.deltaTime;
                    target = 0f;
                }
            }
        }

        else if(transform.position.x < Player.transform.position.x)
        {
            if (Player.transform.position.x - transform.position.x  > 330)
            {
                anim.SetBool("gun", false);
                direction = (Player.transform.position - transform.position);

                point.x = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                rb.velocity = new Vector2(direction.x, direction.y) / point.x * speed;
            }

            else if (Player.transform.position.x - transform.position.x <= 330)
            {
                anim.SetBool("gun", true);
                rb.velocity = Vector2.zero;

                if (timeBtwShots <= 0)
                {
                    count = Random.Range(0, 2);
                    Vector3 difference = Player.transform.position - transform.position;
                    float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                    if (count == 0 || lucky == 2)
                    {
                        lucky = 0;
                        count = Random.Range(0, 2);
                        if (count == 0) { target = 0f; }
                        else { target = -2f; }
                        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset + target);
                    }

                    else if (count == 1 && lucky < 2)
                    {
                        lucky ++;
                        target = 4f;
                        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset + target);
                    }

                    Instantiate(bullet, shotPoint.position, shotPoint.transform.rotation);
                    timeBtwShots = startTimeBtwShots;
                }
                
                else
                {
                    timeBtwShots -= Time.deltaTime;
                    target = 0f;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
