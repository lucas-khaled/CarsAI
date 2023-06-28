using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carros : MonoBehaviour
{
    [Header("Values to change")]

    [SerializeField] private float maxVelocity;
    [SerializeField] private float minDistanceToDeaccelerate;
    [SerializeField] private float acceleration;

    [Header("ConstantValues")]
    [SerializeField] List<Transform> points;
    [SerializeField] float raycastDistance = 10;
    [SerializeField] private float raycastAngleVariation = -45;
    [SerializeField] private float maxBreakSpeed = 0.5f;

    private Vector3 destiny;
    private Vector3 currentVelocity;
    private Rigidbody rb;
    private int actualDestiny;

    private void Start()
    {
        actualDestiny = 0;
        destiny = points[actualDestiny].position;
        UpdateDestiny();
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        UpdatePosition();
        DoBehaviour();
        Debug.DrawLine(transform.position, destiny, Color.black);
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;

        newPosition.y = 0.06f;
        transform.LookAt(newPosition);
        transform.position = newPosition;
    }

    private void ChooseBestDirection(Vector3 desiredDir, Vector3 raycastDir, ref float bestSolutionValue, ref Vector3 bestDir)
    {
        RaycastHit hit;

        float angle = Mathf.Acos(Vector3.Dot(desiredDir, raycastDir));
        float raycastDistanceReached = raycastDistance;

        if (Physics.Raycast(transform.position, raycastDir, out hit, raycastDistance))
        {
            raycastDistanceReached = Vector3.Distance(transform.position, hit.point);
        }

        float solutionValue = raycastDistanceReached + (1.5f - angle);

        if (solutionValue > bestSolutionValue)
        {
            bestSolutionValue = solutionValue;
            bestDir = raycastDir;
        }
    }

    private Vector3 ChooseBestDirection()
    {
        RaycastHit hit;
        Vector3 desiredDirection;
        Vector3 velocityNormalized = currentVelocity.normalized;

        float distanceToTarget = Vector3.Distance(transform.position, destiny);

        Vector3 targetPosition = destiny;

        desiredDirection = (targetPosition - transform.position).normalized;

        float rayTargetDistance = Vector3.Distance(transform.position, targetPosition);

        rayTargetDistance = (rayTargetDistance < raycastDistance) ? rayTargetDistance : raycastDistance;

        if (!Physics.Raycast(transform.position, desiredDirection, out hit, rayTargetDistance))
            return desiredDirection;

        float bestSolutionValue = Vector3.Distance(transform.position, hit.point);
        Vector3 bestDir = desiredDirection;

        Vector3 leftDiagonal = -this.transform.right + this.transform.forward;
        Vector3 rightDiagonal = this.transform.right + this.transform.forward;
        Vector3 left = -this.transform.right;
        Vector3 right = this.transform.right;
        Vector3 forward = this.transform.forward;

        Debug.DrawRay(transform.position, forward, Color.blue);
        Debug.DrawRay(transform.position, rightDiagonal, Color.magenta);
        Debug.DrawRay(transform.position, right, Color.red);
        Debug.DrawRay(transform.position, left, Color.green);
        Debug.DrawRay(transform.position, leftDiagonal, Color.yellow);

        ChooseBestDirection(desiredDirection, rightDiagonal, ref bestSolutionValue, ref bestDir);
        ChooseBestDirection(desiredDirection, leftDiagonal, ref bestSolutionValue, ref bestDir);
        ChooseBestDirection(desiredDirection, left, ref bestSolutionValue, ref bestDir);
        ChooseBestDirection(desiredDirection, right, ref bestSolutionValue, ref bestDir);
        ChooseBestDirection(desiredDirection, forward, ref bestSolutionValue, ref bestDir);

        return bestDir;
    }
    
    private void DoBehaviour()
    {
        float distance = Vector3.Distance(destiny, transform.position);

        Vector3 directionToTarget = ChooseBestDirection();

        UpdateVelocity(directionToTarget * acceleration);

        if (distance <= minDistanceToDeaccelerate)
        {
            float newSpeed = (maxBreakSpeed * distance) / minDistanceToDeaccelerate;
            newSpeed = (newSpeed > 0.5f) ? newSpeed : 0.5f;

            currentVelocity = currentVelocity.normalized * newSpeed;
        }
        else
            maxBreakSpeed = currentVelocity.magnitude;
    }

    private void UpdateVelocity(Vector3 SteeringForce)
    {
        float mass = GetComponent<Rigidbody>().mass;
        Vector3 acceleration = SteeringForce / mass;

        currentVelocity += acceleration * Time.deltaTime;
        currentVelocity = Truncate(currentVelocity, maxVelocity);
    }

    private Vector3 Truncate(Vector3 dir, float maxMagnitude)
    {
        Vector3 normalizedVector = dir.normalized;

        if (dir.magnitude <= maxMagnitude)
            return dir;

        return normalizedVector * maxMagnitude;
    }

    public void UpdateDestiny()
    {
        if (actualDestiny + 1 > points.Count)
            actualDestiny = 0;
        else
            actualDestiny += 1;

        destiny = points[actualDestiny].position;
        Debug.Log(gameObject.name + " destino atualizado para " + destiny);
    }
}
