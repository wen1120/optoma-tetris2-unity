using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<Transform> ().Rotate (new Vector3(0, 1, 0));
	}

	void UpdateFacePosition(string position) {
		string[] xy = position.Split (';');
		float x = float.Parse (xy [0]);
		float y = float.Parse (xy [1]);
		float r = float.Parse (xy [2]);

		transform.position = new Vector3 (-(x - 0.5f) * 20, -(y - 0.5f) * 20, 0);
		transform.rotation = Quaternion.AngleAxis (-r, Vector3.forward);
	}
}
