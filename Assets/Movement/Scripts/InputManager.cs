using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager GetInstance() {
        return _instance;
    }

    private PlayerControls playerControls;
    private void Awake() {

        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        playerControls = new PlayerControls();

    }

    private void OnEnable() {
        playerControls.Enable();    
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    public Vector2 GetCameraDelta() {
        return playerControls.NonVR.Direction.ReadValue<Vector2>();
    }

    public bool PlayerHoldingTrigger() {
        return playerControls.NonVR.Shoot.IsPressed();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
