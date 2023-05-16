using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Slam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SlamGround()
    {
        GameObject newSlam = Instantiate(Slam);
        Destroy(newSlam, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
