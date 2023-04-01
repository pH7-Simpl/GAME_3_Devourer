using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private int maxEnemyHealth = 100;
    public int enemyHealth = 0;
    private float showHBCooldown = 0f;
    public bool showHB = false;
    public bool hit = false;
    private GameObject healthBar;
    public Animator animator;
    EnemyMovement em;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "windSlash")
        {
            enemyHealth -= 25;
            showHB = true;
            StartCoroutine(HitEffect(0.5f));
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == "lightningDash")
        {
            enemyHealth -= 25;
            showHB = true;
            StartCoroutine(HitEffect(0.5f));
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Player") {
            PlayerStats ps = other.GetComponent<PlayerStats>();
            ps.StartCoroutine(HitEffect(0.5f));
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.name == "fireBreath")
        {
            enemyHealth -= 1;
            showHB = true;
            StartCoroutine(HitEffect(1f));
        }
        if (other.gameObject.layer == 7)
        {
            StartCoroutine(HitEffect(0.1f));
        }
    }

    void Start()
    {
        enemyHealth = maxEnemyHealth;
        healthBar = transform.GetChild(1).gameObject;
        healthBar.SetActive(false);
        em = GetComponent<EnemyMovement>();
    }

    void FixedUpdate()
    {
        if (enemyHealth <= 0)
        {
            enemyHealth = 0;
            StartCoroutine(Die());
        }
        ShowHealthBar();
        if(em.IsGrounded()) {
        Vector2 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
        }
    }
    private void ShowHealthBar()
    {
        if (showHB)
        {
            showHBCooldown = 1f;
            healthBar.SetActive(true);
            showHB = false;
        }
        if (showHBCooldown >= 0f)
        {
            showHBCooldown -= Time.deltaTime;
        }
        else
        {
            healthBar.SetActive(false);
        }
    }

    IEnumerator HitEffect(float duration)
    {
        hit = true;
        yield return new WaitForSeconds(duration);
        hit = false;
    }
    IEnumerator Die()
    {
        animator.SetBool("died", true);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<EnemyMovement>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
