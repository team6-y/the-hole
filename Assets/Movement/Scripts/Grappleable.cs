using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrappleable
{
    
}

public enum GrappleState {
    Aiming,
    Shooting,
    Reeling
}

[RequireComponent(typeof(LineRenderer))]
public class Grappleable : MonoBehaviour, IGrappleable
{
    public Transform pointer;

    public GameObject targetPoint;

    public float range = 10f;

    public float shootSpeed = 100f;

    public float reelSpeed = 100f;



    private GrappleState state;

    private InputManager inputManager;

    public Rigidbody rb;

    private Vector3 reelDir;
    private Vector3 hookPos;

    private LineRenderer lineRenderer;
    private Vector3[] lineVertices = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        state = GrappleState.Aiming;
        inputManager = InputManager.GetInstance();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(pointer.position, pointer.position + pointer.forward * range, Color.cyan);        


        switch (state) {
            case GrappleState.Aiming:
                AimHook();
                break;
            case GrappleState.Shooting:
                ShootHook();
                break;
            // case GrappleState.Reeling:
            //     ReelHook();
            //     break;
            default:
                break;
        }
    }

    void FixedUpdate() {
        if (state == GrappleState.Reeling) ReelHook();
    } 


    void AimHook() {
        lineRenderer.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, range)){
            targetPoint.SetActive(true);
            targetPoint.transform.position = hit.point;
            if (inputManager.PlayerHoldingTrigger()) {
                state = GrappleState.Shooting;

                lineVertices[0] = rb.transform.position - new Vector3(0, 1, 0);
                hookPos = lineVertices[0];
                lineVertices[1] = hookPos;
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(lineVertices);
            }
        } else {
            targetPoint.SetActive(false);
        }
    }

    void ShootHook() {
        if (!inputManager.PlayerHoldingTrigger()) state = GrappleState.Aiming;
        hookPos = Vector3.MoveTowards(hookPos, targetPoint.transform.position, shootSpeed * Time.deltaTime);
        lineRenderer.SetPosition(0, hookPos);
        lineRenderer.SetPosition(1, transform.position - new Vector3(0, 1, 0));
        if (Vector3.Distance(hookPos, targetPoint.transform.position) < float.Epsilon) state = GrappleState.Reeling;
    }

    void ReelHook() {
        if (!inputManager.PlayerHoldingTrigger()) state = GrappleState.Aiming;
        reelDir = Vector3.Normalize(targetPoint.transform.position - rb.transform.position);
        lineRenderer.SetPosition(1, rb.transform.position - new Vector3(0, 1, 0));
        rb.AddForce(reelDir * reelSpeed);
        // transform.position = rb.transform.position;
    }
}
