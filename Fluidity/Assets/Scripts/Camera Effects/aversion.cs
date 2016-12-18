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
	public bool dbg= false, DoXRot = false;

	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController target;
	private bool msgSent = false;

	//Math3d MHelper = new Math3d();

	void Awake() {
		target = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ();

	}

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}


	Quaternion rotx, roty;
	// Update is called once per frame
	void Update () {
		//throw sphere cast, so we know where we came from
		hit = Physics.SphereCastAll (cam.transform.position, aversionRadius, cam.transform.forward, aversionDistance > 0? aversionDistance : Mathf.Infinity, 1 << LayerMask.NameToLayer (layerName));
		//if hit
		if (hit.Length > 0) {
			//for all collisions on appropriete layer
			foreach (RaycastHit tmp in hit) {
				//check the object tag is correct
				if (tmp.transform.tag == aversionTag) {
					Debug.Log ("Gotcha");
					//Start COuntiing
					timer += Time.deltaTime;
					//if >timetoaversion
					if (timer >= timeToAversion) {
						if (!msgSent) {
							target.SendMessage ("setRotationLock", true);
							msgSent = true;
						}
						//project the center of collider on the center line of the camera, to get a point on that linee
						Vector3 projectionPoint = Vector3.Project ((tmp.collider.bounds.center - cam.transform.position),((cam.transform.position + 11 * cam.transform.forward) - cam.transform.position )) + cam.transform.position;
						// collider center to camera line vector
						Vector3 aversionVector = projectionPoint - tmp.collider.bounds.center;

						//move camera away
						Vector3 interpolatinPoint = Vector3.Lerp(projectionPoint, projectionPoint + aversionVector, speedOfAversion * Time.deltaTime);

						interpolatinPoint = interpolatinPoint - cam.transform.position;
						//calculates the signed angle between us and the interpolation point
						float Angler = Math3d.SignedVectorAngle (transform.forward, interpolatinPoint, transform.up);
						//set up the rotation
						Quaternion targetRot;
						targetRot = transform.localRotation * Quaternion.Euler (0, Angler, 0);
						//smoooOOoth it 
						transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRot,
							speedOfAversion * Time.deltaTime);

						//does the rotation for the camera X axis
						if (DoXRot) {
							Angler = Math3d.SignedVectorAngle (cam.transform.up, interpolatinPoint, cam.transform.forward);
							targetRot = cam.transform.localRotation * Quaternion.Euler (Angler, 0, 0);

							cam.transform.localRotation = Quaternion.Slerp (cam.transform.localRotation, targetRot,
								speedOfAversion * Time.deltaTime);
						}

						//this is to update the mouseLook memory of unity, so it wont reset my changes
						target.SendMessage("UpdateRotas", new Transform[] { transform, cam.transform});

						Debug.Log (targetRot.ToString());
						//debug info
						if (dbg) {
							Debug.DrawLine (projectionPoint, projectionPoint + 3 * aversionVector, Color.blue);
						}
					}
				}
			}
		} else {
			if (msgSent) {
				target.SendMessage ("setRotationLock", false);
				msgSent = false;
			}
			if(timer >= 0)
				timer -= Time.deltaTime;
			hit = null;
		}
		//Debug.Log (timer);
	}

	//DEBUGGGGGGGgggggg
	void OnDrawGizmos() {
		if (dbg) {
			if (hit != null) {
				foreach (RaycastHit tmp in hit) {
					Gizmos.color = Color.red;
					Gizmos.DrawSphere (cam.transform.position + cam.transform.forward * tmp.distance, aversionRadius);
				}
			} 
		}
	}

	/*
	void OnGUI(){
		GUI.Label(new Rect(10, 10, 300, 20), "rotX:" + rotx.ToString());
		GUI.Label(new Rect(10, 35, 300, 20), "rotY:" + roty.ToString());
		GUI.Label(new Rect(10, 55, 300, 20), "rotY:" + transform.up.ToString());
	}
	/*
}
