using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRandomizer : MonoBehaviour
{
    [SerializeField] GameObject grass;
    [SerializeField] GameObject[] clovers;
    [SerializeField] GameObject[] flowers;
    private bool bool1;
    private bool bool2;
    private bool bool3;

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
        Quaternion newRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, rotationStuff[random]);
        gameObject.transform.rotation = newRotation;
        int random2 = Random.Range(0, 2);
        Quaternion newRotation2 = Quaternion.Euler(grass.transform.rotation.eulerAngles.x, grass.transform.rotation.eulerAngles.y, rotationStuff[random]);
        grass.transform.rotation = newRotation2;
        for (int i = 0; i < clovers.Length; i++)
        {
            int randombool1 = Random.Range(0, 1);
            int randombool2 = Random.Range(0, 1);
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
            int randombool1 = Random.Range(0, 1);
            int randombool2 = Random.Range(0, 1);
            int randombool3 = Random.Range(0, 1);
            if (randombool1 == 0)
            {
                bool1 = false;
            }
            if (randombool1 == 1)
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
            if (randombool3 == 0)
            {
                bool3 = false;
            }
            if (randombool3 == 1)
            {
                bool3 = true;
            }

            if (bool1 && bool2 && bool3)
            {
                flowers[i].SetActive(true);
            }
            else
            {
                flowers[i].SetActive(false);
            }
        }
    }
}
