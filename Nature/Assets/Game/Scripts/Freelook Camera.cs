using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreelookCamera : MonoBehaviour
{
    [SerializeField] float sensitivity;
    Vector2 turn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //turn Camera
        turn += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
