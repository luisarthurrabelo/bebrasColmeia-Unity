using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDoor : MonoBehaviour
{
    public bool isColliding = false;
    public Image levelDescription;
    public GameObject playerCapsule;
    private FirstPersonController firstPersonController;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = false;
        }
    }

    private void Start()
    {
        firstPersonController = playerCapsule.GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        if(isColliding)
        {
            levelDescription.gameObject.SetActive(true);
            firstPersonController.enabled = false;
            SetCursorLock(true);

        }
        else
        {
            levelDescription.gameObject.SetActive(false);
            firstPersonController.enabled = true;
            SetCursorLock(false);
        }
    }

    void SetCursorLock(bool isLocked)
    {
        Cursor.visible = isLocked;

        if(isLocked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void changeCollidingState()
    {
        isColliding = false;
    }
}
