using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroyAfter = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

}
