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
    private float strafeGlideReduction = 0.111f;

    Rigidbody rb;
    PlayerInput playerInput;
    // Input Values
    [SerializeField]
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private float pitchYaw1D;
    private float thrustStrafe1D;
    private float glide = 0f;
    private float upDownGlide = 0f;
    private float strafeGlide = 0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

    }


    void FixedUpdate() {
        HandleMovement();
    }
 
    void HandleMovement(){
        // Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        // Pitch -- up down, invert pitchYaw1D here to invert y axis controls :)
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw1D, -1f, 1f) * pitchTorque * Time.deltaTime);

        // Thrusters
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust = thrust;
            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide *= thrust * thrust1D;
        } else if (glide != 0f)
        {
            // Once thrust buttom has stopped being pressed, over time, reduce forward momentum to 0
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }
        // Landing Mode
        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.fixedDeltaTime);
            upDownGlide *= upDown1D * upThrust;
        }
        else if (upDownGlide != 0f)
        {
            rb.AddRelativeForce(Vector3.forward * upDownGlide * Time.deltaTime);
            upDownGlide *= upDownGlideReduction;
        }
        // Left / Right
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.fixedDeltaTime);
            strafeGlide *= strafe1D * upThrust;
        }
        else if (strafeGlide != 0f)
        {
            rb.AddRelativeForce(Vector3.right * strafeGlide * Time.deltaTime);
            strafeGlide *= strafeGlideReduction;
        }
        // Forward / Back
        if (thrustStrafe1D > 0.1f || thrustStrafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.forward * thrustStrafe1D * strafeThrust * Time.fixedDeltaTime);
            glide *= thrustStrafe1D * strafeThrust;
        }
        else if (glide != 0f)
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }
    }

    #region Input Methods
    public void onThrust(InputAction.CallbackContext context){
        thrust1D = context.ReadValue<float>();
    }
    public void onStrafe(InputAction.CallbackContext context){
        strafe1D = context.ReadValue<float>();
    }
    public void onUpDown(InputAction.CallbackContext context){
        upDown1D = context.ReadValue<float>();
    }
    public void onRoll(InputAction.CallbackContext context){
        roll1D = context.ReadValue<float>();
    }
    public void onPitchYaw(InputAction.CallbackContext context){
        pitchYaw1D = context.ReadValue<float>();
    }
    public void onThrustLandingMode(InputAction.CallbackContext context){
       thrustStrafe1D = context.ReadValue<float>();
    }
    #endregion
}
