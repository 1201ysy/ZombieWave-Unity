using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private GameObject player;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            float posX = Mathf.Clamp(player.transform.position.x, minX, maxX);
            float posY = Mathf.Clamp(player.transform.position.y, minY, maxY);

            transform.position = new Vector3(posX, posY, -10f);
        }
    }
}
