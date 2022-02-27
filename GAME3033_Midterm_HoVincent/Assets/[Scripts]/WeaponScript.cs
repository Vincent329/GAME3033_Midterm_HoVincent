using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [Header("ExplosionToSpawn")]
    [SerializeField]
    private GameObject explosion;

    private Vector3 spawnItem;
    public Vector3 SpawnLocationAccess
    {
        get => spawnItem;
        set
        {
            spawnItem = value;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// call this through the player controller somehow
    /// </summary>
    /// <param name="obj"></param>
    public void FireWeapon(Transform locationOfSpawn)
    {
        Instantiate(explosion, locationOfSpawn.position, locationOfSpawn.rotation);
    }
}
