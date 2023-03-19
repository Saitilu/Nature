using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    PlayerController playerController;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.speed = PlayerPrefs.GetFloat("speed")*2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
