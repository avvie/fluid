using UnityEngine;
using System.Collections;

public class DestroyerScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
	}
}
