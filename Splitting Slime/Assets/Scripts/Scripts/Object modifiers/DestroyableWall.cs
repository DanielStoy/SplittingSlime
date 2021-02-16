using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cracks;
    private int hits = 0;
    void Start()
    {
        cracks = transform.GetChild(0).gameObject;
        cracks.SetActive(false);
    }

    public void takeDamage()
    {
        hits++;
        if(hits == 1)
        {
            cracks.SetActive(true);
        }
        else if(hits == 2)
        {
            gameObject.SetActive(false);
        }
    }
}
