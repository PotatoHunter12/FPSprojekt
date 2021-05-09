using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region var
        public float speed;
        public float sprintModifier;
        public float jumpForce;

        //public Camera normalCam;
        public Transform groundDetect;
        public Transform weaponParent;
        public LayerMask ground;
        public Animator uok;
        private Rigidbody rig;
        private Vector3 parentOrigin;
        private Vector3 bobPos;

        private float moveCounter;
        private float idleCounter;
    #endregion
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        parentOrigin = weaponParent.localPosition;
    }

    private void Update()
    {
        //Axis
        float t_hmove = Input.GetAxis("Horizontal");
        float t_vmove = Input.GetAxis("Vertical");

        

        //Contorls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKey(KeyCode.Space);

        //States
        bool isGrounded = Physics.Raycast(groundDetect.position, Vector3.down, 0.1f, ground);
        bool isSprinting = sprint && t_vmove > 0 && !Input.GetMouseButton(1);

        //Animation
        uok.SetFloat("vertical", t_vmove + Convert.ToInt32(isSprinting));
        uok.SetFloat("horizontal", t_hmove);

        //headbob
        if (t_hmove == 0 && t_vmove == 0)
        {
            Headbob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, bobPos, Time.deltaTime * 2f);
        }
        else if(!isSprinting)
        { 
            Headbob(moveCounter, 0.04f, 0.04f);
            moveCounter += 3f * Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, bobPos, Time.deltaTime * 8f);
        }
        else
        {
            Headbob(moveCounter, 0.15f, 0.1f);
            moveCounter += 3f * Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, bobPos, Time.deltaTime * 20f);
        }

    }
    void FixedUpdate()
    {
        //Axis
        float t_hmove = Input.GetAxis("Horizontal");
        float t_vmove = Input.GetAxis("Vertical");


        ////Contorls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKey(KeyCode.Space);


        ////States
        bool isGrounded = Physics.Raycast(groundDetect.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !Input.GetMouseButton(1);


        //Movement


        if (isJumping) rig.AddForce(Vector3.up * jumpForce);
        
        Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
        t_direction.Normalize();
        
        float t_adjust = speed;
        if (isSprinting) t_adjust *= sprintModifier;

        
        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjust * Time.deltaTime;
        t_targetVelocity.y = rig.velocity.y;
        rig.velocity = t_targetVelocity;

    }

    void Headbob(float z,float x_intens,float y_intens)
    {
        bobPos = parentOrigin + new Vector3(Mathf.Cos(z) * x_intens, Mathf.Sin(z*2) * y_intens,0);

    }
}
