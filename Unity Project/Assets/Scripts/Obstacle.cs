using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Arrow"){
			col.gameObject.GetComponent<Arrow>().canMove = false;
		}
	}
}
