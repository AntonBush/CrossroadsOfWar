using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveLoadGame : MonoBehaviour {

    [Header("Прогресс")]
    public MusicScript music;
    [Header("Время")]
    public TimeCount time;
    public SunMoving sunMov;
    public WeatherControl weather;
    [Header("Существа")]
    public MovingController Player;
    public UnicornsSpawn UniSpawn;
    public EarthponiesCamp MainCamp;
    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public UrsaMinor Ursa;
    [Header("Строительство")]
    public GameObject ItemPrefab;
    public Resourses res;
    public MainFire mainFire;
    public WarehouseBuild warehouse;
    public WeaponBuilding weapon;
    public FarmBuild farm;
    public HousesBuild houses;
    public TowerBuild LeftTower;
    public TowerBuild RightTower;
    public WallBuild LeftWall;
    public WallBuild RightWall;
    public TreeBuild[] Trees;

    //-----------------Saving-----------------

    void SaveColor(string colorName, Color color)
    {
        Ini.Set(colorName + "_red", color.r.ToString());
        Ini.Set(colorName + "_green", color.g.ToString());
        Ini.Set(colorName + "_blue", color.b.ToString());
    }

    void SavePonyVisual(RandomSex pony, string ponyName)
    {
        if (pony.playerMare)
            Ini.Set(ponyName + "_mare", "1");
        else
            Ini.Set(ponyName + "_mare", "0");

        if (pony.Beard.activeSelf)
            Ini.Set(ponyName + "_beard", "1");
        else
            Ini.Set(ponyName + "_beard", "0");

        Ini.Set(ponyName + "_hair", pony.randomHair.ToString());

        SaveColor(ponyName + "_hairColor", pony.HairAnim.GetComponent<SpriteRenderer>().color);
        SaveColor(ponyName + "_bodyColor", pony.GetComponent<SpriteRenderer>().color);
        SaveColor(ponyName + "_flagColor", pony.anims[1].GetComponent<SpriteRenderer>().color);
        SaveColor(ponyName + "_eyesColor", pony.anims[2].GetComponent<SpriteRenderer>().color);
    }

    void SavePony(string PonyName, Creature Pony, bool saveTransform)
    {
        Ini.AddHeader(PonyName);
        Ini.Set(PonyName  + "_health", Pony.health.ToString());

        if (saveTransform)
        {
            Ini.Set(PonyName + "_x", Pony.transform.position.x.ToString());
            Ini.Set(PonyName + "_y", Pony.transform.position.y.ToString());
            Ini.Set(PonyName + "_z", Pony.transform.position.z.ToString());
        }

        SavePonyVisual(Pony.GetComponent<RandomSex>(), PonyName);
    }

    void SaveMainCampPonies()
    {
        SavePony("Player", Player, true);
        if (Player.GetComponent<MovingController>().hasBow)
            Ini.Set("Player_hasBow", "1");
        else
            Ini.Set("Player_hasBow", "0");

        float tempPoniesCount = MainCamp.Ponies.Count;
        if (weapon.worker != null) tempPoniesCount++; //работников нужно сохранять отдельно, т.к. они не в списке лагерей
        if (farm.worker != null) tempPoniesCount++; //можно типа сохранить их как работников, чтоб они при загрузке оставались
        Ini.Set("PoniesCount", tempPoniesCount.ToString()); //но сложна с:
        for (int i = 0; i < tempPoniesCount; i++)
        {
            if (i < MainCamp.Ponies.Count) //если мы еще не прошлись по списку свободных пней
                SavePony("CampPony_" + i, MainCamp.Ponies[i], false);
            else
            {
                if (weapon.worker != null && farm.worker != null) //если работник есть и на ферме, и в оружейной
                { //сохраняем их по очереди
                    if (i < tempPoniesCount - 1)
                        SavePony("CampPony_" + i, weapon.worker, false);
                    if(i < tempPoniesCount)
                        SavePony("CampPony_" + i, farm.worker, false);
                }
                else
                { //иначе просто  проходим по тому, кто есть - тут либо один из них активен, либо нет
                    if(weapon.worker != null)
                    {
                        SavePony("CampPony_" + i, weapon.worker, false);
                    }
                    if(farm.worker != null)
                    {
                        SavePony("CampPony_" + i, farm.worker, false);
                    }
                }
            }
        }

        float tempHuntersCount = MainCamp.Hunters.Count;
        if (LeftTower.myHunter != null) tempHuntersCount++;
        if (RightTower.myHunter != null) tempHuntersCount++;

        Ini.Set("HuntersCount", tempHuntersCount.ToString());
        for (int i = 0; i < tempHuntersCount; i++)
        {
            if(i < MainCamp.Hunters.Count)
            SavePony("Hunter_" + i, MainCamp.Hunters[i], false);
            else
            {
                if(LeftTower.myHunter != null && RightTower.myHunter != null)
                {
                    if(i < tempHuntersCount - 1)
                    {
                        SavePony("Hunter_" + i, LeftTower.myHunter, false);
                    }
                    if (i < tempHuntersCount)
                    {
                        SavePony("Hunter_" + i, RightTower.myHunter, false);
                    }
                }
                else
                {
                    if(LeftTower.myHunter != null) SavePony("Hunter_" + i, LeftTower.myHunter, false);
                    if(RightTower.myHunter != null) SavePony("Hunter_" + i, RightTower.myHunter, false);
                }
            }
        }
    }

    void SaveCreatures()
    {
        SaveMainCampPonies();

        Ini.AddHeader("OtherCreatures");
        Ini.Set("UniSpawn_health", UniSpawn.health.ToString());
        Ini.Set("Timber1_health", Timberwolf1.health.ToString());
        Ini.Set("Timber2_health", Timberwolf2.health.ToString());
        Ini.Set("Ursa_health", Ursa.health.ToString());
    }

    void SaveBuilding()
    {
        Ini.AddHeader("Resourses");
        Ini.Set("Wood", res.Wood.ToString());
        Ini.Set("Food", res.Food.ToString());
        //Ini.Set("Ponies", res.Ponies.ToString());
        Ini.AddHeader("Items");
        Ini.Set("ItemsCount", res.SaveItems.Count.ToString());
        for(int i = 0; i < res.SaveItems.Count; i++)
        {
            Ini.Set("Item_" + i + "_x", res.SaveItems[i].transform.position.x.ToString());
            Ini.Set("Item_" + i + "_wood", res.SaveItems[i].woodCount.ToString());
            Ini.Set("Item_" + i + "_food", res.SaveItems[i].foodCount.ToString());
            Ini.Set("Item_" + i + "_bow", res.SaveItems[i].bowCount.ToString());
        }

        Ini.AddHeader("Buildings");
        Ini.Set("FireLevel", mainFire.buildingLevel.ToString());
        Ini.Set("Warehouse", warehouse.buildingLevel.ToString());
        Ini.Set("Weapon", weapon.buildingLevel.ToString());
        Ini.Set("Farm", farm.buildingLevel.ToString());
        Ini.Set("Houses", houses.buildingLevel.ToString());
        Ini.Set("LeftTower", LeftTower.buildingLevel.ToString());
        Ini.Set("RightTower", RightTower.buildingLevel.ToString());
        Ini.Set("LeftWall", LeftWall.buildingLevel.ToString());
        Ini.Set("LeftWall_health", LeftWall.health.ToString());
        Ini.Set("RightWall", RightWall.buildingLevel.ToString());
        Ini.Set("RightWall_health", RightWall.health.ToString());

        for(int i = 0; i < Trees.Length; i++)
        {
            if (Trees[i].gameObject.activeSelf) Ini.Set("Tree_" + i, "1");
            else Ini.Set("Tree_" + i, "0");
        }
    }

    void SaveWeather()
    {
        Ini.Set("weather",weather.weatherNumber.ToString());
        Ini.Set("Sun_x", sunMov.sun.position.x.ToString());
        Ini.Set("Sun_y", sunMov.sun.position.y.ToString());
        //SaveColor("sun", sunMov.SunColor);
        //SaveColor("sky", sunMov.SkyColor);
    }

    void SaveProgress()
    {
        Ini.AddHeader("Music");
        if(music.Lv0played) Ini.Set("Music_Lvo", "1");
        else Ini.Set("Music_Lvo", "0");
        if (music.Lv2played) Ini.Set("Music_Lv2", "1");
        else Ini.Set("Music_Lv2", "0");
        if (music.Lv3played) Ini.Set("Music_Lv3", "1");
        else Ini.Set("Music_Lv3", "0");
        if (music.FightTrackPlayed) Ini.Set("Music_Fight", "1");
        else Ini.Set("Music_Fight", "0");
        if (music.SadTrackPlayed) Ini.Set("Music_Sad", "1");
        else Ini.Set("Music_Sad", "0");
    }

    public void SaveGame()
    {
        Ini.ClearValues();
        Ini.AddHeader("Hello! This is your save game. Please dont move something c:");
        Ini.Set("Hours", time.hours.ToString());
        Ini.Set("Minutes", time.minutes.ToString());
        Ini.Set("Days", time.days.ToString());

        SaveCreatures();
        SaveBuilding();
        SaveWeather();
        SaveProgress();

        Ini.Save("Save.sv");
    }

    //-------------Loading-------------------

    public static Color LoadColor(string colorName)
    {
        float red = float.Parse(Ini.Get(colorName + "_red"));
        float green = float.Parse(Ini.Get(colorName + "_green"));
        float blue = float.Parse(Ini.Get(colorName + "_blue"));

        return new Color(red, green, blue);
    }

    void LoadPonyVisual(RandomSex pony, string ponyName)
    {
        pony.enabled = false;

        pony.playerMare = Ini.Get(ponyName + "_mare") == "1";
        pony.Beard.SetActive(Ini.Get(ponyName + "_beard") == "1");
        pony.randomHair = Convert.ToInt32(Ini.Get(ponyName + "_hair"));

        pony.HairAnim.GetComponent<SpriteRenderer>().color = LoadColor(ponyName + "_hairColor");
        pony.GetComponent<SpriteRenderer>().color = LoadColor(ponyName + "_bodyColor");
        pony.anims[1].GetComponent<SpriteRenderer>().color = LoadColor(ponyName + "_flagColor");
        pony.anims[2].GetComponent<SpriteRenderer>().color = LoadColor(ponyName + "_eyesColor");

        pony.SetNewValues();
    }

    void LoadPony(string PonyName, Creature Pony, bool loadTransform)
    {
        Pony.health = Convert.ToInt32(Ini.Get(PonyName + "_health"));
        LoadPonyVisual(Pony.GetComponent<RandomSex>(), PonyName);
        if (loadTransform)
        {
            float PonyX = float.Parse(Ini.Get(PonyName + "_x"));
            float PonyY = float.Parse(Ini.Get(PonyName + "_y"));
            float PonyZ = float.Parse(Ini.Get(PonyName + "_z"));
            Pony.transform.position = new Vector3(PonyX, PonyY, PonyZ);
        }
    }

    void LoadMainCampPonies()
    {
        LoadPony("Player", Player, true);
        Player.GetComponent<MovingController>().hasBow = Ini.Get("Player_hasBow") == "1";
        if (Player.hasBow) weapon.playerHasBow = true;

        float poniesCount = Convert.ToInt32(Ini.Get("PoniesCount"));

        for (int i = 0; i < poniesCount; i++)
        {
            GameObject newPony = MainCamp.SpawnNewPony(Convert.ToInt32(Ini.Get("CampPony_" + i + "_health")));
            LoadPony("CampPony_" + i, newPony.GetComponent<Creature>(), false);
        }

        float huntersCount = Convert.ToInt32(Ini.Get("HuntersCount"));

        for(int i = 0; i < huntersCount; i++)
        {
            GameObject newHunter = MainCamp.SpawnNewHunter(Convert.ToInt32(Ini.Get("Hunter_" + i + "_health")));
            LoadPony("Hunter_" + i, newHunter.GetComponent<Creature>(), false);
        }
    }

    void LoadCreatures()
    {
        LoadMainCampPonies();

        UniSpawn.health = Convert.ToInt32(Ini.Get("UniSpawn_health"));
        if (UniSpawn.health <= 0) UniSpawn.gameObject.SetActive(false);

        Timberwolf1.health = Convert.ToInt32(Ini.Get("Timber1_health"));
        if (Timberwolf1.health <= 0) Timberwolf1.gameObject.SetActive(false);

        Timberwolf2.health = Convert.ToInt32(Ini.Get("Timber2_health"));
        if (Timberwolf2.health <= 0) Timberwolf2.gameObject.SetActive(false);

        Ursa.health = Convert.ToInt32(Ini.Get("Ursa_health"));
        if (Ursa.health <= 0) Ursa.gameObject.SetActive(false);
    }

    void LoadBuilding()
    {
        res.Wood = Convert.ToInt32(Ini.Get("Wood"));
        res.Food = Convert.ToInt32(Ini.Get("Food"));
        //res.Ponies = Convert.ToInt32(Ini.Get("Ponies"));
        res.UpdateResourses();

        int itemsCount = Convert.ToInt32(Ini.Get("ItemsCount"));

        for (int i = 0; i < itemsCount; i++)
        {
            Item newItem = PoolManager.getGameObjectFromPool(ItemPrefab).GetComponent<Item>();
            newItem.player = Player.transform;
            float itemPosX = float.Parse(Ini.Get("Item_" + i + "_x"));
            newItem.transform.position = new Vector2(itemPosX, -12.9f);
            newItem.woodCount = Convert.ToInt32(Ini.Get("Item_" + i + "_wood"));
            newItem.foodCount = Convert.ToInt32(Ini.Get("Item_" + i + "_food"));
            newItem.bowCount = Convert.ToInt32(Ini.Get("Item_" + i + "_bow"));
            

            if (newItem.woodCount > 0) newItem.GetComponent<SpriteRenderer>().sprite = newItem.woodSprite;
            else if(newItem.foodCount > 0) newItem.GetComponent<SpriteRenderer>().sprite = newItem.foodSprite;
            else if (newItem.bowCount > 0) newItem.GetComponent<SpriteRenderer>().sprite = newItem.foodSprite;
            else
            {
                Debug.Log("Че за херня, почему итем сохранился пустой?");
                PoolManager.putGameObjectToPool(newItem.gameObject);
                return;
            }

            warehouse.resourses.SaveItems.Add(newItem);
        } 

        mainFire.buildingLevel = Convert.ToInt32(Ini.Get("FireLevel"));
        warehouse.buildingLevel = Convert.ToInt32(Ini.Get("Warehouse"));
        weapon.buildingLevel = Convert.ToInt32(Ini.Get("Weapon"));
        farm.buildingLevel = Convert.ToInt32(Ini.Get("Farm"));
        houses.buildingLevel = Convert.ToInt32(Ini.Get("Houses"));
        LeftTower.buildingLevel = Convert.ToInt32(Ini.Get("LeftTower"));
        RightTower.buildingLevel = Convert.ToInt32(Ini.Get("RightTower"));
        LeftWall.buildingLevel = Convert.ToInt32(Ini.Get("LeftWall"));
        LeftWall.health = Convert.ToInt32(Ini.Get("LeftWall_health"));
        RightWall.buildingLevel = Convert.ToInt32(Ini.Get("RightWall"));
        RightWall.health = Convert.ToInt32(Ini.Get("RightWall_health"));

        for (int i = 0; i < Trees.Length; i++)
        {
            Trees[i].gameObject.SetActive(Ini.Get("Tree_" + i) ==  "1");
            if (!Trees[i].gameObject.activeSelf)
            {
                if (Trees[i].leftTree)
                {
                    Trees[i].squirrels.LeftTrees.Remove(Trees[i].transform);
                }
                else
                {
                    Trees[i].squirrels.RightTrees.Remove(Trees[i].transform);
                }

                Trees[i].crown.LandingPositions.Remove(Trees[i]);
                Trees[i].myForest.myTrees--;
            }
        }
    }

    void LoadWeather()
    {
        weather.weatherNumber = Convert.ToInt32(Ini.Get("weather"));
        weather.changeWeather = true;
        float sunX = float.Parse(Ini.Get("Sun_x"));
        float sunY = float.Parse(Ini.Get("Sun_y"));
        sunMov.sun.position = new Vector3(sunX, sunY, sunMov.sun.position.z);
       // sunMov.SunColor = LoadColor("sun");
       // sunMov.SunColor = LoadColor("sky");
    }

    void LoadProgress()
    {
        music.Lv0played = Ini.Get("Music_Lvo") ==  "1";
        music.Lv2played = Ini.Get("Music_Lv2") == "1";
        music.Lv3played = Ini.Get("Music_Lv3") == "1";
        music.FightTrackPlayed = Ini.Get("Music_Fight") == "1";
        music.SadTrackPlayed = Ini.Get("Music_Sad") == "1";

    }

    public void LoadGame()
    {
        if (Ini.FileExists("Save.sv"))
        {
            Ini.LoadFile("Save.sv");
            time.hours = Convert.ToInt32(Ini.Get("Hours"));
            time.minutes = Convert.ToInt32(Ini.Get("Minutes"));
            time.days = Convert.ToInt32(Ini.Get("Days"));
            LoadCreatures();
            LoadBuilding();
            LoadWeather();
            LoadProgress();
        }
        else
        {
            Debug.Log("Верни файл мазафака");
        }
    }

}
