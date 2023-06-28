using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class CarAI : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 20;
    [SerializeField] private int numOfRaycast = 5;
    [FormerlySerializedAs("raycastSize")] [SerializeField] private float breakDistance = 10;
    [FormerlySerializedAs("carForce")] [SerializeField] private float carSteeringForce = 100;
    [SerializeField] private float breakForce = 50;
    
    private Rigidbody rb;
    private float toFrontDistance = float.MaxValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsWrongWay())
            Turn();
        else
            UpdateVelocity();
    }

    private Vector3 ChooseDirection(out float distance)
    {
        Vector3 frontDir = transform.forward;
        Vector3 direction = frontDir;
        if (numOfRaycast % 2 != 0)
            numOfRaycast--;

        distance = 0;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, breakDistance))
        {
            distance = Vector3.Distance(transform.position, hit.point);
            toFrontDistance = distance;
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            distance = toFrontDistance = breakDistance;
            Debug.DrawLine(transform.position, transform.position+transform.forward*breakDistance, Color.red);
            return direction;
        }

        float angleVariation = 180 / numOfRaycast;
        float angle = 90;

        for (int i = 1; i <= numOfRaycast; i++)
        {
            if (angle == 0)
                angle -= angleVariation;
            
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 dir = (rot * frontDir).normalized;

            if (Physics.Raycast(transform.position, dir, out hit, breakDistance))
            {
                Debug.DrawLine(transform.position, hit.point, Color.cyan);
                if (Vector3.Distance(transform.position, hit.point) > distance)
                {
                    direction = dir;
                    distance = Vector3.Distance(transform.position, hit.point);
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position+dir*breakDistance, Color.red);
                distance = breakDistance;
                continue;
            }

            angle -= angleVariation;
        }

        return direction;
    }
    
    
    private void UpdateVelocity()
    {
        float distance;
        Vector3 steeringForce = ChooseDirection(out distance) * carSteeringForce;
        Vector3 vel = rb.velocity;
        vel.y = 0;
        
        if (toFrontDistance < breakDistance && (toFrontDistance < distance))
        {
            float distanceAverage = (distance + toFrontDistance) * 0.5f;
            Vector3 breakForce = -vel * this.breakForce/distanceAverage;
            rb.AddForce(breakForce);
        }

        //Debug.DrawLine(transform.position, transform.position+steeringForce,Color.yellow);
        
        if(Vector3.Dot(steeringForce, vel) < 0.5f || vel.magnitude < maxVelocity)
            rb.AddForce(steeringForce, ForceMode.Force);
        
        transform.LookAt(transform.position + vel);
    }

    private bool IsWrongWay()
    {
        RaceCheckPoint check = GetComponent<Car>().CheckPoint;
        if (check == null) return false;

        return Vector3.Dot(transform.forward, (check.transform.position - transform.position).normalized) <= 0;
    }

    private void Turn()
    {
        RaceCheckPoint check = GetComponent<Car>().CheckPoint;
        float angle = Vector3.Angle(transform.forward, (check.transform.position - transform.position).normalized);
        
        rb.AddForce(transform.forward * carSteeringForce * 0.3f);
        transform.Rotate(Vector3.up, angle * 0.1f);
    }
}
