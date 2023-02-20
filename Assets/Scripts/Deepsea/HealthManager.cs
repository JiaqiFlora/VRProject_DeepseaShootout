using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public static HealthManager instance { get; private set; }

    public int maxHealth = 3;
    public int currentHealth = 3;
    public List<GameObject> heartsList = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void SetMaxHealth(int health)
    {
        this.maxHealth = health;

    }


    public void SetHealth(int health)
    {
        currentHealth = health;
        

    }

    public void DecreaseHealth()
    {
        if(currentHealth <= 0)
        {
            return;
        }

        SetHealth(currentHealth - 1);

        // remove heart symbol
        GameObject lastHeat = heartsList[heartsList.Count - 1];
        lastHeat.SetActive(false);
        heartsList.Remove(lastHeat);
    }



}
