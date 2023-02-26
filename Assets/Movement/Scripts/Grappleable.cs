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
    [Header("Object References")]
    public Transform pointer;
    public GameObject targetPointPrefab;
    public Rigidbody affectedRigidbody;
    
    [Header("Variables")]
    public float range = 30f;
    public float shootSpeed = 25f;
    public float reelSpeed = 50f;

    [Header("Materials")]
    public Material cannotShootMaterial;
    public Material canShootMaterial;
    public Material reelMaterial;

    private GrappleState state;
    private InputManager inputManager;

    private GameObject targetPoint;
    private Vector3 targetPos;
    private Vector3 reelDir;
    private Vector3 hookPos;

    enum Hand {Left, Right}
    [Header("Left/Right Hand")]
    [SerializeField]
    private Hand hand;

    private delegate bool CheckForShoot();
    private CheckForShoot checkForShoot;

    private LineRenderer lineRenderer;
    private Vector3[] lineVertices = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        state = GrappleState.Aiming;
        inputManager = InputManager.GetInstance();
        lineRenderer = GetComponent<LineRenderer>();
        targetPoint = Instantiate(targetPointPrefab);
        targetPoint.SetActive(false);

        if (hand == Hand.Left) {
            checkForShoot = inputManager.PlayerHoldingTriggerL;
        } else {
            checkForShoot = inputManager.PlayerHoldingTriggerR;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case GrappleState.Aiming:
                AimHook();
                break;
            case GrappleState.Shooting:
                ShootHook();
                break;
            case GrappleState.Reeling:
                break;
            default:
                break;
        }

        RenderReelLine();

        Debug.DrawLine(pointer.position, pointer.position + pointer.forward * range, Color.cyan); // Debug line showing pointer direction
    }

    void FixedUpdate() {
        // ReelHook logic is in fixed update because it involves physics
        if (state == GrappleState.Reeling) ReelHook();
    } 

    void ChangeState(GrappleState newState) {

        targetPoint.SetActive(false);

        switch (newState) {
            case GrappleState.Aiming:
                hookPos = pointer.position;
                break;
            case GrappleState.Shooting:
                hookPos = pointer.position;
                break;
            case GrappleState.Reeling:
                break;
            default:
                break;
        }

        state = newState;
    }

    void AimHook() {
        RaycastHit hit;
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, range)){
            targetPoint.SetActive(true);
            targetPoint.transform.position = hit.point;
            targetPos = targetPoint.transform.position;
            if (checkForShoot()) ChangeState(GrappleState.Shooting);

        } else {
            targetPoint.SetActive(false);
        }
    }

    void ShootHook() {
        if (!checkForShoot()) ChangeState(GrappleState.Aiming);
        hookPos = Vector3.MoveTowards(hookPos, targetPos, shootSpeed * Time.deltaTime);
        if (Vector3.Distance(hookPos, targetPos) < float.Epsilon) state = GrappleState.Reeling;
    }

    void ReelHook() {
        if (!checkForShoot()) ChangeState(GrappleState.Aiming);
        reelDir = Vector3.Normalize(targetPos - pointer.position);
        affectedRigidbody.AddForce(reelDir * reelSpeed);
    }

    void RenderReelLine() {
        // lineVertices[0] is the far end of the line
        switch (state) {
            case GrappleState.Aiming:
                lineRenderer.enabled = true;
                if (targetPoint.activeInHierarchy) {
                    // if target point is active, it means player can shoot
                    lineRenderer.material = canShootMaterial;
                    // set far end of line to target position
                    lineVertices[0] = targetPos;
                } else {
                    // if target point is not active, it means player cannot shoot
                    lineRenderer.material = cannotShootMaterial;
                    // set far end of line to <range> distance away from pointer
                    lineVertices[0] = pointer.position + pointer.forward * range;
                }
                break;
            case GrappleState.Shooting:
                // in this state, far end of line moves with the hook
                lineRenderer.material = reelMaterial;
                lineVertices[0] = hookPos;
                break;
            case GrappleState.Reeling:
                // in this state, hook should not be moving, so no updates to lineVertices[0]
                lineVertices[0] = hookPos;
                break;
            default:
                break;
        }

        // lineVertices[1] is the origin position of the grappling gun
        lineVertices[1] = pointer.position;

        // Set the positions in the Line Renderer Component
        lineRenderer.SetPositions(lineVertices);
    }

    void OnDisable() {
        targetPoint.SetActive(false);
        lineRenderer.enabled = false;
    }

    void OnEnable() {
        lineRenderer.enabled = true;
    }

}
