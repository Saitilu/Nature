using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    PlayerController playerController;
    MeshGenerator meshGen;
    [SerializeField] Slider speedSlider;
    [SerializeField] Slider nimblenessSlider;
    [SerializeField] Slider sizeSlider;
    GameObject otherPlayer;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    string name;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPosition = new Vector3(1, 1, 1); // Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)
        name = PhotonNetwork.CurrentRoom.PlayerCount.ToString();//player number
        GameObject player;
        if (name == "1")
        {
            player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        }
        else
        {
            player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.Euler(180, 0, 0));
        }

        player.name = name;
        DontDestroyOnLoad(player);
        playerController = player.transform.Find("Pivot").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //when other player joins add their player character to DontDestroyOnLoad
        otherPlayer = GameObject.Find("Player(Clone)");
        if (otherPlayer != null)
            DontDestroyOnLoad(otherPlayer);

        //sync players stats with sliders
        playerController.efficiency = speedSlider.value;
        playerController.nimbleness = nimblenessSlider.value;
        playerController.SetScale(sizeSlider.value);
    }

    public void JoinGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
