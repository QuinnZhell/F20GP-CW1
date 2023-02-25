using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float FOV;
    [SerializeField] private float SmoothDamp;
    private List<Fish> cohesionNeighbours = new List<Fish>();
    private List<Fish> avoidNeighbours = new List<Fish>();
    private List<Fish> alignNeighbours = new List<Fish>();
    private Shoal shoal;
    private Vector3 currentVelocity;
    public float speed;
    public Transform fishTransform {get;set;}

    public void Awake() {
        fishTransform = transform;
    }

    public void SetShoal(Shoal s) {
        shoal = s;
    }

    public void InitializeSpeed(float speed){
        this.speed = speed;
    }

    public void Swim() {
        //Debug.Log("Swimming");
        FindNeighbours();
        Vector3 cohesion = CalculateCohesion();
        Vector3 alignment = CalculateAlignment();
        Vector3 avoidance = CalculateAvoidance();

        var moveVector = cohesion + alignment + avoidance;
        moveVector = Vector3.SmoothDamp(fishTransform.forward, cohesion, ref currentVelocity, SmoothDamp);
        moveVector = moveVector.normalized * speed;

        fishTransform.forward = moveVector;
        fishTransform.position += moveVector * Time.deltaTime;
    }

    

    private void FindNeighbours() {
        cohesionNeighbours.Clear();

        var siblings = shoal.shoalMembers;
        for(int i = 0; i < siblings.Length; i++) {
            var currentSib = siblings[i];
            if (currentSib != this) {

                float distanceToSib = Vector3.SqrMagnitude(currentSib.fishTransform.position - fishTransform.position);
                if(distanceToSib <= shoal.getCohesion * shoal.getCohesion){
                    cohesionNeighbours.Add(currentSib);
                }
                if(distanceToSib <= shoal.getAlignment * shoal.getAlignment){
                    avoidNeighbours.Add(currentSib);
                }
                if(distanceToSib <= shoal.getAvoidance * shoal.getAvoidance){
                    alignNeighbours.Add(currentSib);
                }
            }
        }
    }

    private bool IsInFOV(Vector3 position) {
        return Vector3.Angle(fishTransform.forward, position - fishTransform.position) <= FOV;
    }

    private void CalculateSpeed() {
        if(cohesionNeighbours.Count == 0)
            return;

        speed = 0;

        for(int i = 0; i < cohesionNeighbours.Count; i++) {
            speed += cohesionNeighbours[i].speed;
        }

        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, shoal.minSpeed, shoal.maxSpeed);
    }

    private Vector3 CalculateCohesion() {
        var cohesion = Vector3.zero;

        if(cohesionNeighbours.Count == 0)
            return cohesion;

        int neighboursInFOV = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++) {
            if(IsInFOV(cohesionNeighbours[i].fishTransform.position)) {
                neighboursInFOV++;
                cohesion += cohesionNeighbours[i].fishTransform.position;
            }
        }

        if (neighboursInFOV == 0)
            return cohesion;
        
        cohesion /= neighboursInFOV;
        cohesion -= fishTransform.position;
        cohesion = Vector3.Normalize(cohesion);

        return cohesion;
    }

    private Vector3 CalculateAlignment()
    {
        var alignment = fishTransform.forward;
        int neighboursInFOV
    }

    private Vector3 CalculateAvoidance()
    {
        throw new NotImplementedException();
    }
}
