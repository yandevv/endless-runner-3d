using UnityEngine;

public class SpeedScaler : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float maxSpeed = 30f;
    public float acceleration = 0.5f;
    public static float CurrentSpeed { get; private set; }

    void Start() => CurrentSpeed = baseSpeed; // reset on every (re)load — fixes the static leak

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;
        CurrentSpeed = Mathf.Min(CurrentSpeed + acceleration * Time.deltaTime, maxSpeed);
    }
}