using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Transform target;

    private HPBar hpBar;

    public Animator animator;

    private bool isDead = false;
    private PolygonCollider2D polyCollider;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;


    [SerializeField] private float maxHP;
    private float hp;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] public float attackDamage = 5f;
    [SerializeField] public float collisionDamage = 1f;
    [SerializeField] private float attackDistanceX = 1.1f;
    [SerializeField] private float attackDistanceY = 0.4f;

    [SerializeField] private float hitStaggerTime = 0.2f;

    [SerializeField] private Color hitColor;

    [SerializeField] private int numOfAttack = 1;
    [SerializeField] private int defaultDirectionRight = 1;


    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float lastHitTime;

    private bool isHit = false;
    // private bool isAttacking = false;
    // private float lastAttackTime;

    // [SerializeField] private float attackInterval = 1f;




    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        hpBar = GetComponentInChildren<HPBar>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        hp = maxHP;
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && GameManager.instance.isFreezeMode == false)
        {

            animator.speed = 1;
            if (isHit && Time.time - lastHitTime >= hitStaggerTime)
            {
                // If hit, perform hit animation and stagger
                animator.SetBool("isHit", false);
                isHit = false;
            }

            else if (!isHit)
            {

                if (Mathf.Abs(target.position.x - transform.position.x) < attackDistanceX && Mathf.Abs(target.position.y - transform.position.y) < attackDistanceY)
                {
                    // If target in range, perform attack
                    animator.SetBool("isAttack", true);
                    animator.SetInteger("attackIndex", Random.Range(0, numOfAttack));
                    // isAttacking = true;
                    // lastAttackTime = Time.time;
                }
                else
                {
                    animator.SetBool("isAttack", false);
                    // Otherwise, move toward target
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        Vector3 moveTo = (target.position - transform.position).normalized;

                        transform.position += moveTo * moveSpeed * Time.deltaTime;

                        Vector3 currScale = transform.localScale;
                        Vector3 currHpBarScale = hpBar.transform.localScale;

                        if (moveTo.x * defaultDirectionRight > 0)
                        {
                            transform.localScale = new Vector3(Mathf.Abs(currScale.x), currScale.y, currScale.z);
                            hpBar.transform.localScale = new Vector3(Mathf.Abs(currHpBarScale.x), currHpBarScale.y, currHpBarScale.z);
                        }
                        else
                        {
                            transform.localScale = new Vector3(-Mathf.Abs(currScale.x), currScale.y, currScale.z);
                            hpBar.transform.localScale = new Vector3(-Mathf.Abs(currHpBarScale.x), currHpBarScale.y, currHpBarScale.z);
                        }
                    }

                }

            }

            // if (isAttacking && Time.time - lastAttackTime >= attackInterval)
            // {
            //     animator.SetBool("isAttack", false);
            //     isAttacking = false;
            // }
        }

        else if (!isDead && GameManager.instance.isFreezeMode)
        {
            animator.speed = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet" && Time.time - lastHitTime >= hitStaggerTime)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            hp -= bullet.damage;
            hpBar.SetHP(hp, maxHP);
            lastHitTime = Time.time;

            Debug.Log("Damage Taken : " + bullet.damage + " / HP : " + hp);

            if (hp <= 0)
            {
                // Dead enemy; perform death animation & disable collider 

                animator.SetBool("isDead", true);
                isDead = true;
                animator.speed = 1;
                polyCollider.enabled = false;
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }

                if (capsuleCollider != null)
                {
                    capsuleCollider.enabled = false;
                }

                if (gameObject.tag == "Boss")
                {
                    GameManager.instance.SetGameOver(true);
                }
                Destroy(gameObject, 2f);
            }
            else
            {
                // Hit enemy; flash red to show hit
                animator.SetBool("isHit", true);
                isHit = true;
                spriteRenderer.color = hitColor;
                Invoke("ResetColor", 0.1f);
            }
            bullet.Remove();
        }

        else if (other.gameObject.tag == "Grenade")
        {
            Grenade grenade = other.gameObject.GetComponent<Grenade>();
            hp -= grenade.damage;
            hpBar.SetHP(hp, maxHP);
            Debug.Log("Grenade Damage Taken : " + grenade.damage + " / HP : " + hp);
            if (hp <= 0)
            {
                // Dead enemy; perform death animation & disable collider 
                animator.SetBool("isDead", true);
                isDead = true;
                animator.speed = 1;
                polyCollider.enabled = false;
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }

                if (capsuleCollider != null)
                {
                    capsuleCollider.enabled = false;
                }

                if (gameObject.tag == "Boss")
                {
                    GameManager.instance.SetGameOver(true);
                }
                Destroy(gameObject, 2f);
            }
            else
            {
                // Hit enemy; flash red to show hit
                animator.SetBool("isHit", true);
                isHit = true;
                spriteRenderer.color = hitColor;
                Invoke("ResetColor", 0.1f);
            }
        }
    }

    public void KillSelf()
    {
        animator.SetBool("isDead", true);
        isDead = true;
        animator.speed = 1;
        polyCollider.enabled = false;
        Destroy(gameObject, 2f);
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }
}
