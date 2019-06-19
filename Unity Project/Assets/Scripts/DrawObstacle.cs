using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObstacle : MonoBehaviour {
	public bool isDrawing = false;
	public Vector3 Corner1;
	public Vector3 Corner2;
	public GameObject ObstaclePrefab, DeleteObstaclesButton;
	public Canvas worldSpaceCanvas;
	public List<GameObject> Obstacles;
	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.LeftControl)){
			isDrawing = true;
		}
		if(Input.GetKeyUp(KeyCode.LeftControl)){
			isDrawing = false;
		}
		if(isDrawing){
			if(Input.GetMouseButtonDown(0)){
				Vector3 rawCameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Corner1 = new Vector3(rawCameraPos.x, rawCameraPos.y, 0 );
			}
			if(Input.GetMouseButtonUp(0)){
				Vector3 rawCameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Corner2 = new Vector3(rawCameraPos.x, rawCameraPos.y, 0 );
				isDrawing = false;
				DrawRectangle();

			}
		}
	}

	void DrawRectangle(){
		Vector3 resultVector = Corner1 + Corner2;
		Vector3 rectangleCenter = new Vector3(resultVector.x/2, resultVector.y/2, 0);
		float halfHeight = Vector3.Distance(new Vector3(Corner1.x,0,0), new Vector3(rectangleCenter.x, 0,0));
		float halfWidth = Vector3.Distance(new Vector3(0,Corner1.y,0), new Vector3(0,rectangleCenter.y,0));
		GameObject instance = Instantiate(ObstaclePrefab);
		instance.transform.SetParent(worldSpaceCanvas.transform);
		instance.transform.position = rectangleCenter;
		instance.GetComponent<RectTransform>().sizeDelta = new Vector2(halfHeight*2, halfWidth*2);
		instance.GetComponent<BoxCollider2D>().size = new Vector2(halfHeight*2, halfWidth*2);

		if(Obstacles.Count == 0){
			DeleteObstaclesButton.SetActive(true);
		}
		Obstacles.Add(instance);
	}

	public void ClearObstacles(){
		foreach(GameObject obstacle in Obstacles){
			Destroy(obstacle);
		}
		Obstacles.Clear();
		DeleteObstaclesButton.SetActive(false);
	}
}
