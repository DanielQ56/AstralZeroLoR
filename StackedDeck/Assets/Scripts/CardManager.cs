using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    Region[] allRegions;


    // Start is called before the first frame update
    void Awake()
    {
        LoadJson.DeleteCardsOnPath();
        allRegions = LoadJson.LoadResourceTextFile();
        for(int i = 0; i < allRegions.Length; ++i)
        {
            Debug.Log(Utilities.IndexToRegion[i] + ":\n" + allRegions[i].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
