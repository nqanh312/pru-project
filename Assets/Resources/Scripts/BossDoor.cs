using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject enterBossDoor;
    public GameObject existBossDoor;

    private void Start() {
        enterBossDoor.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
    enterBossDoor.SetActive(true);
        }
        
    }
}
