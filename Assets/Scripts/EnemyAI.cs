using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;
    public Animator animator;
    public int health;

    private GameObject player;
    private float playerDist;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool("Run", playerDist < 20f && playerDist > 1.5f);
        animator.SetBool("Hit", playerDist <= 1.5f);

        playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (health > 0)
        {
            if (playerDist < 30f) LookAtPlayer();
            if (playerDist < 20f && playerDist >= 1.5f) Chase();
        }
        else
        {
            animator.SetBool("dead", true);
        }
    }

    private void Chase()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
    }

    private void LookAtPlayer()
    {
        Quaternion rotate = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 5f);
    }
}
