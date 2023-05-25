using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{

    private Transform HPGreen;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        HPGreen = transform.Find("HPGreen");
        originalScale = HPGreen.localScale;
    }

    public void SetHP(float hp, float maxHP)
    {
        float scale = Mathf.Clamp(hp / maxHP, 0, 1);

        HPGreen.localScale = new Vector3(originalScale.x * scale, originalScale.y, originalScale.z);

    }
}
