using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Angle to view target at. 90 is directly above.
    [Range(0f, 90f)] public float Angle = 75f;
    // Distance to keep from the target.
    [Range(1f, 30f)] public float Distance = 10f;

    private Vector3 m_offset;
    private float m_lastAngle;
    private float m_lastDistance;

    // Target to follow.
    public static GameObject Target;

    private void LateUpdate()
    {
        if (m_lastAngle != Angle || m_lastDistance != Distance)
        {
            // Compute an offset from the Angle and Distance.
            float radians = Angle * Mathf.Deg2Rad;
            m_offset = new Vector3(0f, Mathf.Sin(radians) * Distance, -Mathf.Cos(radians) * Distance);
            m_lastAngle = Angle;
            m_lastDistance = Distance;
        }
        if (Target != null)
        {
            // Add the offset to the target position to get the camera position, and look at the target.
            transform.position = Target.transform.position + m_offset;
            transform.LookAt(Target.transform);
        }
    }
}