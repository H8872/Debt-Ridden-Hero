using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonB : MonoBehaviour
{
    PlayerController player;
    RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.rangedTime > 0)
        {
            image.color = Color.gray;
        } else {
            image.color = Color.white;
        }
    }
}
