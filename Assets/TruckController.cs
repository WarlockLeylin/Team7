using UnityEngine;

public class TruckController : MonoBehaviour
{
    [Header("Truck settings")]
    public float motorForce = 1500f;   // мощность двигателя
    public float brakeForce = 3000f;   // тормоза
    public float maxSteerAngle = 30f;  // угол поворота колес

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public WheelCollider rearLeftWheel2;
    public WheelCollider rearRightWheel2;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;
    public Transform rearLeftMesh2;
    public Transform rearRightMesh2;

    private float steerInput;
    private float throttleInput;
    private float brakeInput;

    void Update()
    {
        // Считываем ввод
        steerInput = Input.GetAxis("Horizontal");
        throttleInput = Input.GetAxis("Vertical");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }

    void FixedUpdate()
    {
        HandleSteering();
        HandleMotor();
    }

    void LateUpdate()
    {
        UpdateWheels();
    }

    void HandleMotor()
    {
        // Газ / тормоз
        frontLeftWheel.motorTorque = throttleInput * motorForce;
        frontRightWheel.motorTorque = throttleInput * motorForce;

        // Ручной тормоз
        if (brakeInput > 0)
        {
            frontLeftWheel.brakeTorque = brakeForce;
            frontRightWheel.brakeTorque = brakeForce;
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;
            rearLeftWheel2.brakeTorque = brakeForce;
            rearRightWheel2.brakeTorque = brakeForce;
        }
        else
        {
            frontLeftWheel.brakeTorque = 0f;
            frontRightWheel.brakeTorque = 0f;
            rearLeftWheel.brakeTorque = 0f;
            rearRightWheel.brakeTorque = 0f;
            rearLeftWheel2.brakeTorque = 0f;
            rearRightWheel2.brakeTorque = 0f;
        }
    }

    void HandleSteering()
    {
        float steerAngle = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheel, frontLeftMesh);
        UpdateSingleWheel(frontRightWheel, frontRightMesh);
        UpdateSingleWheel(rearLeftWheel, rearLeftMesh);
        UpdateSingleWheel(rearRightWheel, rearRightMesh);
        UpdateSingleWheel(rearLeftWheel2, rearLeftMesh2);
        UpdateSingleWheel(rearRightWheel2, rearRightMesh2);
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
    }
}
