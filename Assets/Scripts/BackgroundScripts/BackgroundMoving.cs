using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour {

	public Transform player;
	public float speed = 8.5f;
	public float zeroPosition;

	void Update () {
		float newX = zeroPosition - player.position.x / speed;
		transform.position = new Vector3(newX,transform.position.y,transform.position.z);
	}
}
