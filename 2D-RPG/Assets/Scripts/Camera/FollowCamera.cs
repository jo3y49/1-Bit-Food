using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector2 offset;
    [Range(1, 10)]
    public float smoothFactor;
    // [SerializeField] private Vector3 minValues, maxValues;

    // private void OnEnable() 
    // {
    //     SnapToPosition();
    // }

    // private void SnapToPosition()
    // {
    //     transform.position = target.position + offset;
    // }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(target);
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector2 targetPosition = (Vector2)target.position + offset;
        // Check if camera is out of bounds or not
        // Vector3 boundPosition = new Vector3(
        //     Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
        //     Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
        //     Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));
        Vector2 smoothPosition = Vector2.Lerp((Vector2)transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);

        Debug.Log(transform.position+ " "+targetPosition);
    }
}
