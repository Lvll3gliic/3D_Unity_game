using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float minChangeDirectionInterval = 3f;
    public float maxChangeDirectionInterval = 100f;
    public float changeDirectionInterval = 0f;
    public float minIdleTime = 1f; // Minimum duration for staying idle
    public float maxIdleTime = 60f;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    public float groundOffset = 0.1f;
    public LayerMask groundLayer;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float directionTimer = 0f;
    private bool isIdle = false;
    private float idleTimer = 0f;
    private float idleDuration = 0f;

    public bool isDead = false;

    private void Start()
    {
        changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
        SetRandomTargetPosition();
        SetRandomTargetRotation();
    }

    private void Update()
    {
        if (isIdle || isDead)
        {
            HandleIdleState();
            
        }
        else
        {
            MoveToTargetPosition();
            RotateTowardsTargetRotation();

            directionTimer += Time.deltaTime;
            if (directionTimer >= changeDirectionInterval)
            {
                directionTimer = 0f;
                isIdle = true;
                idleDuration = Random.Range(minIdleTime, maxIdleTime);
                Debug.Log(idleDuration);
            }
        }
    }

    private void HandleIdleState()
    {
        if (isDead)
            return;

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            idleTimer = 0f;
            idleDuration = 0f;
            isIdle = false;
            SetRandomTargetPosition();
            SetRandomTargetRotation();
        }
    }

    private void MoveToTargetPosition()
    {
        Vector3 clampedTargetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x),
            transform.position.y,
            Mathf.Clamp(targetPosition.z, minBounds.z, maxBounds.z)
        );

        transform.position = Vector3.MoveTowards(transform.position, clampedTargetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == clampedTargetPosition)
        {
            isIdle = true;
            idleDuration = Random.Range(minIdleTime, maxIdleTime);
            Debug.Log(idleDuration);
        }
    }

    private void RotateTowardsTargetRotation()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);
        targetPosition = new Vector3(randomX, 0f, randomZ);
    }

    private void SetRandomTargetRotation()
    {
        targetRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPosition = hit.point + Vector3.up * groundOffset;
            transform.position = newPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((minBounds + maxBounds) / 2f, maxBounds - minBounds);
    }

}
