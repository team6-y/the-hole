using UnityEngine;

public class NonVRController : MonoBehaviour
{
    public Camera cam;
    private Transform camTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    private InputManager inputManager;

    private float xRot, yRot;
    private void Start()
    {
        camTransform = cam.gameObject.transform;
        inputManager = InputManager.GetInstance();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ControlPosition();
        ControlCamera();
    }

    void ControlPosition(){
        // groundedPlayer = controller.isGrounded;
        // if (groundedPlayer && playerVelocity.y < 0)
        // {
        //     playerVelocity.y = 0f;
        // }

        // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // controller.Move(move * Time.deltaTime * playerSpeed);

        // // if (move != Vector3.zero)
        // // {
        // //     gameObject.transform.forward = move;
        // // }

        // // Changes the height position of the player..
        // if (Input.GetButtonDown("Jump") && groundedPlayer)
        // {
        //     playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        // }

        // // y velocity
        // playerVelocity.y += gravityValue * Time.deltaTime;
        // controller.Move(playerVelocity * Time.deltaTime);

    }

    void ControlCamera(){
        Vector2 camDelta = inputManager.GetCameraDelta();
        if (camDelta == new Vector2(0, 0)) return;
                    
        xRot -= camDelta.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        yRot += camDelta.x;

        camTransform.rotation = Quaternion.Euler(xRot, yRot, 0);
        // gameObject.transform.forward = camTransform.forward;

    }
}
