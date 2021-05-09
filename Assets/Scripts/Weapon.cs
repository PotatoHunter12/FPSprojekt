using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Weapon : MonoBehaviour
{
    public Gun[] loadout;
    public Transform weaponParent;
    public Transform enemyParent;
    public GameObject bulletHole;
    public LayerMask shootable;
    public LayerMask alive;
    public GameObject enemy;
    public Transform podn;
    public GameObject pts;

    private float curCD;
    private float spawn_x;
    private float spawn_z;
    private int gunIndex;
    private GameObject curWeapon;
    private GameObject nmeHit;
    private int points;
    private float damage;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            spawn_x = podn.position.x + Random.Range(1, 249);
            spawn_z = podn.position.z + Random.Range(1, 249);

            Vector3 spawn = new Vector3(spawn_x, 0.1f, spawn_z);

            Instantiate(enemy, spawn, Quaternion.identity,enemyParent);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(1);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) Equip(2);
        if (curWeapon != null)
        {
            Aim(Input.GetMouseButton(1));
            if(Input.GetMouseButtonDown(0) && curCD <= 0) Shoot();

            curWeapon.transform.localPosition = Vector3.Lerp(curWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            if (curCD > 0) curCD -= Time.deltaTime;

        }
    }

    void Equip(int i)
    {
        if (curWeapon != null) Destroy(curWeapon);

        gunIndex = i;

        GameObject t_newEquip = Instantiate(loadout[i].prefab, weaponParent) as GameObject;
        t_newEquip.transform.localPosition = Vector3.zero;
        t_newEquip.transform.localEulerAngles = Vector3.zero;

        curWeapon = t_newEquip;
        damage = loadout[i].damage;
    }

    void Aim(bool aim)
    {

        Transform t_aim_stt = curWeapon.transform.Find("States/Aim");
        Transform t_hip_stt = curWeapon.transform.Find("States/Hip");
        Transform t_anchor = curWeapon.transform.Find("Anchor");

        if(aim) t_anchor.position = Vector3.Lerp(t_anchor.position, t_aim_stt.position, Time.deltaTime * loadout[gunIndex].aimSpeed);
        else t_anchor.position = Vector3.Lerp(t_anchor.position, t_hip_stt.position, Time.deltaTime * loadout[gunIndex].aimSpeed);

    }

    void Shoot()
    {
        Transform t_spawn = transform.Find("Camera");

        //bloom
        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
        if (!Input.GetMouseButton(1))
        {
            t_bloom += Random.Range(-loadout[gunIndex].accuracy, loadout[gunIndex].accuracy) * t_spawn.up;
            t_bloom += Random.Range(-loadout[gunIndex].accuracy, loadout[gunIndex].accuracy) * t_spawn.right;
            t_bloom -= t_spawn.position;
            t_bloom.Normalize();
        }
        spawn_x = podn.position.x + Random.Range(1, 249);
        spawn_z = podn.position.z + Random.Range(1, 249);

        Vector3 spawn = new Vector3(spawn_x, 0.1f, spawn_z);

        //raycast
        RaycastHit t_hit = new RaycastHit();

        if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, alive))
        {
            nmeHit = t_hit.transform.gameObject;
            nmeHit.GetComponent<EnemyAI>().health -= (int)damage;
            if (nmeHit.GetComponent<EnemyAI>().health <= 0)
            {
                Destroy(nmeHit,0.5f);
                points += 1;
                pts.GetComponent<Text>().text = "Points: " + points;
                Instantiate(enemy, spawn, Quaternion.identity, enemyParent);
            }
        }
        else if(Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, shootable))
        {
            GameObject t_hole = Instantiate(bulletHole, t_hit.point + t_hit.normal * 0.001f,Quaternion.identity);
            t_hole.transform.LookAt(t_hit.point + t_hit.normal);
            Destroy(t_hole, 5f);
        }

        curWeapon.transform.Rotate(-loadout[gunIndex].recoil, 0, 0);
        curWeapon.transform.position -= curWeapon.transform.forward * loadout[gunIndex].kickback;

        curCD = loadout[gunIndex].fireRate;
    }
}
