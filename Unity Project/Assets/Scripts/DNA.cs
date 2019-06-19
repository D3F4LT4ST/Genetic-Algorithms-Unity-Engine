using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA{
	public List<KeyValuePair<Vector3,float>> genotype;
	public Vector3 finalPosition;
	public float fitness;
	private System.Random random;
	public DNA(System.Random random){
		genotype = new List<KeyValuePair<Vector3, float>>();
		this.random = random;
		fitness = 0;
	}

	public void CalcFitness(Transform start, Transform target){
		float startDistanceToTarget = Vector3.Distance(start.position, target.position);
		float distanceLeftToTarget = Vector3.Distance(finalPosition, target.position);
		float relativeDistanceTraveled = Mathf.Clamp((startDistanceToTarget-distanceLeftToTarget), 0, startDistanceToTarget);
		fitness = relativeDistanceTraveled/startDistanceToTarget;
	}

	public DNA Crossover(DNA partner){
		
		DNA longestGenotypeObject = Mathf.Max(this.genotype.Count, partner.genotype.Count) == this.genotype.Count? this : partner;
		DNA shortestGenotypeObject = Mathf.Min(this.genotype.Count, partner.genotype.Count) == this.genotype.Count? this : partner;

		DNA child = new DNA(random);

		int shortestGenotypeObjectIndex = 0;
		for(int i = 0; i < longestGenotypeObject.genotype.Count; i++){
			if(random.Next(0,1) == 0 && shortestGenotypeObjectIndex < shortestGenotypeObject.genotype.Count){
				child.genotype.Add(shortestGenotypeObject.genotype[shortestGenotypeObjectIndex]);
				shortestGenotypeObjectIndex++;
			}else{
				child.genotype.Add(longestGenotypeObject.genotype[i]);
			}
		}

		return child;
	}
	public void Mutate(int mutationRate){
		for(int i = 0; i < this.genotype.Count; i++){
			if(random.Next(0,100) < mutationRate){
				this.genotype[i] = General.RandomGene();
			}
		}
	}

}
