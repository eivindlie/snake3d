using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float damping = 1;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float currentX = transform.eulerAngles.x;
        float desiredX = target.transform.eulerAngles.x;
        float x = Mathf.LerpAngle(currentX, desiredX, Time.deltaTime * damping);

        float currentY = transform.eulerAngles.y;
        float desiredY = target.transform.eulerAngles.y;
        float y = Mathf.LerpAngle(currentY, desiredY, Time.deltaTime * damping);
        
        float currentZ = transform.eulerAngles.z;
        float desiredZ = target.transform.eulerAngles.z;
        float z = Mathf.LerpAngle(currentZ, desiredZ, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, y, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }
}