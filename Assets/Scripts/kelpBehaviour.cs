using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kelpBehaviour : MonoBehaviour
{

    public GameObject leafMeshPrefab;
    public float leafRotationVariance = 2.0f;

    // Initial position of each bone
    private Vector3[] initialPositions;
    private GameObject[] leafMeshInstances;
    private Transform[] bones;
    private float[] leafRotationOffset;

    void Start()
    {
        // Get the bones in the rig
        bones = GetComponentsInChildren<Transform>();
        leafMeshInstances = new GameObject[bones.Length];
        leafRotationOffset = new float[bones.Length];

        // Store the initial position of each bone
        initialPositions = new Vector3[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            initialPositions[i] = bones[i].localPosition;

            if (bones[i].name.Contains("Bone"))
            {
                leafRotationOffset[i] = Random.Range(-leafRotationVariance, leafRotationVariance);
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
                leafMeshInstances[i].transform.rotation = new Quaternion(bones[i].rotation.x, bones[i].rotation.y + leafRotationOffset[i], bones[i].rotation.z, bones[i].rotation.w);
            }

        }
    }
}