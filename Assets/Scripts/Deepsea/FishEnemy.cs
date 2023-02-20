using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemy : MonoBehaviour
{
    public float turnAroundTotalTime;
    public float drawingTotalTime;
    public Animator animator;

    [Header("Shoot")]
    public GameObject gun;
    public GameObject bullet;
    public float bulletSpeed;
    public Transform target;
    public Transform spawnPoint;
    public AudioSource dieSound;
   
    private GameObject bulletObj;
    private float startTurnTime = 0.0f;
    private bool canTurnAround   = false;
    private bool isDead = false;


    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTurnTime;
        if (canTurnAround && elapsedTime < turnAroundTotalTime)
        {
            float currentAngle = 180 - 180 * elapsedTime / turnAroundTotalTime;
            transform.eulerAngles = new Vector3(0, currentAngle, 0); ;
        }
    }

    private void OnDisable()
    {
        resetEnemy();
    }

    private void OnEnable()
    {
        resetEnemy();
    }

    // player's bullet hit enemy
    private void OnTriggerEnter(Collider other)
    {
        JudgeManager.instance.setPlayerWin();

        if(!isDead)
        {
            isDead = true;

            // step 1: trigger die animation and audio
            animator.SetTrigger("die");
            dieSound.Play();

            // step 2: destroy enemy's bullue (make it not shoot player!)

            // if enemy already shot a bullet
            if (bulletObj != null)
            {
                bulletObj.SetActive(false);
                Destroy(bulletObj);
                bulletObj = null;
            }

            // setp 3: player enter into next level
            TimingManager.instance.EndCurLevel(false);
        }
    }

    // trigger enemy start process
    public void OnTriggerStart()
    {
        // step 1: turn around
        startTurnTime = Time.time;
        canTurnAround = true;

        // step 2: draw gun + step 3: shooting
        StartCoroutine(Shoot());
    }

    private void resetEnemy()
    {
        isDead = false;
        gun.SetActive(false);
        Destroy(bulletObj);
        bulletObj = null;
    }

    
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(turnAroundTotalTime); // wait for turning around
        animator.SetTrigger("draw");

        yield return new WaitForSeconds(0.3f); // wait for right time to make gun appear
        gun.SetActive(true);

        yield return new WaitForSeconds(drawingTotalTime - 0.3f); // wait for drawing gun
        ShootBullet();
    }

    private void ShootBullet()
    {
        if(!isDead)
        {
            bulletObj = Instantiate(bullet);
            bulletObj.transform.position = spawnPoint.position;

            Vector3 direction = (target.position - bulletObj.transform.position).normalized;
            bulletObj.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
         
        }
        
    }
}
