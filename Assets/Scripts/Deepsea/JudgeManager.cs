using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    public static JudgeManager instance { get; private set; }

    private bool enemyWin = false;
    private bool playerWin = false;
    private bool setEnd = false;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // player win
        if(playerWin && !enemyWin)
        {

        } else if(enemyWin && !playerWin) // enemy win
        {

        }
    }

    public void setEnemyWin()
    {
        if(!enemyWin && !playerWin)
        {
            enemyWin = true;
        }
        
    }

    public void setPlayerWin()
    {
        if(!enemyWin && !playerWin)
        {
            playerWin = true;
        }
    }

}
