using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipRig : MonoBehaviour {

    [Header("~~Ship Movement Options~~")]
    [SerializeField]
    private float yawTorque = 500f;
    [SerializeField]
    private float pitchTorque = 1000f;
    [SerializeField]
    private float rollTorque = 1000f;
    [SerializeField]
    private float thrust = 100f;
    [SerializeField]
    private float upThrust = 50f;
    [SerializeField]
    private float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.111f;

    Rigidbody rb;
    // Input Values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    [SerializeField]
    private float pitchYaw1D;
    [SerializeField]
    private bool landingMode = false;
    private float thrustStrafe1D;
    
    
    void Start(){
        rb = GetComponent<Rigidbody>();

    }


    void FixedUpdate(){
        HandleMovement();
    }

    void HandleMovement(){
        // Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        // Pitch -- up down, invert pitchYaw1D here to invert y axis controls :)
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(pitchYaw1D, -1f, 1f) * pitchTorque * Time.deltaTime);

        // Thrusters
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust;
        }
    }

    #region Input Methods
    public void onThrust(InputAction.CallbackContext context){
        thrust1D = context.ReadValue<float>();
    }
    public void onStrafe(InputAction.CallbackContext context){
        // Make sure landing mode is engaged, might be inefficient
        if (landingMode) strafe1D = context.ReadValue<float>();
    }
    public void onUpDown(InputAction.CallbackContext context){
        // Make sure landing mode is engaged
        if (landingMode) upDown1D = context.ReadValue<float>();
    }
    public void onRoll(InputAction.CallbackContext context){
        roll1D = context.ReadValue<float>();
    }
    public void onPitchYaw(InputAction.CallbackContext context){
        pitchYaw1D = context.ReadValue<float>();
    }
    // May not be nessacary
    public void onLandingMode(InputAction.CallbackContext context){
        landingMode = !landingMode;
        // trigger UI element ?
    }
    public void onThrustLandingMode(InputAction.CallbackContext context){
        if (landingMode) thrustStrafe1D = context.ReadValue<float>();
    }
    #endregion
}
