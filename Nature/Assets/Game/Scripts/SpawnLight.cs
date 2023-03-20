using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnLight : MonoBehaviourPun
{
    [SerializeField] PhotonView photonView;
    [SerializeField] GameObject lightPrefab;
    [SerializeField] int mapSize;
    [SerializeField] int lightNumber;

    LightsObject lights;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("the code after this is breaking things");
        if (PhotonNetwork.IsMasterClient)
        {
            lights = new LightsObject();

            for (int i = 0; i < lightNumber; i++)
            {
                Vector3 pos = new Vector3(Random.Range(40, mapSize * 10), 0, 0); // set random distance from the centre
                Quaternion rotationalPos = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)); // randomRotation
                pos = rotationalPos * pos; //create actual position
                Vector3Int finalPos = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z); //make ints for faster transfer

                //if space taken, redo
                //if (Physics.OverlapSphere((Vector3)finalPos, .1f) != null)
                //{
                //    i--;
                //    continue;
                //}
                //instantiate on local client
                GameObject light = Instantiate(lightPrefab, (Vector3)finalPos / 10, Quaternion.identity);
                DontDestroyOnLoad(light);
                //add data to object for transfer
                lights.list.Add(finalPos);
            }
            //transfer position data to other clients
            string dataTransferString = JsonUtility.ToJson(lights);
            photonView.RPC("RPC_SyncLights", RpcTarget.OthersBuffered, dataTransferString);
        }
    }

    [PunRPC]
    public void RPC_SyncLights(string dataTransferString)
    {
        Debug.Log(dataTransferString);
        //convert data to usable object
        lights = JsonUtility.FromJson<LightsObject>(dataTransferString);
        //instantiate for each data point
        for (int i = 0; i < lights.list.Count; i++)
        {
            GameObject light = Instantiate(lightPrefab, (Vector3)lights.list[i] / 10, Quaternion.identity);
            DontDestroyOnLoad(light);
        }
    }
}

class LightsObject
{
    public List<Vector3Int> list = new List<Vector3Int>();
}
