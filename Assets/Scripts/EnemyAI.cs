using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;
    public Gun[] loadout;
    public Transform weaponParent;

    private GameObject player;
    private GameObject botWeapon;
    private float playerDist;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        

        playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (playerDist < 15f) LookAtPlayer();
        else if (botWeapon != null) Destroy(botWeapon);

        if (playerDist < 12f && playerDist > 5f)
        {
            Chase();
            Equip(0);
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
    void Equip(int i)
    {
        if (botWeapon != null) Destroy(botWeapon);

        GameObject t_newEquip = Instantiate(loadout[i].prefab, weaponParent);
        t_newEquip.transform.localPosition = Vector3.zero;
        t_newEquip.transform.localEulerAngles = Vector3.zero;

        botWeapon = t_newEquip;

    }
}
