using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour

{

    private GameObject player;

    void Start()
    
    {

        // Finds the player object.
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
 
    {

        // Makes the object that this script is attached to always look at the player. This script is attached to the helicopter camera and helicopter spotlight.
        transform.LookAt(player.transform.position);

    }

}