using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {

	public bool newPony;
	SpriteRenderer SR;
	MovingController myParent;

	void SetColor()
	{
		SR.color = new Color(Random.value,Random.value,Random.value);
	}

	void Start () {
		SR = GetComponent<SpriteRenderer>();
		SetColor();
	}

	private void Update() {
		if(newPony)
		{
			SetColor();
			newPony = false;
		}
	}
}
