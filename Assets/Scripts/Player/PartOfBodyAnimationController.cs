using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBodyAnimationController : MonoBehaviour {

	public MovingController origin;
	Animator anim;

	private void Start() {
		anim = GetComponent<Animator>();
	}

	private void Update() {
		anim.SetFloat("speed",Mathf.Abs(origin.speedX));
	}
}
