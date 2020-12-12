using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour {

	public Slider healthSlider;
	public MovingController player;

	private void Update() {
		healthSlider.value = player.health;
	}
}
