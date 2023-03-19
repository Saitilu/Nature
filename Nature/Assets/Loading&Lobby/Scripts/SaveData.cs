using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SaveData : MonoBehaviour
{
    int temp;
    // Start is called before the first frame update
    void Start()
    {
        temp = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public void OnReady()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void OnApplicationQuit()
    {

        PlayerPrefs.SetFloat("speed", temp);
    }
}
