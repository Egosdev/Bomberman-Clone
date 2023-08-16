using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBomb : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
