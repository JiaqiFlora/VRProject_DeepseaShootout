using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternHub : MonoBehaviour
{
    public static PatternHub instance { get; private set; }

    [Header("model")]
    public GameObject bigEarRabbit;
    public GameObject bigButtRabbit;
    public GameObject bigEarAndBigButtRabbit;
    public GameObject normalRabbit;

    // TODO: - ganjiaqi add more texture
    [Header("pattern texture")]
    public Texture texture1;
    public Texture texture2;

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

    // TODO: - ganjiaqi add more texture
    // Map for texture index(priority) and texture value
    public Dictionary<int, Texture> textureDictionary = new Dictionary<int, Texture>();

    // Start is called before the first frame update
    void Start()
    {
        textureDictionary.Add(1, texture1);
        textureDictionary.Add(2, texture2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
