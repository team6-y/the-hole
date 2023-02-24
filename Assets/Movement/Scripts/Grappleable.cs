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


    public GameObject targetPointPrefab;

    private GameObject targetPoint;
    private Vector3 targetPos;

    public float range = 10f;

    public float shootSpeed = 100f;

    public float reelSpeed = 100f;

    public Material cannotFireMaterial;
    
    public Material canFireMaterial;
    public Material reelMaterial;

    private GrappleState state;

    private InputManager inputManager;

    public Rigidbody rb;

    private Vector3 reelDir;
    private Vector3 hookPos;

    enum Hand {
        Left,
        Right
    }

    [SerializeField] private Hand hand;

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
        Debug.DrawLine(pointer.position, pointer.position + pointer.forward * range, Color.cyan);        


        switch (state) {
            case GrappleState.Aiming:
                AimHook();
                break;
            case GrappleState.Shooting:
                ShootHook();
                break;
            case GrappleState.Reeling:
                lineRenderer.SetPosition(1, pointer.position);
                break;
            default:
                break;
        }
    }

    void FixedUpdate() {
        if (state == GrappleState.Reeling) ReelHook();
    } 


    void AimHook() {
        lineRenderer.enabled = true;
        RaycastHit hit;
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, range)){
            targetPoint.SetActive(true);
            targetPoint.transform.position = hit.point;

            lineRenderer.material = cannotFireMaterial;
            lineVertices[0] = hit.point;
            lineVertices[1] = pointer.position;

            lineRenderer.material = canFireMaterial;

            if (checkForShoot()) {
                state = GrappleState.Shooting;

                lineVertices[0] = pointer.position;
                hookPos = lineVertices[0];
                lineVertices[1] = hookPos;

                lineRenderer.material = reelMaterial;
            }


        } else {
            targetPoint.SetActive(false);

            lineRenderer.material = cannotFireMaterial;
            lineVertices[0] = pointer.position + pointer.forward * range;
            lineVertices[1] = pointer.position;
        }

        lineRenderer.SetPositions(lineVertices);
    }

    void ShootHook() {
        if (!checkForShoot()) state = GrappleState.Aiming;
        targetPos = targetPoint.transform.position;
        targetPoint.SetActive(false);
        hookPos = Vector3.MoveTowards(hookPos, targetPos, shootSpeed * Time.deltaTime);
        lineRenderer.SetPosition(0, hookPos);
        lineRenderer.SetPosition(1, pointer.position);
        if (Vector3.Distance(hookPos, targetPos) < float.Epsilon) state = GrappleState.Reeling;
    }

    void ReelHook() {
        if (!checkForShoot()) state = GrappleState.Aiming;
        reelDir = Vector3.Normalize(targetPos - pointer.position);
        // lineRenderer.SetPosition(1, pointer.position);
        rb.AddForce(reelDir * reelSpeed);
    }
}
