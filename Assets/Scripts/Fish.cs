using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Fish : MonoBehaviour
{
    // the shoal class which holds the rest of fish
    private Shoal shoal;

    // movement fields
    private Vector3 currentVelocity;
    public float speed;

    public Transform fishTransform {get;set;}       // quicker to get as transform.position goes through further classes
    [SerializeField] private float FOV;             // range that the fish can see, used for calcualting steering and boundaries
    [SerializeField] private float SmoothDamp;      // value which allows for movement from two points to be gradual and smooth

    // collection of nearby fish for each calculation
    private List<Fish> cohesionNeighbours = new List<Fish>();
    private List<Fish> avoidNeighbours = new List<Fish>();
    private List<Fish> alignNeighbours = new List<Fish>();
    private LayerMask fish_mask;
    private LayerMask obstacle_mask;
    private float SphereCastRadius;
                                        // left, right, up, down
    private Vector3[] altDirections = {new Vector3(-1,0,1), new Vector3(1,0,1), new Vector3(0,1,1), new Vector3(0,-1,1)};

    public void Awake() {
        fishTransform = transform;
        fish_mask = LayerMask.GetMask("Fish Boids");
        obstacle_mask = LayerMask.GetMask("Obstacle");
    }

    public void SetShoal(Shoal s) {
        shoal = s;
    }

    public void InitializeSpeed(float speed){
        this.speed = speed;
    }

    public void SetSphereCastRadius(float s) {
        SphereCastRadius = s;
    }

    // basic move function
    public void Swim() {
        //Debug.Log("Swimming");

        // get the nearby fish and calculate steer vectors
        FindNeighbours();
        Vector3 cohesion = CalculateCohesion();
        Vector3 alignment = CalculateAlignment();
        Vector3 avoidance = CalculateAvoidance();

        // calculate the direction to swim
        Vector3 moveVector;

        if(Physics.Raycast(fishTransform.position, fishTransform.forward, 2.5f, obstacle_mask)) {
            Debug.Log("Something in way");
            //Debug.DrawRay(transform.position, fishTransform.forward * 2.5f, Color.green ,3f);
            moveVector = AvoidObstacle();
        } else {
            moveVector = cohesion + alignment + avoidance;
            //Debug.Log("Move Vector: " + moveVector);
            moveVector = Vector3.SmoothDamp(fishTransform.forward, moveVector, ref currentVelocity, SmoothDamp);
            //Debug.Log("Move Vector Dampened: " + moveVector);
            moveVector = moveVector.normalized * speed;
            //Debug.Log("Final: " + moveVector);
        }

        // turn fish towards direction and move 
        fishTransform.forward = moveVector;
        Debug.DrawRay(transform.position, moveVector, Color.red);
        fishTransform.position += moveVector * Time.deltaTime;
    }

    private Vector3 AvoidObstacle() {
        for(int i = 0; i < altDirections.Length; i++) {
            Vector3 vec = fishTransform.InverseTransformDirection(altDirections[i]);
            Debug.Log("Trying: " + altDirections[i] + " , " + vec);
            Debug.DrawRay(transform.position, fishTransform.forward, Color.green ,3f);
            if(!Physics.Raycast(fishTransform.position, vec, 2.5f, obstacle_mask)){
                Debug.Log("Path found!");
                return vec;
            }
                
        }

        Debug.Log("No Path, reversing");
        return fishTransform.InverseTransformDirection(new Vector3(0,0,-1));
    }

    /*
        Find the nearby fish and determine which vector they should influence
    */
    private void FindNeighbours() {
        // clear saved neighbours
        cohesionNeighbours.Clear();
        alignNeighbours.Clear();
        avoidNeighbours.Clear();

        RaycastHit[] hit = Physics.SphereCastAll(fishTransform.position, SphereCastRadius, fishTransform.forward, 0, fish_mask);
        for(int i = 0; i < hit.Length; i++) {
            float distance = Vector3.SqrMagnitude(hit[i].transform.position - fishTransform.position);

            if(distance <= shoal.getCohesion * shoal.getCohesion)
                cohesionNeighbours.Add(hit[i].collider.GetComponent<Fish>());
            if(distance <= shoal.getAlignment * shoal.getAlignment)
                alignNeighbours.Add(hit[i].collider.GetComponent<Fish>());
            if(distance <= shoal.getAvoidance * shoal.getAvoidance)
                avoidNeighbours.Add(hit[i].collider.GetComponent<Fish>());
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward * 5);
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0, 5);
    }

    /*
        Calculate the correct speed for this fish
    */
    private void CalculateSpeed() {

        // if no cohesion neighbours then dont change speed
        if(cohesionNeighbours.Count == 0)
            return;

        // reset the movement speed then add all the speed values of nearby fish
        speed = 0;
        for(int i = 0; i < cohesionNeighbours.Count; i++) {
            speed += cohesionNeighbours[i].speed;
        }

        // get the average speed and clamp between the min and max speeds
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, shoal.minSpeed, shoal.maxSpeed);
    }

    /*
        Calculate the cohesion influence on this fish.
        This will influence invididual fish to steer towards the general
        center of the fish in its FOV.
    */
    private Vector3 CalculateCohesion() {
        var cohesion = Vector3.zero;

        if(cohesionNeighbours.Count == 0)
            return cohesion;

        for(int i = 0; i < cohesionNeighbours.Count; i++)
            cohesion += cohesionNeighbours[i].fishTransform.position;
        
        cohesion /= cohesionNeighbours.Count;
        cohesion -= fishTransform.position;
        cohesion = Vector3.Normalize(cohesion);

        return cohesion;
    }

    /*
        Calculate the alignment influence on this fish.
        This will influence invdividual fish to orientate
        towards the average direction of other fish in its FOV.
    */
    private Vector3 CalculateAlignment()
    {
        var alignment = fishTransform.forward;

        if(alignNeighbours.Count == 0)
            return alignment;

        for (int i = 0; i < alignNeighbours.Count; i++) {
            alignment += alignNeighbours[i].fishTransform.forward;
        }

        alignment /= alignNeighbours.Count;
        alignment = alignment.normalized;
        return alignment;
    }

    /*
        Calculate the avoidance influence on this fish.
        This will influence invidual fish to space themselves
        from fish in their FOV.
    */
    private Vector3 CalculateAvoidance()
    {
        var avoid = Vector3.zero;
        if(avoidNeighbours.Count == 0)
            return avoid;

        for (int i = 0; i < avoidNeighbours.Count; i++) 
            avoid += (fishTransform.position - avoidNeighbours[i].fishTransform.position);

        avoid /= avoidNeighbours.Count;
        avoid = avoid.normalized;
        return avoid;
    }
}
