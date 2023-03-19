using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed;
    [SerializeField] float nimbleness;
    private float timer = 0;
    [SerializeField] MeshGenerator stemScript;
    Rigidbody rigidbody;
    [SerializeField] Transform cameraRotater;
    [SerializeField] float sensitivity;

    float camTurn;
    Vector2 turn;

    [SerializeField] KevinCastejon.ConeMesh.Cone coneScript;

    //multiplayer
    PhotonView view;
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        if (!coneScript.IsConeGenerated)
        {
            coneScript.GenerateCone();
        }

        rigidbody = this.GetComponent<Rigidbody>();

        //multiplayer
        view = GetComponentInParent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(cam);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && SceneManager.GetActiveScene().name == "Game")
        {
            //turn Camera
            camTurn = Input.GetAxis("Mouse X") * sensitivity;
            cameraRotater.localRotation *= Quaternion.Euler(0, 0, -camTurn); //turn camera


            //turn Player
            turn.x = Input.GetAxis("Horizontal") * nimbleness;
            turn.y = -Input.GetAxis("Vertical") * nimbleness;
            Quaternion newRotation = cameraRotater.rotation * Quaternion.Euler(turn.y, turn.x, 0);
            newRotation *= Quaternion.Euler(0, 0, -cameraRotater.localRotation.eulerAngles.z);
            rigidbody.MoveRotation(newRotation);
        }
    }
    void FixedUpdate()
    {
        if (view.IsMine && SceneManager.GetActiveScene().name == "Game")
            MoveCharacter();
        timer += Time.deltaTime;
        if (timer > .1)
        {
            timer %= .1f;
            stemScript.AddToMesh();
        }
    }

    void MoveCharacter()
    {
        //find direction
        Vector3 direction = Vector3.Normalize(transform.forward);
        //move in direction
        rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }
}
