using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InteractScript : MonoBehaviour
{
    public float interactRange = 5f;
    public Text _NPCDialogue;
    public string mensagemText;

    private void Start()
    {
        _NPCDialogue.gameObject.SetActive(false);
        ChangeNpcDialogue(mensagemText);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                if (hit.transform.gameObject.tag == "NPC")
                {
                    StartCoroutine(DialogueDelay());
                }
            }
        }
    }

    public void ChangeNpcDialogue(string message)
    {
        _NPCDialogue.text = message;
    }

    IEnumerator DialogueDelay()
    {
        _NPCDialogue.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _NPCDialogue.gameObject.SetActive(false);
    }
}
