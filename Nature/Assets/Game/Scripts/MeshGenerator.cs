using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] Transform player;
    List<Vector3> playerPositions;
    List<Quaternion> playerRotations;
    Mesh mesh;
    MeshCollider collider;

    [SerializeField] float radius;
    [SerializeField] int sliceVerts;
    Vector3[] circle;
    int[] newCirclesIndexs;
    int[] prevCircle;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        //initialize variables
        playerPositions = new List<Vector3>();
        playerRotations = new List<Quaternion>();
        newCirclesIndexs = new int[sliceVerts];
        prevCircle = new int[sliceVerts];



        InitializeCircle();
        for (int i = 0; i < circle.Length; i++)
        {
            Vector3 point = player.localRotation * circle[i];//vertex;
            point += player.localPosition;

            newCirclesIndexs[i] = vertices.Count;

            vertices.Add(point);
        }
    }

    public void AddToMesh()
    {
        playerPositions.Add(player.localPosition);
        playerRotations.Add(player.localRotation);
        //add vertices
        for (int i = 0; i < circle.Length; i++)
        {
            Vector3 point = player.localRotation * circle[i];
            point += player.localPosition;
            //possible if case
            prevCircle[i] = newCirclesIndexs[i]; //push down previous value
            newCirclesIndexs[i] = vertices.Count; //add newest value to new circle

            vertices.Add(point);

        }
        //add triangles
        for (int i = 0; i < sliceVerts; i++)
        {
            triangles.Add(newCirclesIndexs[i]);
            triangles.Add(prevCircle[i]);
            triangles.Add(prevCircle[(i + 1) % sliceVerts]);

            triangles.Add(newCirclesIndexs[(i + 1) % sliceVerts]);
            triangles.Add(newCirclesIndexs[i]);
            triangles.Add(prevCircle[(i + 1) % sliceVerts]);
        }
        //push to mesh
        UpdateMesh();
    }

    public void InitializeCircle()
    {
        circle = new Vector3[sliceVerts];
        Vector3 point = new Vector3(radius * player.lossyScale.x, 0, 0);
        Quaternion rot = Quaternion.Euler(0, 0, 360 / sliceVerts);
        for (int i = 0; i < sliceVerts; i++)
        {
            circle[i] = point;
            point = rot * point;
        }
    }


    void UpdateMesh()
    {
        //clear data
        mesh.Clear();
        //create vertices
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //calculate lighting
        mesh.RecalculateNormals();
    }
}
