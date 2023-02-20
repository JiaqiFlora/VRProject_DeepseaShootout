using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Rabbit : MonoBehaviour
{
    //[Header("Change Part")]
    //public GameObject ear;
    //public GameObject tail;

    // property control
    [Header("fit property")]
    public bool bigEar = false;
    public bool bigButt = false; // use these two to select the adult model
    public int firstTextureIndex = 1; // index for texture in hub(also priority)
    public int secondTextureIndex = 1;
    public Color color = Color.white;
    public GameObject finalModel;
    

    [Header("other property")]
    public int carrotNumToGrow = 1; // TODO: - ganjiaqi decides final number or depends on rarerity
    public bool isAdult = false;

    private Material wholeMaterail;
    private Texture patternTexture;

    private bool beenMother = false;
    private bool beenFather = false;
    private bool isMoving = false;
    private int carrotNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.activated.AddListener(TriggerSelected);
    }

    // Update is called once per frame
    void Update()
    {
        // control mother moving
        if(isMoving)
        {
            Rabbit father = BreedManager.instance.father;
            Transform fatherTransform = father.transform;
            Vector3 direction = fatherTransform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);
            transform.position += transform.forward * Time.deltaTime * 2.0f;

        }

        // check if rabbit can grow up to adult
        if(carrotNum == carrotNumToGrow && !isAdult)
        {
            BecomeAdult();
            isAdult = true;
        }
    }


    // get final model and update its texture!
    public void SetFormat(Rabbit father, Rabbit mother)
    {
        SelectModel(father, mother);
        SelectPropertyForMaterial(father, mother);
    }

    
    // here to become an adult
    public void BecomeAdult()
    {
        Debug.Log("ganjiaqi: --- become adult!!!");
        // TODO: - ganjiaqi trigger pop up animation for adult!
        GameObject adultModel = Instantiate(finalModel, transform.position, transform.rotation);
        adultModel.transform.SetParent(transform.parent);
        adultModel.GetComponent<Rabbit>().UpdateVariable(this);
        adultModel.GetComponent<Rabbit>().UpdateMaterial();

        Destroy(gameObject);
    }

    // breed button click, this rabbit to be father
    public void BreedButtonClick()
    {
        Debug.Log("ganjiaqi: ----- breen button click!!");
        BreedManager.instance.father = this;
        beenFather = true;
    }

    public void NotToBeParents()
    {
        beenFather = false;
        beenMother = false;
    }

    // TODO: - ganjiaqi here to move to carrot or keep in rabbit
    public void AddCarrot()
    {
        Debug.Log("ganjiaqi: ----- carrot add !");
        carrotNum += 1;
    }

    // here to handle for collide with father to breed
    private void OnCollisionEnter(Collision collision)
    {
        if (beenMother && collision.collider.tag == "rabbit" && collision.collider.gameObject.GetComponent<Rabbit>() == BreedManager.instance.father)
        {
            Debug.Log("ganjiaqi: ---- meet father!!!");
            isMoving = false;
            BreedManager.instance.BeginBreed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //nowBreed = false;

    }

    // select this rabbit to breed to be mother
    private void TriggerSelected(ActivateEventArgs arg0)
    {
        Debug.Log("ganjiaqi: ------ select rabbit!!!!");
        if(BreedManager.instance.father != null && !beenFather)
        {
            BeMotherAndForward();
        }
    }

    // steps for being mother
    private void BeMotherAndForward()
    {
        // step 1: set Breedmanager's mother
        BreedManager.instance.mother = this;
        beenMother = true;

        // step 2: move forward to fatehr
        // TODO: - ganjiaqi here to add forward to target animation
        isMoving = true;

        // step 3: begin breed, when collide with father, do this in ontrigger enter
        
    }


    // TODO: - ganjiaqi remember to update all values here
    private void UpdateVariable(Rabbit originalRabbit)
    {
        bigEar = originalRabbit.bigEar;
        bigButt = originalRabbit.bigButt;
        firstTextureIndex = originalRabbit.firstTextureIndex;
        secondTextureIndex = originalRabbit.secondTextureIndex;
        color = originalRabbit.color;
        finalModel = originalRabbit.finalModel;

        wholeMaterail = originalRabbit.wholeMaterail;
        patternTexture = originalRabbit.patternTexture;
    }

    // select model for rabbit based on its ear and butt
    private void SelectModel(Rabbit father, Rabbit mother)
    {
        // step 1: first to decide its big ear
        if(father.bigEar == mother.bigEar) // if mother and father has same ears, child must be same with parents
        {
            bigEar = father.bigEar;
        } else // if theirs are not same, half half possibility
        {
            bigEar = (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f) ? false : true;
        }

        // step 2: second to decide its butt
        if (father.bigButt == mother.bigButt) // if mother and father has same butts, child must be same with parents
        {
            bigButt = father.bigButt;
        }
        else // if theirs are not same, half half possibility
        {
            bigButt = (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f) ? false : true;
        }

        // step 3: set current model
        // TODO: - ganjiaqi time to init these models!!!
        if(bigEar && bigButt)
        {
            finalModel = PatternHub.instance.bigEarAndBigButtRabbit;
        } else if(bigEar)
        {
            finalModel = PatternHub.instance.bigEarRabbit;
        } else if(bigButt)
        {
            finalModel = PatternHub.instance.bigButtRabbit;
        } else
        {
            finalModel = PatternHub.instance.normalRabbit;
        }
        
    }

    // update rabbit color and pattern
    private void SelectPropertyForMaterial(Rabbit father, Rabbit mother)
    {
        // step 1: set color, 25% father, 25% mother, 50% middle color(fixed 05 fraction)
        // TODO: - ganjiaqi check with designer of fixed middle color
        Color fatherColor = father.color;
        Color motherColor = mother.color;
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log("ganjiaqi: ---- random number for color: " + randomValue);
        if(randomValue <= 0.25f)
        {
            color = fatherColor;
        } else if(randomValue >= 0.75f)
        {
            color = motherColor;
        } else
        {
            color = Color.Lerp(fatherColor, motherColor, 0.5f);
        }

        // step 2: set texture pattern
        int fatherFirstTextureIndex = father.firstTextureIndex;
        int fatherSecondTextureIndex = father.secondTextureIndex;
        int motherFirstTextureIndex = mother.firstTextureIndex;
        int motherSecondTextureIndex = mother.secondTextureIndex;

        randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log("ganjiaqi: ---- random number for texture: " + randomValue);
        if (randomValue <= 0.25f) // f1 & m1
        {
            firstTextureIndex = fatherFirstTextureIndex;
            secondTextureIndex = motherFirstTextureIndex;
        } else if (randomValue <= 0.5f) // f1 & m2
        {
            firstTextureIndex = fatherFirstTextureIndex;
            secondTextureIndex = motherSecondTextureIndex;
        } else if(randomValue <= 0.75f) // f2 && m1
        {
            firstTextureIndex = fatherSecondTextureIndex;
            secondTextureIndex = motherFirstTextureIndex;
        } else // f2 & m2
        {
            firstTextureIndex = fatherSecondTextureIndex;
            secondTextureIndex = motherSecondTextureIndex;
        }

        UpdateMaterial();

    }

    private void UpdateMaterial()
    {
        Material newMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if(firstTextureIndex <= secondTextureIndex)
        {
            patternTexture = PatternHub.instance.textureDictionary[firstTextureIndex];
        } else
        {
            patternTexture = PatternHub.instance.textureDictionary[secondTextureIndex];
        }

        newMat.color = color;
        newMat.SetTexture("_BaseMap", patternTexture);
        wholeMaterail = newMat;
        UpdateMaterialForChildren(this.transform, newMat);
    }

    // recursively loop through all the children objects to set materail to all parts of object
    private void UpdateMaterialForChildren(Transform curTransform, Material newMat)
    {
        if (curTransform.childCount == 0)
        {
            return;
        }

        foreach (Transform child in curTransform)
        {
            // set material
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMat;
            }

            // recursively call all the children
            UpdateMaterialForChildren(child, newMat);
        }
    }


    // useless!!!!!
    //public void UpdateFormat(Rabbit father, Rabbit mother)
    //{
    //    Debug.Log("ganjiaqi: --- begin update format!");
    //    // TODO: - ganjiaqi check: use 4 color and 4 texture of 2???
    //    // father thing
    //    Material fatherMat = father.GetComponent<Rabbit>().wholeMaterail;
    //    Texture fatherTexture1 = fatherMat.GetTexture("_texture1");
    //    Texture fatherTexture2 = fatherMat.GetTexture("_texture2");
    //    float fatherTexFrac = fatherMat.GetFloat("_fraction");
    //    float fatherColorFrac = fatherMat.GetFloat("_colorFrac");
    //    Color fatherColor1 = fatherMat.GetColor("_basicColor1");
    //    Color fatherColor2 = fatherMat.GetColor("_basicColor2");
    //    Color fatherColor = Color.Lerp(fatherColor1, fatherColor2, fatherColorFrac);

    //    // mother thing
    //    Material motherMat = mother.GetComponent<Rabbit>().wholeMaterail;
    //    Texture motherTexture1 = motherMat.GetTexture("_texture1");
    //    Texture motherTexture2 = motherMat.GetTexture("_texture2");
    //    float motherTexFrac = fatherMat.GetFloat("_fraction");
    //    float motherColorFrac = fatherMat.GetFloat("_colorFrac");
    //    Color motherColor1 = motherMat.GetColor("_basicColor1");
    //    Color motherColor2 = motherMat.GetColor("_basicColor2");
    //    Color motherColor = Color.Lerp(motherColor1, motherColor2, motherColorFrac);


    //    Shader rabbitShader = Resources.Load<Shader>("shaders/RabbitDemoShader");
    //    if (rabbitShader == null)
    //    {
    //        Debug.Log("ganjiaqi: --- fail to load rabbit shader!!!");
    //    }

    //    Material newMat = new Material(rabbitShader);

    //    // TODO: - ganjiaqi check with designer!!! stategy and algorithm for new textures and color
    //    // set color, lerp between mother final color and father final color
    //    newMat.SetColor("_basicColor1", motherColor);
    //    newMat.SetColor("_basicColor2", fatherColor);
    //    newMat.SetFloat("_colorFrac", Random.Range(0.0f, 1.0f));

    //    // set texture1 and texture2 based on nexture fraction with 0.5
    //    if (fatherTexFrac <= 0.5)
    //    {
    //        newMat.SetTexture("_texture1", fatherTexture1);
    //    } else
    //    {
    //        newMat.SetTexture("_texture1", fatherTexture2);
    //    }
    //    if (motherTexFrac <= 0.5)
    //    {
    //        newMat.SetTexture("_texture2", motherTexture1);
    //    }
    //    else
    //    {
    //        newMat.SetTexture("_texture2", motherTexture2);
    //    }
    //    // if texture2 is null, only have texture1. Make sure current rabbit fraction smaller than 0.5
    //    if(newMat.GetTexture("_texture2") == null)
    //    {
    //        newMat.SetFloat("_fraction", Random.Range(0.0f, 0.4f));
    //    } else
    //    {
    //        newMat.SetFloat("_fraction", Random.Range(0.0f, 1.0f));
    //    }

    //    // other parts of body
    //    // father thing
    //    GameObject fatherEar = father.ear;
    //    GameObject fatherTail = father.tail;
    //    Vector3 faterEarScale = fatherEar.transform.localScale;
    //    Vector3 faterTailScale = fatherTail.transform.localScale;

    //    // mother thing
    //    GameObject motherEar = mother.ear;
    //    GameObject motherTail = mother.tail;
    //    Vector3 motherEarScale = motherEar.transform.localScale;
    //    Vector3 motherTailScale = motherTail.transform.localScale;

    //    Vector3 earScale = Vector3.Lerp(faterEarScale, motherEarScale, Random.Range(0.0f, 1.0f));
    //    Vector3 tailScale = Vector3.Lerp(faterTailScale, motherTailScale, Random.Range(0.0f, 1.0f));
    //    ear.transform.localScale = earScale;
    //    tail.transform.localScale = tailScale;

    //    // use this new material to update current material for each child
    //    UpdateMaterialForChildren(this.transform, newMat);

    //}

    //// useless!!!!!
    //// Update materails for all children
    //private void UpdateMaterialForChildren(Transform curTransform, Material newMat)
    //{
    //    wholeMaterail = newMat;

    //    if (curTransform.childCount == 0)
    //    {
    //        return;
    //    }

    //    foreach (Transform child in curTransform)
    //    {
    //        // set material
    //        Renderer renderer = child.GetComponent<Renderer>();
    //        if (renderer != null)
    //        {
    //            renderer.material = newMat;
    //        }

    //        // recursively call all the children
    //        UpdateMaterialForChildren(child, newMat);

    //    }

    //}
}
