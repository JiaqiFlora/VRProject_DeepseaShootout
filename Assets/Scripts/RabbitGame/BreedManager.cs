using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedManager : MonoBehaviour
{
    public static BreedManager instance { get; private set; }

    public GameObject littleRabbitPrefab;

    public Rabbit father;
    public Rabbit mother;

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
        //BeginBreed(father, mother);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginBreed()
    {
        Debug.Log("ganjiaqi: --- begin breed!");
        // step 1: init baby rabbit
        // TODO: - ganjiaqi where to spawn???
        GameObject spawnedRabbit = Instantiate(littleRabbitPrefab);
        Vector3 spawnPoint = mother.transform.position + new Vector3(1.0f, 0.0f, 0.0f); // FIXME: - ganjiaqi temporarily besides father
        spawnedRabbit.transform.position = spawnPoint;
        spawnedRabbit.transform.rotation = father.transform.rotation;
        Rabbit rabbit = spawnedRabbit.GetComponent<Rabbit>();

        // step 2: use parents to update it
        Debug.Log("ganjiaqi: --- init rabbits to update format!!!");
        rabbit.SetFormat(father, mother);

        // step3: trigger animation
        // TODO: - ganjiaqi here to trigger animation

        // step4: clear parents for next time, in manager and for rabbits
        // TODO: - ganjiaqi clear!!!!
        //ClearParents();
    }

    private void ClearParents()
    {
        father.NotToBeParents();
        mother.NotToBeParents();
        father = null;
        mother = null;
    }

}
