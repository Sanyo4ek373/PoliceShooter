using System.Collections;
using UnityEngine;

public abstract class Gunner : MonoBehaviour
{
  [SerializeField] private int health;
  [SerializeField] private float moveSpeed, timeBeforeDie;

  private IEnumerator timeToDie;

  private Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
    timeToDie = DestroyObject(timeBeforeDie);
  }

  public void TakeDamage(int damage)
  {
    health -= damage;
  }

  private void CheckedIsDied(int health)
  {
    if (health <= 0) OnDied();
  }

  private void OnDied()
  {
    animator.SetBool("die", true);
    StartCoroutine(timeToDie);
  }

  private IEnumerator DestroyObject(float waitTime)
  {
    yield return new WaitForSeconds(waitTime);
    animator.SetBool("die", false);
    Destroy(this.gameObject);
  }
}
