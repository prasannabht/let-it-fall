using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaterBehaviour : MonoBehaviour {

	Vector3 pos;
	float ang;
	float initAngle;
	bool autoMove = false;

	bool fadeAwayInstruction = false;
	float alphaLevel = 1f;

	bool soundPlayed = false;
	int nextStop;
	bool isNextStopDefined = false;


	void Start () {
		initAngle = transform.rotation.eulerAngles.z;
	}


	void Update(){

		//determine if game is stopped or paused
		//isMoving = !ballScript.getStopMovementFlag () && !ballScript.getGamePausedFlag ();

		if (autoMove && GameManager.IsBallFalling()) {

			if (!isNextStopDefined) {
				//print ("currAng: " + ang);
				nextStop = CommonFunctions.FindNextStop (initAngle, ang);
				isNextStopDefined = true;
				//print ("Go To: " + nextStop);
			}

			if (ang < nextStop) {
				if (ang > nextStop - 5f) {
					ang = nextStop;
					autoMove = false;
					initAngle = ang;
					if (initAngle == 360)
						initAngle = 0;
				} else
					ang += Time.deltaTime * 300f;
			} else {
				if (ang < nextStop + 5f) {
					ang = nextStop;
					autoMove = false;
					initAngle = ang;
					if (initAngle == 360)
						initAngle = 0;
				} else
					ang -= Time.deltaTime * 300f;
			}

			transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);

		}

		if (fadeAwayInstruction && GameManager.IsBallFalling()) {
			if (alphaLevel > 0.0f) {
				alphaLevel -= Time.deltaTime * 5;
				transform.root.Find ("Instruction").gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, alphaLevel);
			}

			if (alphaLevel <= 0f) {
				transform.root.Find ("Instruction").gameObject.SetActive(false);
				fadeAwayInstruction = false;
			}
		}
	}

	void OnMouseDrag () {
		
		if (GameManager.IsBallFalling()) {

			//play sound
			if (!soundPlayed) {
				//GetComponent<AudioSource> ().Play();
				FindObjectOfType<AudioManager>().Play("Rotater");
				soundPlayed = true;
			}

			pos = Camera.main.WorldToScreenPoint (transform.position);
			pos = Input.mousePosition - pos;

			ang = CommonFunctions.GetAngle (pos);
			
			if ( Mathf.Abs(ang) > Mathf.Abs(initAngle) + 10f || Mathf.Abs(ang) < Mathf.Abs(initAngle) - 10f) {
				autoMove = true;
				isNextStopDefined = false;
			}
			
			transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
		}

		if (!transform.root.gameObject.name.Contains("Fake")) {
			if (transform.root.Find ("Instruction").gameObject.activeSelf) {
				fadeAwayInstruction = true;
			}
		}

	}
		
}
