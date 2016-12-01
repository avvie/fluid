using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	public GameObject FPSController;
	GameObject fpsContnstance;
	// Use this for initialization
	void Awake () {
		fpsContnstance = Instantiate(FPSController, transform.position, transform.rotation) as GameObject;
		fpsContnstance.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (fpsContnstance == null) {
			Instantiate(FPSController, transform.position, transform.rotation);
		}
	}
}
