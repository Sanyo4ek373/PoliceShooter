using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector3 direction;
    private Vector3 point;
    private int dist;
    public float moveSpeed = 10f;
    private Animator anim;
    private bool view = true;
    private bool collision = false;
    public GameObject bullet;
    public Transform shotPoint;
    public Transform gunPointTop;
    public Transform gunPointTopDown;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public float dieTime;
    public float timeToDie;
    private float timeDialog;
    public float startTimeDialog;
    public bool gun = false;
    public float offset;
    public bool targetOn;
    Transform target;
    public LayerMask whatIsSolid;
    public float distance;
    public PlayerPosition pos;
    public Animator DialogButton;
    public int health;
    public bool spawn = false;
    public Transform SpawnerR;
    public Transform SpawnerL;
    public Transform SpawnerR1;
    public GameObject EnemyR;
    public GameObject EnemyR1;
    public GameObject EnemyL;
    public int clone = 0;
   
    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void FixedUpdate()
    {
        if (health <= 0 || collision)
        {
            anim.SetBool("die", true);
            if (timeToDie <= 0)
            {
                collision = false;
                anim.SetBool("die", false);
                timeToDie = dieTime;
                SceneManager.LoadScene(4);
            }
            else
            {
                timeToDie -= Time.deltaTime;
            }

        }

        if (timeBtwShots <= 0)
        {
            if (Input.touchCount > 0 && gun)     
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (transform.position - touchPosition);

                if (touchPosition.y <= 40 && touchPosition.y >= -60)
                {
                    if (direction.x >= 100 && !view || direction.x <= -100 && view)
                    {
                        Vector3 difference = Camera.main.ScreenToWorldPoint(touch.position) - shotPoint.transform.position;
                        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                        shotPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
                        Instantiate(bullet, shotPoint.position, shotPoint.transform.rotation);
                        timeBtwShots = startTimeBtwShots;
                    }
                }
            }
        }

        else
        {
            timeBtwShots -= Time.deltaTime;
            anim.SetBool("gun", false);
        }

        if (targetOn == false)
        {
            if (Input.touchCount > 0 && gun == false)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (touchPosition - transform.position);

                if (touchPosition.y >= -100 && touchPosition.y <= 100)
                {
                    point.x = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                    rb.velocity = new Vector2(direction.x, direction.y) / point.x * moveSpeed;       //rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
                }

                if (touch.phase == TouchPhase.Ended)
                    rb.velocity = Vector2.zero;

            }
        }

        else if (targetOn)
        {
            if (Input.GetMouseButtonDown(0) && gun == false )
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                direction = (mousePos - transform.position);
                point.x = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                rb.velocity = new Vector2(direction.x, direction.y) / point.x * moveSpeed; 
            }
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Finish"))
            {
                Destroy(target.gameObject);
                rb.velocity = Vector2.zero;
            }

        }

        if (target && rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            Destroy(target.gameObject);
        }

        if (rb.velocity.x != 0 && rb.velocity.y != 0)
        {
            anim.SetBool("run", true);
        }

        else if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetBool("run", false);
        }

        if (gun == true)
        {
            anim.SetBool("gun", true);
        }

        else if (gun == false)
        {
            anim.SetBool("gun", false);
        }

        if (view == false && touchPosition.x > transform.position.x && touchPosition.y > -100 && touchPosition.y <= 100 && gun == false)
        {
            Flip();
        }

        else if (view == true && touchPosition.x < transform.position.x && touchPosition.y > -100 && touchPosition.y <= 100 && gun == false)
        {
            Flip();
        }

        if (spawn && clone == 0)
        {
        	EnemyR = Instantiate(EnemyR, SpawnerR.position, Quaternion.identity);
        	EnemyL = Instantiate(EnemyL, SpawnerL.position, Quaternion.identity);
        	EnemyR1 = Instantiate(EnemyR1, SpawnerR1.position, Quaternion.identity);
            clone = 1;
        }

        if (EnemyL == null && EnemyR == null && EnemyR1 == null)
        {
        	Debug.Log("Working!");
            SceneManager.LoadScene(3);
        }
    }

    void Flip()
    {
        view = !view;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            collision = true;
        }
        
    }

    public void Button()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            gun = !gun;
            rb.velocity = Vector2.zero;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
            DialogButton.SetInteger("dialog", 1);
            spawn = true;
        }

        if (other.gameObject.tag == "item")
        {
            Destroy(other.gameObject);
            Debug.Log("Apple");
        }
    }
}
