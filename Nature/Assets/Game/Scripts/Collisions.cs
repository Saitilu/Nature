using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Collisions : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Loading");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            playerController.Boost();
            Destroy(other);
        }
    }
}
