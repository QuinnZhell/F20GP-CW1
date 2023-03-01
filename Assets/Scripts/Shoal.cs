using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoal : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private Fish fishPrefab;
    [SerializeField] private int shoalPop = 60;
    [SerializeField] private Vector3 spawnBounds;
    public Fish[] shoalMembers {get; set;}

    [Header ("Steering Behaviours")]
    [Range(0,10)] [SerializeField] private float _cohesion = 9.0f;
    public float getCohesion { get {return _cohesion;}}
    [Range(0,10)] [SerializeField] private float _alignment = 9.0f;
    public float getAlignment { get {return _alignment;}}
    [Range(0,10)] [SerializeField] private float _avoidance = 2.0f;
    public float getAvoidance { get {return _avoidance;}}

    [Header ("Speed Settings")]
    [Range(0,10)] [SerializeField] private float _minSpeed = 3;
    public float minSpeed {get {return _minSpeed;}}
    [Range(0,10)] [SerializeField] private float _maxSpeed = 4;
    public float maxSpeed {get {return _maxSpeed;}}
    
    private float fishFOV;

    void Awake() {
        fishFOV = Mathf.Max(_cohesion, Mathf.Max(_alignment, _avoidance));
    }

    // Start is called before the first frame update
    void Start() {
        GenerateUnits();   
    }

    // Update is called once per frame
    public void Update() {
        for(int i = 0; i < shoalMembers.Length; i++) {
            shoalMembers[i].Swim();
        }
    }

    private void GenerateUnits() {
        shoalMembers = new Fish[shoalPop];
        int positionOffset = shoalPop / 3;

        // instantiate fish in the shoal at random points in the boundaries
        for (int i = 0; i < shoalPop; i++) {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x + i%positionOffset, randomVector.y * spawnBounds.y + i%positionOffset, randomVector.z * spawnBounds.z + i%positionOffset);
            var spawnPosition = transform.position + randomVector;
            var randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            shoalMembers[i] = Instantiate(fishPrefab, spawnPosition, randomRotation);
            shoalMembers[i].SetShoal(this);
            shoalMembers[i].SetSphereCastRadius(fishFOV);
            shoalMembers[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }
    }

    // void spiral(int points, float turn){
    //     for(int i = 0; i < points; i++) {
    //         float dst = i / (points - 1f);
    //         float inclination = Mathf.Acos(1- 2 * dst);
    //         float angle = 2 * Mathf.PI * turn * i;

    //         float x = Mathf.Sin(inclination) * Mathf.Cos(angle);
    //         float y = Mathf.Sin(inclination) * Mathf.Sin(angle);
    //         float z = Mathf.Cos(inclination);

    //         PlotPoint(x,y,z);
    //     }
    // }

    // void PlotPoint(float x, float y, float z) {
    //     //Instantiate(dot, new Vector3(x*2,y*2, z*2), Quaternion.identity);
    //     Debug.DrawLine(new Vector3(0,0,0), new Vector3(x,y,z), Color.green, 1000);
    // }
}
