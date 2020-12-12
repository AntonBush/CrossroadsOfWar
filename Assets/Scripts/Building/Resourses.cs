using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resourses : MonoBehaviour {

    [HideInInspector]
    public List<Item> SaveItems = new List<Item>();

    public WeaponBuilding weapon;

	public MovingController Player;
	public EarthponiesCamp camp;

	public int Wood;
	public int Food;
	public int Ponies;

	public int WoodMax;
	public int FoodMax = 0;
	public int PoniesMax = 4;

	public Text WoodText;
	public Text FoodText;
	public Text PoniesText;

    float timerStartCheckFood = 70f;
    bool onetimeStartCheck;

    IEnumerator checkFood;

    IEnumerator CheckFood()
    {
        for(; ; )
        {
            if(Food > 0)
            {
                AddResourses(0, -2);
            }
            else
            {
                Player.Hitted(2, true);
                for (int i = 0; i < camp.Ponies.Count; i++)
                {
                    camp.Ponies[i].Hitted(2, true);
                }
            }
            yield return new WaitForSeconds(30f);
        }
    }

	IEnumerator checkUnitsHealth;

	IEnumerator CheckUnitsHealth() 
	{
		for(;;)
		{
			if(Player.health > 0 && Player.health < 69 && Food > 9)
			{
				Player.health += 15;
                Player.gameManager.healsCount += 15;
				AddResourses(0,-10);
			}
			for(int i = 0; i < camp.Ponies.Count; i++)
			{
				if(camp.Ponies[i].health > 0 && camp.Ponies[i].health < 50 && Food > 9)
				{
					camp.Ponies[i].health += 15;
                    Player.gameManager.healsCount += 15;
                    AddResourses(0,-10);
				}
			}
			yield return new WaitForSeconds(2f);
		}
	}

	private void Start() {
		Ponies = 1;
		PoniesMax = 4;
		WoodMax = 15;
		UpdateResourses();
		checkUnitsHealth = CheckUnitsHealth();
		StartCoroutine(checkUnitsHealth);
	}

    private void Update()
    {
        if(timerStartCheckFood > 0)
        {
            timerStartCheckFood -= Time.deltaTime;
        }
        else
        {
            if (!onetimeStartCheck)
            {
                checkFood = CheckFood();
                StartCoroutine(checkFood);
                onetimeStartCheck = true;
            }
        }
    }


    public void UpdateResourses() 
	{
		WoodText.text = Wood.ToString() + "/" + WoodMax.ToString();
		FoodText.text = Food.ToString() + "/" + FoodMax.ToString();
		PoniesText.text = Ponies.ToString() + "/" + PoniesMax.ToString();
	}

	public void AddResourses(int wood, int food)
	{
        if (wood < 0) Player.gameManager.spendWoodCount += -wood;
        if (wood > 0) Player.gameManager.gotWoodCount += wood;
        if (food > 0) Player.gameManager.gotFoodCount += food;
		Wood += wood;
		Food += food;
		if(Wood < 0)
		{
			print("Древесина стала отрицательной, исправляем");
			Wood = 0;
		}
		UpdateResourses();
	}
}
