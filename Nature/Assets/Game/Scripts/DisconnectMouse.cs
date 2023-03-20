using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
}
