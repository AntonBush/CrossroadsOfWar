using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMove : MonoBehaviour {
	public GameObject particlePrefab;
	public Sprite[] sprites;
	public float minY;
	public float maxY;
	public float minX;
	public float maxX;

	public float radius_left;
	public float radius_right;
	public float minSpeed;
	public float maxSpeed;
	public bool random_side;
	public float maxParticlesCount;
	[SerializeField]
	public List<Particle> particles = new List<Particle>();

    float timer;
	float tempCount;

	void CreateParticle() {
		GameObject newObject = PoolManager.getGameObjectFromPool(particlePrefab);
		newObject.transform.parent = this.transform;
		newObject.GetComponent<Particle>().mySprite = sprites[Random.Range(0,sprites.Length)];
		newObject.GetComponent<Particle>().speed = Random.Range(minSpeed,maxSpeed);
		if(random_side) {
			if(Random.value > 0.5f) {
			newObject.GetComponent<Particle>().startPosition = new Vector2(Random.Range(minX-radius_left,minX + radius_right),Random.Range(minY,maxY));
			newObject.GetComponent<Particle>().endPosition = new Vector2(Random.Range(maxX-radius_left,maxX + radius_right),Random.Range(minY,maxY));
			}
			else {
			newObject.GetComponent<Particle>().startPosition = new Vector2(Random.Range(maxX-radius_left,maxX + radius_right),Random.Range(minY,maxY));
			newObject.GetComponent<Particle>().endPosition = new Vector2(Random.Range(minX-radius_left,minX + radius_right),Random.Range(minY,maxY));
			}
		}
		else {
			newObject.GetComponent<Particle>().startPosition = new Vector2(Random.Range(minX-radius_left,minX + radius_left),Random.Range(minY,maxY));
			newObject.GetComponent<Particle>().endPosition = new Vector2(Random.Range(maxX-radius_left,maxX + radius_right),Random.Range(minY,maxY));
		}
		newObject.GetComponent<Particle>().myParent = this;
		particles.Add(newObject.GetComponent<Particle>());
	}

	private void Update() {
		if(timer > 0)
		{
			timer -= Time.deltaTime;
			tempCount = 0;
		}
		else 
		{
			if(tempCount == 0) tempCount = Random.Range(1,maxParticlesCount);
			if(particles.Count < tempCount) 
				{
					CreateParticle();
				}
				else 
				{
					timer = Random.Range(1f,10f);
				}
		}
	}
}
