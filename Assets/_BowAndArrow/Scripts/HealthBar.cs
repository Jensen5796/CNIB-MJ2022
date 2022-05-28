using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	Image fill;
	//float health, maxhealth = 180f;
    float health, maxhealth;

    public void Awake()
    {
        maxhealth = GameTimer.getDurationOfRound();
    }
    public void Start()
    {
		health = maxhealth;
		fill = GetComponent<Image>();
    }
	public void Update()
	{
		//health -= Time.deltaTime;
		health = GameTimer.getElapsedTime();
		fill.fillAmount = health / maxhealth;
		if (health < 0) health = 0;
	}


}
