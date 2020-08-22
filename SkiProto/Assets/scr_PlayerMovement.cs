using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float MOVESPEED = 10f;

        Rigidbody thisRigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 oldVelocity = thisRigidBody.velocity;
        float oldGravity = oldVelocity.y;
        
        Vector3 v3_InputVector = new Vector3();

        bool forward_ = Input.GetKey(KeyCode.W);
        bool back_ = Input.GetKey(KeyCode.S);

        if (forward_ && !back_)
            v3_InputVector.x = 1;
        if (back_ && !forward_)
            v3_InputVector.x = -1;

        bool left_ = Input.GetKey(KeyCode.A);
        bool right_ = Input.GetKey(KeyCode.D);

        if (left_ && !right_)
            v3_InputVector.z = 1;
        if (right_ && !left_)
            v3_InputVector.z = -1;

        v3_InputVector.Normalize();
        Vector3 tempVelocity = v3_InputVector * MOVESPEED * thisRigidBody.mass;
        tempVelocity = tempVelocity - oldVelocity;

        // thisRigidBody.velocity = oldVelocity;
        thisRigidBody.AddForce(tempVelocity);

        // Replace Gravity
        Vector3 tempGrav = thisRigidBody.velocity;
        tempGrav.y = oldGravity * 1.01f;
        tempGrav.y = -Math.Abs(tempGrav.y);
        thisRigidBody.velocity = tempGrav;
    }
}
