using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			if(gameObject.GetComponent<Camera>().orthographicSize > 0.5f){
				gameObject.GetComponent<Camera>().orthographicSize -= 0.5f;
			}
		}

		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			gameObject.GetComponent<Camera>().orthographicSize += 0.5f;
		}

		if(Input.GetMouseButtonUp(1)){
			gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
