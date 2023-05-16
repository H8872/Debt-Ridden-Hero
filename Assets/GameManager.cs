using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Debug.Log($"Only one {this} allowed. Deleting this {this}.");
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
