using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraAnchor;
    private Vector3 cameraOffset;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    
    void Start()
    {
        cameraOffset = transform.position - cameraAnchor.position;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>() * Time.deltaTime;
        cameraAngles.x += lookValue.y;
        cameraAngles.y += lookValue.x;
    }

    void LateUpdate()
    {
        transform.eulerAngles = cameraAngles;
        transform.position = cameraAnchor.position + Quaternion.Euler(cameraAngles) * cameraOffset;
    }
}