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
    private float spikesLength;

    public float TileLength => tileLength;



    private void Start()
    {
        spikesLength = GetTileLength(botSpikes.GetComponent<MeshFilter>().sharedMesh);
        tileLength = GetTileLength();
    }



    void OnTriggerExit(Collider other)
    {
        transform.position += new Vector3(0f, 0f, tileLength * 1.5f);
        GenerateSpikes();
    }

    private void GenerateSpikes()
    {
        GenerateSpikesTop();
        GenerateSpikesBot();
    }



    private void GenerateSpikesTop()
    {
        if (spawnedTopSpikes != null)
        {
            Destroy(spawnedTopSpikes);
        }

        spawnedTopSpikes = Instantiate(topSpikes, transform);
        spawnedTopSpikes.transform.position = new Vector3(spawnedTopSpikes.transform.position.x - 2f,
                                            spawnedTopSpikes.transform.position.y,
                                            spawnedTopSpikes.transform.position.z + (Random.Range(-tileLength / 2 * (1 - spikesLength / tileLength), tileLength / 2 * (1 - spikesLength / tileLength))));
        spawnedTopSpikes.transform.localRotation = Quaternion.Euler(spawnedTopSpikes.transform.localRotation.x, 0f, 180f);
    }

    private void GenerateSpikesBot()
    {
        if (spawnedBotSpikes != null)
        {
            Destroy(spawnedBotSpikes);
        }

        spawnedBotSpikes = Instantiate(botSpikes, transform);
        spawnedBotSpikes.transform.position = new Vector3(spawnedBotSpikes.transform.position.x - 2f,
                                            spawnedBotSpikes.transform.position.y,
                                            spawnedBotSpikes.transform.position.z + (Random.Range(-tileLength / 2 * (1 - spikesLength / tileLength), tileLength / 2 * (1 - spikesLength / tileLength))));
        spawnedBotSpikes.transform.localRotation = Quaternion.Euler(spawnedBotSpikes.transform.localRotation.x, 0f, spawnedBotSpikes.transform.localRotation.z);
    }


    private float GetTileLength(Mesh mesh = null)
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }

        float leftPoint = mesh.vertices[0].x;
        foreach (Vector3 point in mesh.vertices)
        {
            if (point.x < leftPoint)
            {
                leftPoint = point.x;
            }
        }

        float rightPoint = mesh.vertices[0].x;
        foreach (Vector3 point in mesh.vertices)
        {
            if (point.x > rightPoint)
            {
                rightPoint = point.x;
            }
        }



        return Mathf.Abs(rightPoint - leftPoint);

    }
}
