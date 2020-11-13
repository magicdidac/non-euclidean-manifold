using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller
{

    /* Controls */
    private Inputs controls;

    private bool runInput;
    private bool jumpInput;
    private Vector2 moveInput;
    private Vector2 lookInput;

    /* Look */
    private float yaw;
    private float pitch;
    [SerializeField] private float sensitivity = 0;
    [SerializeField] private float minPitch = -80.0f;
    [SerializeField] private float maxPitch = 50.0f;
    [SerializeField] private Transform pitchControllerTransform = null;

    /* Movement */
    private Rigidbody rb;
    [SerializeField] private float speed = 10.0f;
    private float currentSpeed;

    /* JUMP & RUN & GRAVITY*/
    [SerializeField] private float runMultiplier = 2;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravityForce = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        yaw = transform.rotation.eulerAngles.y;
        pitch = pitchControllerTransform.rotation.eulerAngles.x;

        controls = new Inputs();

        controls.Player.ToggleCursor.performed += _ => ToggleCursor();

        controls.Player.Sprint.performed += _ => currentSpeed = speed * runMultiplier;
        controls.Player.Sprint.canceled += _ => currentSpeed = speed;

        controls.Player.Jump.performed += _ => jumpInput = true;
        controls.Player.Jump.canceled += _ => jumpInput = false;

        controls.Player.Walk.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Walk.canceled += _ => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => lookInput = Vector2.zero;

        controls.Enable();

        currentSpeed = speed;

        // ToggleCursor();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ToggleCursor()
    {
        /*Cursor.visible = !Cursor.visible;
        Cursor.lockState = (Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;*/
    }

    private void FixedUpdate()
    {
        /* Look */
        float axisY = -lookInput.y;
        pitch += axisY * sensitivity * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        pitchControllerTransform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);

        float axisX = lookInput.x;
        yaw += axisX * sensitivity * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(0, yaw, 0);

        /* Movement */
        Vector3 totalMovement = transform.position;

        if (moveInput.y > 0)
            totalMovement += transform.forward * currentSpeed * Time.deltaTime;
        else if (moveInput.y < 0)
            totalMovement += -transform.forward * currentSpeed * Time.deltaTime;

        if (moveInput.x > 0)
            totalMovement += transform.right * currentSpeed * Time.deltaTime;
        else if (moveInput.x < 0)
            totalMovement += -transform.right * currentSpeed * Time.deltaTime;

        rb.MovePosition(totalMovement);


        /* Jump */
        if (jumpInput && Mathf.Abs(rb.velocity.y) <= 0)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        /* Gravity */
        rb.AddForce(-transform.up * 9.81f * gravityForce);

    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(yaw, eulerRot.y);
        yaw += delta;
        transform.eulerAngles = Vector3.up * yaw;
        //velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }

}