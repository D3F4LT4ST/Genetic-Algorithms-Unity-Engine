using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population{
	public GameObject[] populationObjects;
	public DNA[] populationDNA;
	public List<DNA> matingPool;
	private System.Random random;
	public Transform startPos, targetPos;
	public float maxFitness;

	public Population(int popSize,Transform startPos, Transform targetPos, System.Random random){
		populationObjects = new GameObject[popSize];
		populationDNA = new DNA[popSize];
		matingPool = new List<DNA>();
		maxFitness = 0;
		this.random = random;
		this.targetPos = targetPos;
		this.startPos = startPos;
	}

	public void CalculateFitness(){
		maxFitness = 0;
		foreach(DNA objectDNA in populationDNA){
			objectDNA.CalcFitness(startPos, targetPos);
			if(objectDNA.fitness > maxFitness){
				maxFitness = objectDNA.fitness;
			}
		}
	}

	public void SelectMateReproduct(int mutationRate){
		int numOfEntries = 0;
		foreach(DNA element in populationDNA){
			numOfEntries = System.Convert.ToInt32(System.Math.Floor(element.fitness * 100));
			for (int i = 0; i < numOfEntries; i++){
				matingPool.Add(element);
			}	
		}
		if(matingPool.Count == 0){
			for (int i = 0; i < populationDNA.Length; i ++){
				matingPool.Add(populationDNA[i]);
			}
		}
		for (int i = 0; i < populationDNA.Length; i++)
		{
			int randIntA = random.Next(0, General.Clamp(matingPool.Count-1, 0 , matingPool.Count-1));
			int randIntB = random.Next(0, General.Clamp(matingPool.Count-1, 0 , matingPool.Count-1));
			
			DNA parentA = matingPool[randIntA];
			DNA parentB = matingPool[randIntB];
			DNA child = parentA.Crossover(parentB);

			child.Mutate(mutationRate);

			populationDNA[i] = child;
		}
		matingPool.Clear();
	}
}
