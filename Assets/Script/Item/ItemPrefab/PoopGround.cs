using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoopGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            col.GetComponent<PlayerMovement>().GetSlowed();
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            col.GetComponent<PlayerMovement>().GetOffSlowed();
        }
    }
}
