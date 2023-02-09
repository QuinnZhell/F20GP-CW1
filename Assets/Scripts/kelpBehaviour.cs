using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kelpBehaviour : MonoBehaviour
{
    // Animation parameters
    public float waveSpeed = 0.5f;
    public float waveAmplitude = 0.05f;
    public float waveFrequency = 0.1f;

    public GameObject leafMeshPrefab;

    // Initial position of each bone
    private Vector3[] initialPositions;
    private GameObject[] leafMeshInstances;
    private Transform[] bones;

    void Start()
    {
        // Get the bones in the rig
        bones = GetComponentsInChildren<Transform>();
        leafMeshInstances = new GameObject[bones.Length];

        // Store the initial position of each bone
        initialPositions = new Vector3[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            initialPositions[i] = bones[i].localPosition;

            if (bones[i].name.Contains("Bone"))
            {
                GameObject leafMeshInstance = Instantiate(leafMeshPrefab, bones[i].position, Quaternion.identity);
                leafMeshInstance.transform.parent = bones[i];
                leafMeshInstances[i] = leafMeshInstance;
            }

        }
    }

    void Update()
    {

        // Animate each bone
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i].name.Contains("Bone"))
            {
                leafMeshInstances[i].transform.position = bones[i].position;
                leafMeshInstances[i].transform.rotation = bones[i].rotation;
            }

        }
    }
}