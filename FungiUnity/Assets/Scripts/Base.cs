using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] GameObject grass;
    [SerializeField] GameObject[] clovers;
    [SerializeField] GameObject[] flowers;
    private bool bool1 = true;
    private bool bool2 = true;
    private bool bool3 = true;
    private bool flowerbool1 = true;
    private bool flowerbool2 = true;
    private bool flowerbool3 = true;
    public bool IsFunged { get; set; }
    public bool IsMycelled { get; set; }
    public bool AllowsFunging { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public Root[] rootArray;

    // Start is called before the first frame update
    void Start()
    {
        Randomizer();
    }

    private void Randomizer()
    {
        List<int> rotationStuff = new List<int>();
        rotationStuff.Add(90);
        rotationStuff.Add(180);
        rotationStuff.Add(270);
        int random = Random.Range(0, 2);
        Quaternion newRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, rotationStuff[random], gameObject.transform.rotation.eulerAngles.z);
        gameObject.transform.rotation = newRotation;
        int random2 = Random.Range(0, 2);
        Quaternion newRotation2 = Quaternion.Euler(grass.transform.rotation.eulerAngles.x, rotationStuff[random], grass.transform.rotation.eulerAngles.z);
        grass.transform.rotation = newRotation2;
        for (int i = 0; i < clovers.Length; i++)
        {
            int randombool1 = Random.Range(0, 2);
            int randombool2 = Random.Range(0, 2);
            if(randombool1 == 0)
            {
                bool1 = false;
            }
            if(randombool1 == 1)
            {
                bool1 = true;
            }
            if (randombool2 == 0)
            {
                bool2 = false;
            }
            if (randombool2 == 1)
            {
                bool2 = true;
            }

            if(bool1 && bool2)
            {
                clovers[i].SetActive(true);
            }
            else
            {
                clovers[i].SetActive(false);
            }
        }
        for (int i = 0; i < flowers.Length; i++)
        {
            int randombool1 = Random.Range(0, 2);
            int randombool2 = Random.Range(0, 2);
            int randombool3 = Random.Range(0, 2);
            if (randombool1 == 0)
            {
               flowerbool1 = false;
            }
            if (randombool1 == 1)
            {
                flowerbool1 = true;
            }
            if (randombool2 == 0)
            {
               flowerbool2 = false;
            }
            if (randombool2 == 1)
            {
                flowerbool2 = true;
            }
            if (randombool3 == 0)
            {
                flowerbool3 = false;
            }
            if (randombool3 == 1)
            {
                flowerbool3 = true;
            }

            Debug.Log("Flowerbool1: " + flowerbool1);
            Debug.Log("Flowerbool2: " + flowerbool2);
            Debug.Log("Flowerbool3: " + flowerbool3);

            if (flowerbool1 && flowerbool2 && flowerbool3)
            {
                flowers[i].SetActive(true);
            }
            else
            {
                flowers[i].SetActive(false);
            }
        }
    }
    public void DoHighlight(bool highlighted, bool correct)
    {
        // Highlight this block based on the parameters
    }

    public bool IsFungable()
    {
        return AllowsFunging && !IsFunged;
    }

    public bool IsMycelable()
    {
        return AllowsFunging && !IsMycelled;
    }
}
