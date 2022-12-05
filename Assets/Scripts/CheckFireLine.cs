using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFireLine : MonoBehaviour
{
    private Explosion ExplosionScript;
    private MapRandomizer mapRandomizerScript;

    private void Start()
    {
        mapRandomizerScript = GameObject.Find("Objects").GetComponent<MapRandomizer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bomb" && this.gameObject.name != "fire_center" )
        {
            ExplosionScript = col.gameObject.GetComponent<Explosion>();
            if(ExplosionScript.timeRemaining > 0.05f)
                ExplosionScript.timeRemaining = 0.05f;
        }
        if(col.gameObject.tag == "Player")
        {
            PlayerController tempPlayerScript = col.gameObject.GetComponent<PlayerController>();
            if (tempPlayerScript.invincible == false)
            {
                tempPlayerScript.runSpeed = 0f;
                Destroy(col.gameObject, 1);
            }
        }
        if (col.gameObject.tag == "Enemy")
        {
            EnemyOne tempEnemyScript = col.gameObject.GetComponent<EnemyOne>();
            if (tempEnemyScript.invincible == true) return;

            col.gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("isDead");
            col.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            col.gameObject.GetComponent<Collider2D>().enabled = false;
            col.gameObject.GetComponent<EnemyOne>().walkSpeed = 0f;
            Destroy(col.gameObject, 3);
            mapRandomizerScript.EnemyCounter.Remove(col.gameObject);
            if (mapRandomizerScript.EnemyCounter.Count == 0)
            {
                GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>().SetBool("canUseDoor", true);
            }
        }
        if (col.gameObject.tag == "Door")
        {
            col.GetComponent<Animator>().SetBool("canUseDoor", false);
            
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<Animator>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;

            mapRandomizerScript.SpawnEnemiesAtDoor(col.gameObject.transform.localPosition);
        }
    }
}
