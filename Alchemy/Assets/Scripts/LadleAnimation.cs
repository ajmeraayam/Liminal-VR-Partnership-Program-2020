using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadleAnimation : MonoBehaviour
{
    float timeCounter = 0f;
    float radius = 0.30f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * 2.5f;
        float z = Mathf.Cos (timeCounter) * radius;
        float x = Mathf.Sin (timeCounter) * radius - 0.05f;
        float y = transform.position.y;
        transform.position = new Vector3 (x, y, z);
    }
}
