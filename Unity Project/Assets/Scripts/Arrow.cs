using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	public Main mainScript;
	public System.Random random;
	public DNA objectDNA;
	public Vector3 currentDirection = Vector3.zero;
	public Vector3 distance, finalPos;
	public Func<IEnumerator> dependantBehaviour;
	public float moveSpeed = 2.0f;
	public float angle, moveTime;
	public int moveIndex = 0;
	public bool changeDirection = true;
	public bool canMove = true;

	void Start () {
		random = mainScript.random;
		if(mainScript.generationsNum == 1){
			dependantBehaviour = SetDirection;
			objectDNA = new DNA(random);
		}else{
			dependantBehaviour = ChangeDirectionAccordingToDNA;
		}
	}
	void Update () {
		if(canMove){
			if(changeDirection){
				if(objectDNA != null){
					StartCoroutine(dependantBehaviour.Invoke());
				}
			}
			transform.position += currentDirection.normalized*moveSpeed*Time.deltaTime;
		}
	}

	IEnumerator SetDirection(){
		currentDirection = General.RandomGene().Key;
		angle = Mathf.Atan(Mathf.Abs(currentDirection.y/currentDirection.x)) * Mathf.Rad2Deg;
		angle = angle*Math.Sign(currentDirection.x)*Math.Sign(currentDirection.y)+(180*Mathf.Clamp(Mathf.Sign(Mathf.Abs(currentDirection.y)/currentDirection.x) ,-1,0));
		transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
		changeDirection = false;

		moveTime = General.RandomGene().Value;
		objectDNA.genotype.Add(new KeyValuePair<Vector3, float>(currentDirection, moveTime));

		yield return new WaitForSeconds(moveTime);
		
		moveIndex++;
		changeDirection = true;

	}
	IEnumerator ChangeDirectionAccordingToDNA(){
		currentDirection = objectDNA.genotype[moveIndex].Key;
		angle = Mathf.Atan(Mathf.Abs(currentDirection.y/currentDirection.x)) * Mathf.Rad2Deg;
		angle = angle*Math.Sign(currentDirection.x)*Math.Sign(currentDirection.y)+(180*Mathf.Clamp(Mathf.Sign(Mathf.Abs(currentDirection.y)/currentDirection.x) ,-1,0));
		transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
		changeDirection = false;

		moveTime = objectDNA.genotype[moveIndex].Value;
		yield return new WaitForSeconds(moveTime);
		
		//moveIndex++;
		if(moveIndex < objectDNA.genotype.Count-1){
			moveIndex++;
			changeDirection = true;
		}
	}
	
}
