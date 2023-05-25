using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpecial;
    [SerializeField] private GameObject grenade;

    private Transform gunTransform;
    private Transform grenadeTransform;
    private Animator animator;
    private HPBar hpBar;

    private SpriteRenderer spriteRenderer;
    private bool isShooting;
    private bool isThrowing;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float shootInterval = 0.3f;

    private float originalShootInterval;
    [SerializeField] private float maxHp = 50f;

    [SerializeField] private Color hitColor;


    private float throwInterval = 0.45f;
    private Color originalColor;
    private float hp;

    private float damageTime = 0.3f;
    private float lastDamagedTime;

    private float lastShotTime;

    private bool isDead = false;

    private int grenadeCount = 3;


    Vector3 grenadeTarget;
    public PolygonCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hpBar = GetComponentInChildren<HPBar>();
        playerCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunTransform = transform.Find("Gun");
        grenadeTransform = transform.Find("Grenade");
        hp = maxHp;
        originalColor = spriteRenderer.color;
        originalShootInterval = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !GameManager.instance.isGameOver)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");


            Vector3 moveTo = new Vector3(horizontalInput, verticalInput, 0f).normalized; // Normalize to set speed when moving diagonal to be constant as single direction

            if (!isShooting && !isThrowing)
            {
                // only move while not shooting
                transform.position += moveTo * moveSpeed * Time.deltaTime;
            }


            if (moveTo != Vector3.zero)
            {
                // if moving; perform walking animation
                animator.SetBool("isWalking", true);
            }
            else
            {
                // else, perform idle animation
                animator.SetBool("isWalking", false);
            }


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            Vector3 currScale = transform.localScale;
            Vector3 currHpBarScale = hpBar.transform.localScale;

            // Change sprite direction based on mouse cursor position
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(currScale.x), currScale.y, currScale.z);
                hpBar.transform.localScale = new Vector3(Mathf.Abs(currHpBarScale.x), currHpBarScale.y, currHpBarScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(currScale.x), currScale.y, currScale.z);
                hpBar.transform.localScale = new Vector3(-Mathf.Abs(currHpBarScale.x), currHpBarScale.y, currHpBarScale.z);
            }

            if (!isThrowing && Input.GetMouseButton(0))
            {
                // Shoot weapon when left mouse button is pressed in the direction of mouse
                if (Time.time - lastShotTime >= shootInterval)
                {
                    animator.SetBool("isShooting", true);
                    isShooting = true;

                    Vector2 bulletDirection = mousePos - transform.position;
                    float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

                    Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);


                    if (GameManager.instance.isFeverMode == true)
                    {
                        Instantiate(bulletSpecial, gunTransform.position, bulletRotation);
                    }
                    else
                    {
                        Instantiate(bullet, gunTransform.position, bulletRotation);
                    }

                    lastShotTime = Time.time;

                }
            }



            if (!isShooting && grenadeCount > 0 && Input.GetMouseButtonDown(1))
            {
                // Shoot weapon when left mouse button is pressed in the direction of mouse
                if (Time.time - lastShotTime >= shootInterval)
                {
                    animator.SetBool("isThrowing", true);
                    isThrowing = true;
                    grenadeTarget = mousePos;
                    Invoke("throwGrenade", throwInterval);
                    lastShotTime = Time.time;
                    grenadeCount -= 1;

                }
            }

            if (isShooting && Time.time - lastShotTime >= shootInterval)
            {
                animator.SetBool("isShooting", false);
                isShooting = false;
            }

            if (isThrowing && Time.time - lastShotTime >= throwInterval)
            {
                animator.SetBool("isThrowing", false);
                isThrowing = false;
            }

        }

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //BoxCollider2D box = other.gameObject.GetComponent<BoxCollider2D>();
            PolygonCollider2D poly = other.gameObject.GetComponent<PolygonCollider2D>();

            if (poly.IsTouching(playerCollider))
            {
                TakeDamage(enemy.collisionDamage);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //BoxCollider2D box = other.gameObject.GetComponent<BoxCollider2D>();
            PolygonCollider2D poly = other.gameObject.GetComponent<PolygonCollider2D>();

            if (poly.IsTouching(playerCollider))
            {
                TakeDamage(enemy.collisionDamage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with enemy
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            bool isAttack = enemy.animator.GetBool("isAttack");

            BoxCollider2D box = other.gameObject.GetComponent<BoxCollider2D>();
            CapsuleCollider2D capsule = other.gameObject.GetComponent<CapsuleCollider2D>();
            //PolygonCollider2D poly = other.gameObject.GetComponent<PolygonCollider2D>();

            // If attacked by enemy
            if (box != null)
            {
                if (box.IsTouching(playerCollider))
                {
                    if (isAttack)
                    {
                        TakeDamage(enemy.attackDamage);
                    }
                }
            }
            else if (capsule != null)
            {
                if (capsule.IsTouching(playerCollider))
                {
                    if (isAttack)
                    {
                        TakeDamage(enemy.attackDamage);
                    }
                }
            }

            // Collision damage with enemy
            // else if (poly.IsTouching(playerCollider))
            // {
            //     TakeDamage(enemy.collisionDamage);
            // }

        }

        // Collision with Items
        else if (other.gameObject.tag == "Item")
        {
            if (other.gameObject.name == "ItemHP(Clone)")
            {
                Debug.Log("Heal +20");
                hp += 20;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
                hpBar.SetHP(hp, maxHp);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name == "ItemTime(Clone)")
            {
                Debug.Log("Freeze enemy for 5s");
                GameManager.instance.SetFreezeMode(true);
                CancelInvoke("ResetFreezeMode");
                Invoke("ResetFreezeMode", 5f);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name == "ItemBomb(Clone)")
            {
                Debug.Log("Grenade +1");
                grenadeCount += 1;
                if (grenadeCount > 10)
                {
                    grenadeCount = 10;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name == "ItemSpecial(Clone)")
            {
                Debug.Log("Gun Fever time for 5s");
                shootInterval = 0.05f;
                GameManager.instance.SetFeverMode(true);
                CancelInvoke("ResetFeverMode");
                Invoke("ResetFeverMode", 5f);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Collision with enemy (while still in range for collision)
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            bool isAttack = enemy.animator.GetBool("isAttack");

            BoxCollider2D box = other.gameObject.GetComponent<BoxCollider2D>();
            CapsuleCollider2D capsule = other.gameObject.GetComponent<CapsuleCollider2D>();
            //PolygonCollider2D poly = other.gameObject.GetComponent<PolygonCollider2D>();

            if (box != null)
            {
                if (box.IsTouching(playerCollider))
                {
                    if (isAttack)
                    {
                        TakeDamage(enemy.attackDamage);
                    }
                }
            }
            else if (capsule != null)
            {
                if (capsule.IsTouching(playerCollider))
                {
                    if (isAttack)
                    {
                        TakeDamage(enemy.attackDamage);
                    }
                }
            }
            // else if (poly.IsTouching(playerCollider))
            // {
            //     TakeDamage(enemy.collisionDamage);
            // }
        }
    }


    private void ResetFeverMode()
    {
        shootInterval = originalShootInterval;
        GameManager.instance.SetFeverMode(false);

    }

    private void ResetFreezeMode()
    {
        GameManager.instance.SetFreezeMode(false);
    }
    private void TakeDamage(float damage)
    {

        if (GameManager.instance.isFreezeMode == false)
        {
            if (Time.time - lastDamagedTime >= damageTime)
            {
                hp -= damage;
                hpBar.SetHP(hp, maxHp);
                Debug.Log("Player HP : " + hp + " / Damage dealt : " + damage);
                lastDamagedTime = Time.time;

                if (hp <= 0)
                {
                    animator.SetBool("isDead", true);
                    isDead = true;
                    playerCollider.enabled = false;
                    GameManager.instance.SetGameOver(false);
                }
                else
                {
                    spriteRenderer.color = hitColor;
                    Invoke("ResetColor", 0.1f);
                }

            }
        }
    }

    public void SetIdleAnimation()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isThrowing", false);

    }
    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    private void throwGrenade()
    {

        GameObject newObject = Instantiate(grenade, grenadeTransform.position, Quaternion.identity) as GameObject;
        Grenade newNade = newObject.GetComponent<Grenade>();
        newNade.SetTarget(grenadeTarget);
    }

}
