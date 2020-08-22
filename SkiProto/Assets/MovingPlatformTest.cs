using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        startZ = gameObject.transform.position.z;
    }

    // Update is called once per frame
    float currTime;
    float startZ;
    void Update()
    {
        Vector3 currPos = gameObject.GetComponent<Rigidbody>().position;

        currTime += Time.deltaTime / 3f;

        currPos.z = Mathf.Sin(currTime * 10f) + startZ;
        currPos.y = Mathf.Sin(currTime * 10f);

        gameObject.GetComponent<Rigidbody>().MovePosition(currPos);
    }
}
