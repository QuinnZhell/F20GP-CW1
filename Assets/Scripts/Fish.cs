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
    private int fish_mask;
    private int obstacle_mask;
    private float SphereCastRadius;
                                        // left, right, up, down
    private Vector3[] altDirections = {new Vector3(-1,0,1), new Vector3(1,0,1), new Vector3(0,1,1), new Vector3(0,-1,1)};

    public void Awake() {
        fishTransform = transform;
        fish_mask = LayerMask.GetMask("Fish Boids");
        obstacle_mask = LayerMask.GetMask("Obstacle");
    }

    // set the shoal this fish belongs sto
    public void SetShoal(Shoal s) {
        shoal = s;
    }

    // set its initial speed
    public void InitializeSpeed(float speed){
        this.speed = speed;
    }

    // set the radius it will check for flock members
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

        // raycast to check that the fish is not moving into an obstacle
        if(Physics.Raycast(fishTransform.position, fishTransform.forward, 2.5f, obstacle_mask)) {
            moveVector = AvoidObstacle();
        } else {
            // get a move vector from the steering components, move in that direction
            moveVector = cohesion + alignment + avoidance;
            moveVector = Vector3.SmoothDamp(fishTransform.forward, moveVector, ref currentVelocity, SmoothDamp);
            moveVector = moveVector.normalized * speed;
        }

        // turn fish towards direction and move 
        fishTransform.forward = moveVector;
        Debug.DrawRay(transform.position, moveVector, Color.red);
        fishTransform.position += moveVector * Time.deltaTime;
    }

    // find alternate paths to get around objects
    private Vector3 AvoidObstacle() {
        for(int i = 0; i < altDirections.Length; i++) {
            Vector3 vec = fishTransform.InverseTransformDirection(altDirections[i]);
            //Debug.Log("Trying: " + altDirections[i] + " , " + vec);
            //Debug.DrawRay(transform.position, fishTransform.forward, Color.green ,3f);
            if(!Physics.Raycast(fishTransform.position, vec, 2.5f, obstacle_mask)){
                //Debug.Log("Path found!");
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

        // gather information on nearby fish
        RaycastHit[] hit = Physics.SphereCastAll(fishTransform.position, SphereCastRadius, fishTransform.forward, 0, fish_mask);
        for(int i = 0; i < hit.Length; i++) {
            float distance = Vector3.SqrMagnitude(hit[i].transform.position - fishTransform.position);

            // if within cohesion distance
            if(distance <= shoal.getCohesion * shoal.getCohesion)
                cohesionNeighbours.Add(hit[i].collider.GetComponent<Fish>());

            // if within alignment distance
            if(distance <= shoal.getAlignment * shoal.getAlignment)
                alignNeighbours.Add(hit[i].collider.GetComponent<Fish>());

            // if within avoidance distance
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

        // get the sum of all fish to be considered in cohesion
        for(int i = 0; i < cohesionNeighbours.Count; i++)
            cohesion += cohesionNeighbours[i].fishTransform.position;
        
        // get the average
        cohesion /= cohesionNeighbours.Count;
        // remove the fishes own influence
        cohesion -= fishTransform.position;
        // change magnitude
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

        // get the sum of the align members directions
        for (int i = 0; i < alignNeighbours.Count; i++) {
            alignment += alignNeighbours[i].fishTransform.forward;
        }

        // get average
        alignment /= alignNeighbours.Count;
        // remove own influence
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

        // get the sum of distance between nearby fish members and itself
        for (int i = 0; i < avoidNeighbours.Count; i++) 
            avoid += (fishTransform.position - avoidNeighbours[i].fishTransform.position);

        // get the average avoidance distance
        avoid /= avoidNeighbours.Count;
        // normalize to reduce magnitude
        avoid = avoid.normalized;
        return avoid;
    }
}
