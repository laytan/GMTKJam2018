﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {
    public int health;
    public int money;
    public GameObject slider;
    public GameObject dragging;
    public GameObject baseModule, machineGunModule, sniperModule;
    public GameObject moneyText;
    private bool holdingDownUp = false;
    private bool holdingDownDown = false;
    public float camSpeed;
    private GameObject cam;
    // Use this for initialization
    void Start () {
        slider.GetComponent<Slider>().value = health;
        slider.GetComponent<Slider>().maxValue = health;
        moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString();
        cam = Camera.main.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if(holdingDownDown)
        {
            if(cam.transform.position.y <= 3.7f)
            {
                return;
            }
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x,0, cam.transform.position.z), Time.deltaTime * camSpeed);
        }
        else if(holdingDownUp)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x, 100, cam.transform.position.z), Time.deltaTime * camSpeed);
        }
	}
    public void TakeDamage()
    {
        health--;
        slider.GetComponent<Slider>().value = health;

        if(health <= 0)
        {
            slider.transform.GetChild(1).gameObject.SetActive(false);
            Die();
        }
    }

    void Die()
    {
        float lastedTime = (float)System.Math.Round(Time.time, 1);
        //Set the time we lasted this run
        PlayerPrefs.SetFloat("lastedTime", lastedTime);
        //If there is already a highscore compare it to our score and change it if it's better,
        //Else we set the highscore to our score
        if(PlayerPrefs.HasKey("highscore"))
        {
            float highscore = PlayerPrefs.GetFloat("highscore");
            if (lastedTime > highscore)
            {
                PlayerPrefs.SetFloat("highscore", lastedTime);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("highscore", lastedTime);
        }

        SceneManager.LoadScene(2);
    }

    public bool TakeMoney(int cost)
    {
        if(cost > money)
        { return false; }
        else
        {
            money -= cost;
            moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString();
            return true;
        }
    }
    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString();
    }

    public void SelectMachineGun()
    {
        dragging.GetComponent<Dragging>().DragThis(machineGunModule);
    }
    public void SelectModule()
    {
        dragging.GetComponent<Dragging>().DragThis(baseModule);
    }
    public void SelectSniper()
    {
        dragging.GetComponent<Dragging>().DragThis(sniperModule);
    }

    public void OnUpHoldBegin()
    {
        holdingDownUp = true;
    }
    public void OnUpHoldEnd()
    {
        holdingDownUp = false;
    }
    public void OnDownHoldBegin()
    {
        holdingDownDown = true;
    }
    public void OnDownHoldEnd()
    {
        holdingDownDown = false;
    }
}
