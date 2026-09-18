using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class UR7eJointController : MonoBehaviour
{
    [Header("UR7e Joint Pivots")]
    public Transform joint1Pivot;
    public Transform joint2Pivot;
    public Transform joint3Pivot;
    public Transform joint4Pivot;
    public Transform joint5Pivot;
    public Transform joint6Pivot;

    private ROSConnection ros;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<JointStateMsg>(
            "/joint_states",
            JointStateCallback
        );

        Debug.Log("UR7e JointState subscriber started.");

        // Pivot 연결 상태 확인
        CheckPivotReferences();
    }

    void CheckPivotReferences()
    {
        Debug.Log("===== UR7e Pivot Check =====");

        Debug.Log(
            "Joint1Pivot: " +
            (joint1Pivot != null ? joint1Pivot.name : "NULL")
        );

        Debug.Log(
            "Joint2Pivot: " +
            (joint2Pivot != null ? joint2Pivot.name : "NULL")
        );

        Debug.Log(
            "Joint3Pivot: " +
            (joint3Pivot != null ? joint3Pivot.name : "NULL")
        );

        Debug.Log(
            "Joint4Pivot: " +
            (joint4Pivot != null ? joint4Pivot.name : "NULL")
        );

        Debug.Log(
            "Joint5Pivot: " +
            (joint5Pivot != null ? joint5Pivot.name : "NULL")
        );

        Debug.Log(
            "Joint6Pivot: " +
            (joint6Pivot != null ? joint6Pivot.name : "NULL")
        );

        Debug.Log("============================");
    }


    void JointStateCallback(JointStateMsg msg)
    {
        Debug.Log("JointState received");

        for (int i = 0; i < msg.name.Length; i++)
        {
            string jointName = msg.name[i];

            // ROS radian → Unity degree
            float angle =
                (float)msg.position[i] * Mathf.Rad2Deg;

            Debug.Log(
                $"ROS Joint: {jointName} = {angle:F2} deg"
            );

            switch (jointName)
            {
                case "shoulder_pan_joint":
                    SetJoint1(angle);
                    break;

                case "shoulder_lift_joint":
                    SetJoint2(angle);
                    break;

                case "elbow_joint":
                    SetJoint3(angle);
                    break;

                case "wrist_1_joint":
                    SetJoint4(angle);
                    break;

                case "wrist_2_joint":
                    SetJoint5(angle);
                    break;

                case "wrist_3_joint":
                    SetJoint6(angle);
                    break;
            }
        }
    }


    void SetJoint1(float angle)
    {
        if (joint1Pivot == null)
        {
            Debug.LogError("Joint1Pivot is NOT assigned!");
            return;
        }

        joint1Pivot.localRotation =
            Quaternion.Euler(0f, -angle, 0f);
    }


    void SetJoint2(float angle)
    {
        if (joint2Pivot == null)
        {
            Debug.LogError("Joint2Pivot is NOT assigned!");
            return;
        }

        joint2Pivot.localRotation =
            Quaternion.Euler(0f, 0f, -angle);
    }


    void SetJoint3(float angle)
    {
        if (joint3Pivot == null)
        {
            Debug.LogError("Joint3Pivot is NOT assigned!");
            return;
        }

        joint3Pivot.localRotation =
            Quaternion.Euler(0f, 0f, -angle);
    }


    void SetJoint4(float angle)
    {
        if (joint4Pivot == null)
        {
            Debug.LogError("Joint4Pivot is NOT assigned!");
            return;
        }

        joint4Pivot.localRotation =
            Quaternion.Euler(0f, 0f, -angle);
    }


    void SetJoint5(float angle)
    {
        if (joint5Pivot == null)
        {
            Debug.LogError("Joint5Pivot is NOT assigned!");
            return;
        }

        joint5Pivot.localRotation =
            Quaternion.Euler(0f, -angle, 0f);
    }


    void SetJoint6(float angle)
    {
        if (joint6Pivot == null)
        {
            Debug.LogError("Joint6Pivot is NOT assigned!");
            return;
        }

        joint6Pivot.localRotation =
            Quaternion.Euler(0f, 0f, -angle);
    }
}