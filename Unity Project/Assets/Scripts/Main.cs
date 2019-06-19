using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public Transform startPosition, targetPosition;
	public GameObject arrowPrefab, runPanel, processPanel;
	public Text timerUI, GenerationsLabel, MaxFitnessLabel;
	public InputField PopulationInF, MutRateInF, StartPosInF, TargetPosInF, ObjVelocityInF, DirChangeTimeInF, TimePerGenInf;
	public Button RunButton, StopButton;
	public System.Timers.Timer timer;
	public System.Random random;
	private Stopwatch stopwatch;
	public Population population;
	private static Main _instance;
	public float timePerGeneration, moveSpeed, directionChangeTime;
	public double ElapsedTimeInSeconds;
	public int populationMax, mutationRate, generationsNum;
	public bool generationIsRunning = false;

	void Start () {
		_instance = this;
		random = new System.Random();
		stopwatch = new Stopwatch();
	}
	
	void Update () {
		if(generationIsRunning){
			ElapsedTimeInSeconds = System.Math.Round(System.Math.Round((System.Convert.ToDouble(stopwatch.ElapsedMilliseconds)/1000),2), 2);
			timerUI.text = ElapsedTimeInSeconds.ToString();
			GenerationsLabel.text = generationsNum.ToString();
			MaxFitnessLabel.text = population.maxFitness.ToString();
		}

		
	}
	public void InsantiateNewPopulation(){
		populationMax = System.Convert.ToInt32(PopulationInF.text);
		mutationRate = System.Convert.ToInt32(MutRateInF.text);
		moveSpeed = float.Parse(ObjVelocityInF.text);
		directionChangeTime = float.Parse(DirChangeTimeInF.text);
		timePerGeneration = float.Parse(TimePerGenInf.text);
		Vector3 startInputFieldPosition = StartPosInF.GetComponent<PositionInputField>().position;
		Vector3 targetInputFieldPosition = TargetPosInF.GetComponent<PositionInputField>().position;
		startPosition.position = new Vector3(startInputFieldPosition.x, startInputFieldPosition.y, 0);
		targetPosition.position = new Vector3(targetInputFieldPosition.x, targetInputFieldPosition.y, 0);
		startPosition.gameObject.SetActive(true);
		targetPosition.gameObject.SetActive(true);


		timer = new System.Timers.Timer(timePerGeneration*1000);
		timer.Elapsed += GenerationTimeElapsed;

		population = new Population(populationMax,startPosition, targetPosition, random);
		RunButton.gameObject.SetActive(false);
		StopButton.gameObject.SetActive(true);
		processPanel.SetActive(true);
		runPanel.SetActive(false);
		InstantiatePopulation();
	}
	public void Reset(){
		foreach(GameObject obj in population.populationObjects){
			Destroy(obj);
		}
		timer.Stop();
		stopwatch.Stop();
		stopwatch.Reset();
		generationsNum = 0;
		timerUI.text = "";
		GenerationsLabel.text = "";
		MaxFitnessLabel.text = "";
		PopulationInF.text = "";
		MutRateInF.text = "";
		StartPosInF.text = "";
		TargetPosInF.text = "";
		ObjVelocityInF.text = "";
		DirChangeTimeInF.text = "";
		TimePerGenInf.text = "";
		RunButton.gameObject.SetActive(true);
		StopButton.gameObject.SetActive(false);
		processPanel.SetActive(false);
		runPanel.SetActive(true);
	}
	void InstantiatePopulation(){
		for(int i = 0; i < populationMax; i++){
			GameObject modifiedPrefab = arrowPrefab;
			modifiedPrefab.GetComponent<Arrow>().mainScript = this;
			modifiedPrefab.GetComponent<Arrow>().moveSpeed = this.moveSpeed;
			GameObject instance =  Instantiate(modifiedPrefab, startPosition.position, Quaternion.identity);
			if(generationsNum >= 1){
				instance.GetComponent<Arrow>().objectDNA = this.population.populationDNA[i];
			}
			population.populationObjects[i] = instance;
		}
		timer.Start();
		stopwatch.Start();
		generationIsRunning = true;
		generationsNum++;
	}

	void GenerationTimeElapsed(object sender, System.Timers.ElapsedEventArgs args){		
		generationIsRunning = false;
		timer.Stop();
		stopwatch.Stop();
		stopwatch.Reset();
		UnityMainThreadDispatcher.Instance().Enqueue(NextGeneration());
	}
	IEnumerator NextGeneration(){
		timerUI.text = "";
		
		for(int i = 0; i < populationMax; i++){
			population.populationObjects[i].GetComponent<Arrow>().objectDNA.finalPosition = population.populationObjects[i].transform.position;
			if(generationsNum == 1){
				population.populationDNA[i] = population.populationObjects[i].GetComponent<Arrow>().objectDNA;
			}
		}
		foreach(GameObject obj in population.populationObjects){
			Destroy(obj);
		}

		population.CalculateFitness();

		population.SelectMateReproduct(mutationRate);

		InstantiatePopulation();
		
		yield return null;
	}

	public static Main GetInstance(){
		return _instance;
	}

	

}
