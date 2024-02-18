using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HiveScript : MonoBehaviour
{
    private Rigidbody Hive_rb;
    private GridManager grid_component;

    public void Start()
    {
        Hive_rb = GetComponent<Rigidbody>();
        grid_component = GameObject.FindObjectOfType<GridManager>();
        GetComponent<ObjectPosition>().TilePosition = new Vector2(-1, -1);
    }

    private void Update()
    {
        Debug.Log(GetComponent<ObjectPosition>().TilePosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ItemSupport"))
        {
            var objectPosition = collision.gameObject.transform.position;

            Vector3 novaPosicao = new Vector3(objectPosition.x, objectPosition.y + 0.3f, objectPosition.z);
            transform.rotation = Quaternion.identity;
            transform.position = novaPosicao;
            StartCoroutine(AntiOverThrow());

            Vector2 HiveSupPosition = collision.gameObject.GetComponent<ObjectPosition>().TilePosition;
            //grid_component.addHiveOnHiveSupMatrix(HiveSupPosition, 3);
            GetComponent<ObjectPosition>().TilePosition = HiveSupPosition;
            
            //Debug.Log(transform.position);
        }

        IEnumerator AntiOverThrow()
        {
            Hive_rb.mass = 20;
            yield return new WaitForSeconds(0.5f);
            Hive_rb.mass = 1;
        }
    }
}
