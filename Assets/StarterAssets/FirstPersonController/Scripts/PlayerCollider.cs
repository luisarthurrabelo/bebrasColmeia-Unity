using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform camera;
    private InteractScript interact;

    private void Start()
    {
        interact = camera.GetComponent<InteractScript>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("npcRange"))
        {
            //Debug.Log(interact.canTalk);
            interact.canTalk = true;
        }
    }
}
