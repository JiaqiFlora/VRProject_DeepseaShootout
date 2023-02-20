using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerColliderTrigger : MonoBehaviour
{

    public AudioSource hitSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // here to control the enemy's bullet hit player
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ganjiaqi: --- enter into player!!!, thing is: " + other.gameObject + " tag is: " + other.tag);
        if(other.tag == "bullet")
        {
            hitSound.Play();
            JudgeManager.instance.setEnemyWin();

            TimingManager.instance.EndCurLevel(true);
        }
    }

}
