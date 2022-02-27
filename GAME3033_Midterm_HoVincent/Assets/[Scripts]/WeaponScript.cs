using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    

    void Start()
    {
      
    }

    private void OnEnable()
    {
      
    }
    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// call this through the player controller somehow
    /// </summary>
    /// <param name="obj"></param>
    public void FireWeapon(InputAction.CallbackContext obj)
    {
        Debug.Log("BAP BAP BAP");
    }
}
