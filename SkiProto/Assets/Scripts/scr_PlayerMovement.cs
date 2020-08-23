using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{
    GameObject playerVisor;
    float cameraStartRotHoriz;
    float cameraStartRotVert;

    // Start is called before the first frame update
    void Start()
    {
        playerVisor = gameObject.transform.Find("Visor").gameObject;

        cameraStartRotVert = playerVisor.transform.rotation.x;
        cameraStartRotHoriz = gameObject.transform.rotation.y;

        newX = cameraStartRotVert;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();

        UpdateMovement();
    }

    float newX;
    void UpdateCamera()
    {
        // Vertical
        float tempVert = -Input.GetAxis("Mouse Y");

        // Horizontal
        float tempHoriz = Input.GetAxis("Mouse X");

        Vector3 playerRot = gameObject.transform.eulerAngles;
        playerRot.y += tempHoriz;
        gameObject.transform.eulerAngles = playerRot;

        newX += tempVert;
        newX = Mathf.Clamp(newX, -85f, 85f);

        Vector3 camVertRot = playerVisor.transform.eulerAngles;
        camVertRot.x = newX;
        playerVisor.transform.eulerAngles = camVertRot;
    }

    void UpdateMovement()
    {
        float MOVESPEED = 10f;

        Rigidbody thisRigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 oldVelocity = thisRigidBody.velocity;
        float oldGravity = oldVelocity.y;

        Vector3 v3_InputVector = new Vector3();

        bool forward_ = Input.GetKey(KeyCode.W);
        bool back_ = Input.GetKey(KeyCode.S);
        bool left_ = Input.GetKey(KeyCode.A);
        bool right_ = Input.GetKey(KeyCode.D);
        bool jump_ = Input.GetKey(KeyCode.Space);

        bool inAir_ = true;
        RaycastHit Hit_;
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (Vector3.down * 1f), Color.red);
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out Hit_, 1f))
        {
            print(Hit_.transform.name);

            if (Hit_.transform.gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
                if (!forward_ && !back_ && !left_ && !right_ && !jump_)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = Hit_.transform.gameObject.GetComponent<Rigidbody>().velocity;

                    Vector3 playerNewPos = gameObject.GetComponent<Rigidbody>().position;
                    playerNewPos.y = Hit_.transform.gameObject.GetComponent<Rigidbody>().position.y + 0.75f + (gameObject.transform.localScale.y / 2f);
                    gameObject.GetComponent<Rigidbody>().MovePosition(playerNewPos);
                }

            inAir_ = false;
        }

        if (forward_ && !back_)
            v3_InputVector.x = 1;
        if (back_ && !forward_)
            v3_InputVector.x = -1;

        if (left_ && !right_)
            v3_InputVector.z = 1;
        if (right_ && !left_)
            v3_InputVector.z = -1;

        // If two opposing directions are held, negate them both
        if (forward_ && back_)
        {
            forward_ = false;
            back_ = false;
        }

        if (left_ && right_)
        {
            left_ = false;
            right_ = false;
        }

        v3_InputVector.Normalize();

        // Align input vector to player rotation
        v3_InputVector = gameObject.transform.rotation * v3_InputVector;

        SetFriction(!forward_ && !back_ && !left_ && !right_ && !jump_ && !inAir_);

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

    void SetFriction(bool IncreaseFriction_)
    {
        PhysicMaterial tempPhysMat = gameObject.GetComponent<Collider>().material;
        float MASS = gameObject.GetComponent<Rigidbody>().mass;

        float frictionAmount = tempPhysMat.dynamicFriction;
        tempPhysMat.frictionCombine = PhysicMaterialCombine.Minimum;

        float tempDrag = gameObject.GetComponent<Rigidbody>().drag;
        
        if ( IncreaseFriction_ )
        {
            frictionAmount += Time.deltaTime * MASS * 5f;
            if (frictionAmount > 10.0f) frictionAmount = 10.0f;

            tempPhysMat.frictionCombine = PhysicMaterialCombine.Maximum;

            if( tempDrag < MASS )
            {
                tempDrag += Time.deltaTime * MASS;
                if (tempDrag > MASS) tempDrag = MASS;
            }

            gameObject.GetComponent<Rigidbody>().drag = MASS;
        }
        else
        {
            frictionAmount = 0f;
            gameObject.GetComponent<Rigidbody>().drag = 0;
        }

        tempPhysMat.dynamicFriction = frictionAmount;
        tempPhysMat.staticFriction = frictionAmount;

        gameObject.GetComponent<Collider>().material = tempPhysMat;

        // print("Friction: " + gameObject.GetComponent<Collider>().material.dynamicFriction);
    }
}
