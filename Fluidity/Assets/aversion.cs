using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aversion : MonoBehaviour {
	float timer = 0;
	Camera cam;
	RaycastHit[] hit;

	[Tooltip("The Speed at which the camera is averted")]
	public float speedOfAversion = 1;
	[Tooltip("Interval before aversion")]
	public float timeToAversion = 1;
	[Tooltip("Tag that triggers averson")]
	public string aversionTag;
	[Tooltip("Aversion Distance, 0 if infinite")]
	public float aversionDistance = 8;
	[Tooltip("Aversion thickness")]
	public float aversionRadius= 1;
	[Tooltip("Layere Name, not required")]
	public string layerName= "AversionLayer";
	[Tooltip("Show Debug info")]
	public bool dbg= false;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		//throw sphere cast, so we know where we came from
		hit = Physics.SphereCastAll (cam.transform.position, aversionRadius, cam.transform.forward, aversionDistance > 0? aversionDistance : Mathf.Infinity, 1 << LayerMask.NameToLayer (layerName));
		//if hit
		if (hit != null) {
			//for all collisions on appropriete layer
			foreach (RaycastHit tmp in hit) {
				//check the object tag is correct
				if (tmp.transform.tag == aversionTag) {
					Debug.Log ("Gotcha");
					//Start COuntiing
					timer += Time.deltaTime;
					//if >timetoaversion
					if (timer >= timeToAversion) {
						//move camera off
					}
				}
			}
		} else {
			if(timer >= 0)
				timer -= Time.deltaTime;
			hit = null;
		}
	}

	//DEBUGGGGGGGgggggg
	float p2,p1;
	void OnDrawGizmos() {
		if (dbg) {
			if (hit != null) {
				foreach (RaycastHit tmp in hit) {
					Gizmos.color = Color.yellow;
					float dist = Vector3.Cross (cam.transform.forward, tmp.point - cam.transform.position).magnitude;
					p1 = ((Vector3.Dot (cam.transform.position, cam.transform.forward) * -2) +
					          Mathf.Sqrt (Mathf.Pow (2 * Vector3.Dot (cam.transform.position, cam.transform.forward), 2) - 4 * Vector3.Dot (cam.transform.forward, cam.transform.forward) * (Mathf.Pow (dist, 2) - Mathf.Pow (aversionRadius, 2))))
					          / (2 * Vector3.Dot (cam.transform.forward, cam.transform.forward));
					p2 = ((Vector3.Dot (cam.transform.position, cam.transform.forward) * -2) -
					          Mathf.Sqrt (Mathf.Pow (2 * Vector3.Dot (cam.transform.position, cam.transform.forward), 2) - 4 * Vector3.Dot (cam.transform.forward, cam.transform.forward) * (Mathf.Pow (dist, 2) - Mathf.Pow (aversionRadius, 2))))
					          / (2 * Vector3.Dot (cam.transform.forward, cam.transform.forward));

					Gizmos.DrawSphere (cam.transform.position + (Mathf.Abs(p1) < Mathf.Abs(p2) ? p1 : p2) * cam.transform.forward, aversionRadius);

				}
			} 
		}
	}

	void OnGUI(){
		if (dbg && hit != null) {
			GUI.Label (new Rect (10, 10, 300, 20), "p1: " + p1 + "  p2:" + p2);
			GUI.Label (new Rect (10, 25, 100, 20), (cam.transform.position + (Mathf.Abs(p1) < Mathf.Abs(p2) ? p1 : p2) * cam.transform.forward).ToString());
		}
	}
}
