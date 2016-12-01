using UnityEngine;
using System.Collections;

public class moveBystander : MonoBehaviour {
	public float speed;

	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
}
