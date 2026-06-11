using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 8f;
    public float laneSpeed = 8f;
    public float laneLimit = 3f;

    [Header("Slide")]
    public float slideDuration = 0.8f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded = true;
    private bool isSliding = false;
    private float slideTimer;

    private float normalHeight;
    private Vector3 normalCenter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        normalHeight = capsule.height;
        normalCenter = capsule.center;
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
            if (animator != null) animator.SetTrigger("Jump");
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
        // shrink only the collider so we pass under SlideBars; the model is untouched
        capsule.height = normalHeight * 0.5f;
        capsule.center = new Vector3(
            normalCenter.x,
            normalCenter.y - normalHeight * 0.25f,
            normalCenter.z);
        if (animator != null) animator.SetBool("IsSliding", true);
    }

    void EndSlide()
    {
        isSliding = false;
        capsule.height = normalHeight;
        capsule.center = normalCenter;
        if (animator != null) animator.SetBool("IsSliding", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
        if (collision.gameObject.CompareTag("Obstacle")) GameManager.Instance.GameOver();
    }
}