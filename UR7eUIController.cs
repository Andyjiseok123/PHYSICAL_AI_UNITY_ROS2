using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Trajectory;

public class UR7eUIController : MonoBehaviour
{
    [Header("UR7e Joint Pivots")]
    public Transform joint1Pivot;
    public Transform joint2Pivot;
    public Transform joint3Pivot;
    public Transform joint4Pivot;
    public Transform joint5Pivot;
    public Transform joint6Pivot;

    [Header("Joint Sliders")]
    public Slider joint1Slider;
    public Slider joint2Slider;
    public Slider joint3Slider;
    public Slider joint4Slider;
    public Slider joint5Slider;
    public Slider joint6Slider;

    [Header("ROS 2")]
    public string commandTopic = "/ur7e_controller/joint_trajectory";

    private ROSConnection ros;

    private readonly string[] jointNames =
    {
        "shoulder_pan_joint",
        "shoulder_lift_joint",
        "elbow_joint",
        "wrist_1_joint",
        "wrist_2_joint",
        "wrist_3_joint"
    };

    // 현재 관절 각도 [deg]
    private float[] jointAngles = new float[6];

    // URDF 기준 위치 제한 [deg]
    private readonly float[] jointMinLimits =
    {
        -360f,   // Joint 1
        -360f,   // Joint 2
        -180f,   // Joint 3
        -360f,   // Joint 4
        -360f,   // Joint 5
        -360f    // Joint 6
    };

    private readonly float[] jointMaxLimits =
    {
         360f,   // Joint 1
         360f,   // Joint 2
         180f,   // Joint 3
         360f,   // Joint 4
         360f,   // Joint 5
         360f    // Joint 6
    };


    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.RegisterPublisher<JointTrajectoryMsg>(commandTopic);

        // Slider 이벤트 연결
        joint1Slider.onValueChanged.AddListener(SetJoint1);
        joint2Slider.onValueChanged.AddListener(SetJoint2);
        joint3Slider.onValueChanged.AddListener(SetJoint3);
        joint4Slider.onValueChanged.AddListener(SetJoint4);
        joint5Slider.onValueChanged.AddListener(SetJoint5);
        joint6Slider.onValueChanged.AddListener(SetJoint6);

        // 시작값 저장
        jointAngles[0] = Mathf.Clamp(
            joint1Slider.value,
            jointMinLimits[0],
            jointMaxLimits[0]
        );

        jointAngles[1] = Mathf.Clamp(
            joint2Slider.value,
            jointMinLimits[1],
            jointMaxLimits[1]
        );

        jointAngles[2] = Mathf.Clamp(
            joint3Slider.value,
            jointMinLimits[2],
            jointMaxLimits[2]
        );

        jointAngles[3] = Mathf.Clamp(
            joint4Slider.value,
            jointMinLimits[3],
            jointMaxLimits[3]
        );

        jointAngles[4] = Mathf.Clamp(
            joint5Slider.value,
            jointMinLimits[4],
            jointMaxLimits[4]
        );

        jointAngles[5] = Mathf.Clamp(
            joint6Slider.value,
            jointMinLimits[5],
            jointMaxLimits[5]
        );

        Debug.Log("UR7e UI Controller Started");
        Debug.Log("ROS 2 Publisher: " + commandTopic);
    }


    // =========================
    // Joint 1
    // =========================

    void SetJoint1(float angle)
    {
        jointAngles[0] = Mathf.Clamp(
            angle,
            jointMinLimits[0],
            jointMaxLimits[0]
        );

        if (joint1Pivot != null)
        {
            joint1Pivot.localRotation =
                Quaternion.Euler(0f, -jointAngles[0], 0f);
        }

        PublishJointTrajectory();
    }


    // =========================
    // Joint 2
    // =========================

    void SetJoint2(float angle)
    {
        jointAngles[1] = Mathf.Clamp(
            angle,
            jointMinLimits[1],
            jointMaxLimits[1]
        );

        if (joint2Pivot != null)
        {
            joint2Pivot.localRotation =
                Quaternion.Euler(0f, 0f, -jointAngles[1]);
        }

        PublishJointTrajectory();
    }


    // =========================
    // Joint 3
    // =========================

    void SetJoint3(float angle)
    {
        jointAngles[2] = Mathf.Clamp(
            angle,
            jointMinLimits[2],
            jointMaxLimits[2]
        );

        if (joint3Pivot != null)
        {
            joint3Pivot.localRotation =
                Quaternion.Euler(0f, 0f, -jointAngles[2]);
        }

        PublishJointTrajectory();
    }


    // =========================
    // Joint 4
    // =========================

    void SetJoint4(float angle)
    {
        jointAngles[3] = Mathf.Clamp(
            angle,
            jointMinLimits[3],
            jointMaxLimits[3]
        );

        if (joint4Pivot != null)
        {
            joint4Pivot.localRotation =
                Quaternion.Euler(0f, 0f, -jointAngles[3]);
        }

        PublishJointTrajectory();
    }


    // =========================
    // Joint 5
    // =========================

    void SetJoint5(float angle)
    {
        jointAngles[4] = Mathf.Clamp(
            angle,
            jointMinLimits[4],
            jointMaxLimits[4]
        );

        if (joint5Pivot != null)
        {
            joint5Pivot.localRotation =
                Quaternion.Euler(0f, -jointAngles[4], 0f);
        }

        PublishJointTrajectory();
    }


    // =========================
    // Joint 6
    // =========================

    void SetJoint6(float angle)
    {
        jointAngles[5] = Mathf.Clamp(
            angle,
            jointMinLimits[5],
            jointMaxLimits[5]
        );

        if (joint6Pivot != null)
        {
            joint6Pivot.localRotation =
                Quaternion.Euler(0f, 0f, -jointAngles[5]);
        }

        PublishJointTrajectory();
    }


    // =========================
    // ROS 2 JointTrajectory
    // =========================

    void PublishJointTrajectory()
    {
        if (ros == null)
            return;

        JointTrajectoryMsg trajectory =
            new JointTrajectoryMsg();

        trajectory.joint_names = jointNames;


        JointTrajectoryPointMsg point =
            new JointTrajectoryPointMsg();

        point.positions = new double[6];


        // Degree → Radian
        for (int i = 0; i < 6; i++)
        {
            float limitedAngle = Mathf.Clamp(
                jointAngles[i],
                jointMinLimits[i],
                jointMaxLimits[i]
            );

            point.positions[i] =
                limitedAngle * Mathf.Deg2Rad;
        }


        // 1초 후 목표 위치
        point.time_from_start =
            new RosMessageTypes.BuiltinInterfaces.DurationMsg
            {
                sec = 1,
                nanosec = 0
            };


        trajectory.points =
            new JointTrajectoryPointMsg[]
            {
                point
            };


        ros.Publish(
            commandTopic,
            trajectory
        );


        Debug.Log(
            $"UR7e Command Published: " +
            $"J1={jointAngles[0]:F1}, " +
            $"J2={jointAngles[1]:F1}, " +
            $"J3={jointAngles[2]:F1}, " +
            $"J4={jointAngles[3]:F1}, " +
            $"J5={jointAngles[4]:F1}, " +
            $"J6={jointAngles[5]:F1}"
        );
    }
}