using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBehaviour : MonoBehaviour {

	Vector3 pos;
	float ang;
	float zRotation;
	//BallBehaviour ballScript;
	Transform GearObstacle;
	bool autoMove = false;
	//bool isMoving = true;
	//bool isClicked = false;
	bool fadeAwayInstruction = false;
	float alphaLevel = 1f;

	bool soundPlayed = false;

	float leftMaxAng;
	float rightMaxAng;
	float initX;
	Vector3 minPos;
	Vector3 maxPos;
	float fraction;

	void Start () {
		//ballScript = GameObject.FindObjectOfType (typeof(BallBehaviour)) as BallBehaviour;
		GearObstacle = transform.root.Find ("Gear obstacle");
		initX = transform.position.x;
		if (initX > 0) {
			leftMaxAng = -150;
			rightMaxAng = 90;
		} else {
			leftMaxAng = -90;
			rightMaxAng = 150;
		}
		zRotation = transform.rotation.eulerAngles.z;
	//	minPos = new Vector3(min, 0, puncher.position.z); 
	//	maxPos = new Vector3(max, 0, puncher.position.z);


	}

	void Update () {

		//determine if game is stopped or paused
		//isMoving = !ballScript.getStopMovementFlag () && !ballScript.getGamePausedFlag ();

	//	fraction = Mathf.Sin(Time.time * puncherSpeed);
	//	puncher.localPosition = Vector3.Lerp (minPos, maxPos, fraction);

		if (autoMove && GameManager.IsBallFalling()) {
			if (ang > 0f ) {
				if (ang > rightMaxAng - 5f && ang < rightMaxAng + 5f) {
					ang = rightMaxAng;
					autoMove = false;
				} else {
					if (ang < rightMaxAng)
						ang += Time.deltaTime * 300f;
					else
						ang -= Time.deltaTime * 300f;
				}
			} else {
				if (ang < leftMaxAng + 5f && ang > leftMaxAng - 5f) {
					ang = leftMaxAng;
					autoMove = false;
				}
				else {
					if (ang < leftMaxAng) {
						ang += Time.deltaTime * 300f;
					} else {
						ang -= Time.deltaTime * 300f;
					}
				}
			}
				
			transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
			GearObstacle.rotation = Quaternion.AngleAxis (ang, Vector3.back);


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

			ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg + 90f;
			if (ang > 180f)
				ang = ang - 360f;

			if ( Mathf.Abs(ang) > Mathf.Abs(zRotation) + 10f || Mathf.Abs(ang) < Mathf.Abs(zRotation) - 10f) {
				autoMove = true;
				//print ("automove");
			}

			transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
			GearObstacle.rotation = Quaternion.AngleAxis (ang, Vector3.back);
		}

		if (transform.root.Find ("Instruction").gameObject.activeSelf) {
			fadeAwayInstruction = true;
		}

	}
}
