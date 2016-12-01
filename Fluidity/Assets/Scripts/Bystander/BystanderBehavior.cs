using UnityEngine;
using System.Collections;

public class BystanderBehavior : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy(){
		transform.parent.SendMessage("Death");
	}
}
