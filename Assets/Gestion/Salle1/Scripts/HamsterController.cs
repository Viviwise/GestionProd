using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public float speed = 3f;
    public Transform cameraTransform; 

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);

        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x);

        transform.position += moveDirection * speed * Time.deltaTime;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}