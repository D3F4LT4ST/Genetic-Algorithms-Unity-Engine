using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General{

	public static KeyValuePair<Vector3, float> RandomGene(){
		Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-1f,1f),UnityEngine.Random.Range(-1f,1f),0);
		float randomTime =  UnityEngine.Random.Range(0,Main.GetInstance().directionChangeTime);
		KeyValuePair<Vector3, float> randomGene = new KeyValuePair<Vector3, float>(randomVector, randomTime);
		return randomGene;
	
	}
	public static T Clamp<T>(T value, T min, T max) where T: IComparable{
		if (value.CompareTo(min) < 0) return min;
		else if (value.CompareTo(max) > 0) return max;
		else return value;
	}
}
