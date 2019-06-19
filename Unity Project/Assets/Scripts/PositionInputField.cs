using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionInputField : MonoBehaviour {
	public Vector3 position;
	public bool inUse = false;
	void Start () {
		gameObject.GetComponent<InputField>().onEndEdit.AddListener(delegate{OnEndEdit();});
	}
	
	void Update () {
		if(gameObject.GetComponent<InputField>().isFocused){
			inUse = true;
			gameObject.GetComponent<InputField>().text = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString();
		}
	}

	void OnEndEdit(){
		position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		inUse = false;
	}

}
