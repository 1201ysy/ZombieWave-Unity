using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] public float damage = 5f;

    [SerializeField] private GameObject bleed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void Remove()
    {
        var bleedRotation = transform.rotation * Quaternion.Euler(0, 0, 44.6f);
        Vector3 bleedPos = transform.position + transform.TransformDirection(Vector3.right * 80f * Time.deltaTime);
        Instantiate(bleed, bleedPos, bleedRotation);

        Destroy(gameObject);
    }
}
