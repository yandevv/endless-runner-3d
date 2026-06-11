using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 8f;
    public float laneSpeed = 8f;   // left/right dodge speed
    public float laneLimit = 3f;   // how far sideways you can go

    [Header("Slide")]
    public float slideDuration = 0.8f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isSliding = false;
    private float slideTimer;
    private Vector3 normalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (keyboard.leftCtrlKey.wasPressedThisFrame && isGrounded && !isSliding)
            StartSlide();

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f) EndSlide();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        // Forward on Z, dodge on X, leave Y to gravity/jump
        Vector3 v = rb.linearVelocity;          // use rb.velocity on Unity 2022 LTS or older
        v.z = SpeedScaler.CurrentSpeed;
        v.x = GetHorizontal() * laneSpeed;
        rb.linearVelocity = v;

        // Clamp to the lanes
        Vector3 p = rb.position;
        p.x = Mathf.Clamp(p.x, -laneLimit, laneLimit);
        rb.position = p;
    }

    float GetHorizontal()
    {
        float h = 0f;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) h -= 1f;
            if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) h += 1f;
        }

        var gamepad = Gamepad.current;
        if (gamepad != null && Mathf.Abs(h) < 0.01f)
            h = gamepad.leftStick.x.ReadValue();

        return h;
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        transform.localScale = new Vector3(normalScale.x, normalScale.y * 0.5f, normalScale.z);
    }

    void EndSlide()
    {
        isSliding = false;
        transform.localScale = normalScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
        if (collision.gameObject.CompareTag("Obstacle")) GameManager.Instance.GameOver();
    }
}