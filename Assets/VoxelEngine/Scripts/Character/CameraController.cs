using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float sensitivity = 5.0f;
    [SerializeField] float height = 2.0f;

    Transform cameraTransform;
    Vector2 mouseLook;
    Vector2 input;

    void Awake()
    {
        cameraTransform = transform;
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Get joystick look axes and correct for drift:
        var rightStickX = Input.GetAxisRaw("Right Stick X");
        var rightStickY = Input.GetAxisRaw("Right Stick Y");
        if (Mathf.Abs(rightStickX) < 0.05f) rightStickX = 0;
        if (Mathf.Abs(rightStickY) < 0.05f) rightStickY = 0;
        var rightStickInput = sensitivity * new Vector2(rightStickX, rightStickY);
        var mouseInput = sensitivity * new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //input *= sensitivity;
        input = mouseInput + rightStickInput;
        
        mouseLook += input;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);    
        
        Quaternion targetRotation = Quaternion.AngleAxis(mouseLook.x, target.transform.up);
        Quaternion cameraRotation = targetRotation * Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        
        target.rotation = targetRotation;
        cameraTransform.position = target.transform.position + Vector3.up * height;
        cameraTransform.rotation = cameraRotation;
    }
}