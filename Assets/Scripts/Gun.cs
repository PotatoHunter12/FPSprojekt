using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun",menuName = "Gun")]

public class Gun : ScriptableObject
{
    public new string name;
    public float fireRate;
    public float accuracy;
    public float recoil;
    public float kickback;
    public float aimSpeed;
    public float damage;
    public GameObject prefab;
}
