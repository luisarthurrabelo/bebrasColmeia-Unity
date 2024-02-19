using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InteractScript : MonoBehaviour
{
    public Text _NPCDialogue;
    public string mensagemText;
    public bool canTalk;

    public float interactRange = 5f;

    [SerializeField] private Transform hive;
    private GridManager grid_component;
    private ObjectPosition hive_position;

    private void Start()
    {
        hive_position = hive.GetComponent<ObjectPosition>();
        grid_component = GameObject.FindObjectOfType<GridManager>();

        _NPCDialogue.gameObject.SetActive(false);
        ChangeNpcDialogue(mensagemText);

        canTalk = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                if (hit.transform.gameObject.tag == "NPC" && canTalk)
                {
                    _NPCDialogue.gameObject.SetActive(true);
                    canTalk = false;
                }
                else
                {
                    _NPCDialogue.gameObject.SetActive(false);
                    canTalk = true;
                }

                if(hit.transform.gameObject.tag == "Button")
                {
                    //Chamar função que "aperta" botão

                    if (hive_position.TilePosition == grid_component.OptimalSolution)
                    {
                        Debug.Log("Acertou");
                    }
                    else
                    {
                        Debug.Log("errou");
                        hive_position.TilePosition = new Vector2(-1, -1);
                        hive.position = new Vector3(-1.025035f, 0.6f, 9.76f);
                        grid_component.wrongGuess = true;
                    }
                }
            }
        }

        if (canTalk)
        {
            _NPCDialogue.gameObject.SetActive(false);
            canTalk = true;
        }
    }

    public void ChangeNpcDialogue(string message)
    {
        _NPCDialogue.text = message;
    }
}
