using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{

    [SerializeField] private GameObject botSpikes;
    [SerializeField] private GameObject topSpikes;


    private GameObject spawnedBotSpikes;
    private GameObject spawnedTopSpikes;

    private float tileLength;


    private void Start()
    {
        tileLength = GetTileLength();
    }



    void OnTriggerExit(Collider other)
    {
        transform.position += new Vector3(0f, 0f, tileLength);
        GenerateSpickes();
    }

    private void GenerateSpickes()
    {
        if (spawnedBotSpikes != null)
        {
            Destroy(spawnedTopSpikes);
        }
        if (spawnedTopSpikes != null)
        {
            Destroy(spawnedTopSpikes);
        }

        spawnedTopSpikes = Instantiate(topSpikes, transform);
        spawnedTopSpikes.transform.position = new Vector3(spawnedTopSpikes.transform.position.x - 2f,
                                            spawnedTopSpikes.transform.position.y,
                                            spawnedTopSpikes.transform.position.z + (Random.Range(-tileLength * 0.8f, tileLength * 0.8f)));
        spawnedTopSpikes.transform.localRotation = Quaternion.Euler(spawnedTopSpikes.transform.localRotation.x, 0f, 180f);

        spawnedBotSpikes = Instantiate(botSpikes, transform);
        spawnedBotSpikes.transform.position = new Vector3(spawnedBotSpikes.transform.position.x - 2f,
                                            spawnedBotSpikes.transform.position.y,
                                            spawnedBotSpikes.transform.position.z + (Random.Range(-tileLength * 0.8f, tileLength * 0.8f)));
        spawnedBotSpikes.transform.localRotation = Quaternion.Euler(spawnedBotSpikes.transform.localRotation.x, 0f, spawnedBotSpikes.transform.localRotation.z);
    }


    private float GetTileLength()
    {
        float leftPoint = GetComponent<MeshFilter>().sharedMesh.vertices[0].x;
        foreach (Vector3 point in GetComponent<MeshFilter>().sharedMesh.vertices)
        {
            if (point.x < leftPoint)
            {
                leftPoint = point.x;
            }
        }

        float rightPoint = GetComponent<MeshFilter>().sharedMesh.vertices[0].x;
        foreach (Vector3 point in GetComponent<MeshFilter>().sharedMesh.vertices)
        {
            if (point.x > rightPoint)
            {
                rightPoint = point.x;
            }
        }



        return Mathf.Abs(rightPoint - leftPoint);

    }
}
