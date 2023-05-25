using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 target;

    [SerializeField] public float damage = 20f;
    private Animator animator;

    //[SerializeField] private float moveSpeed = 2f;
    private float dist;
    private float nextX;
    private float baseY;
    private float height;

    private bool targetSet = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTarget(Vector3 mousePos)
    {
        target = mousePos;
        startPos = transform.position;
        targetSet = true;

        Debug.Log(Mathf.Lerp(startPos.y, target.y, 0));
        Debug.Log(Mathf.Lerp(startPos.y, target.y, 0.3f));
        Debug.Log(Mathf.Lerp(startPos.y, target.y, 0.5f));
        Debug.Log(Mathf.Lerp(startPos.y, target.y, 0.7f));
        Debug.Log(Mathf.Lerp(startPos.y, target.y, 0.1f));
    }
    // Update is called once per frame
    void Update()
    {



        if (targetSet)
        {
            dist = Mathf.Abs(startPos.x - target.x);
            float moveSpeed = dist / 0.7f;

            nextX = Mathf.MoveTowards(transform.position.x, target.x, moveSpeed * Time.deltaTime);
            baseY = Mathf.Lerp(startPos.y, target.y, Mathf.Abs((nextX - startPos.x) / dist));

            // Debug.Log(dist);
            // Debug.Log("Next X : " + nextX + " / NextX - StartX" + (nextX - startPos.x) / dist);
            // Debug.Log("Start Y : " + startPos.y + " / Target Y : " + target.y + " / " + baseY);
            height = 2 * (nextX - startPos.x) * (nextX - target.x) / (-0.25f * dist * dist);

            Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
            //transform.rotation = LookAtTarget(movePosition - transform.position);
            transform.position = movePosition;

            Debug.Log("Pos : " + transform.position + " / Target : " + target);

            if (Mathf.Approximately(transform.position.x, target.x) && Mathf.Approximately(transform.position.y, target.y))
            {
                animator.SetBool("Explode", true);
                Destroy(gameObject, 2f);
            }
        }


    }


    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
