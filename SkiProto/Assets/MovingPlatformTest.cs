using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    float currTime;
    Vector3 startPos;
    void Update()
    {
        Vector3 currPos = gameObject.GetComponent<Rigidbody>().position;

        currTime += Time.deltaTime / 3f;

        currPos.y = Mathf.Sin(currTime * 10f) + startPos.y;
        currPos.z = Mathf.Sin(currTime * 10f) + startPos.z;

        gameObject.GetComponent<Rigidbody>().MovePosition(currPos);
    }
}
