using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float degreesPerSecond = 90f;
    [SerializeField] private float bobAmplitude = 0.1f;
    [SerializeField] private float bobSpeed = 1f;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.Rotate(axis.normalized, degreesPerSecond * Time.deltaTime, Space.Self);

        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.localPosition = startLocalPosition + Vector3.up * bobOffset;
    }
}
    