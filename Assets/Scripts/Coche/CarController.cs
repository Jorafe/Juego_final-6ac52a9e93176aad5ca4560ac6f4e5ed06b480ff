using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody playerRB;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public float gasInput;
    public float brakeInput;
    public float steeringInput;
    public GameObject smokePrefab;
    public float motorPower;
    public float brakePower;
    public float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;

    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        InstantiateSmoke();
    }

    void InstantiateSmoke()
    {
        wheelParticles.FRWheel = Instantiate(smokePrefab, colliders.FRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FRWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.FLWheel = Instantiate(smokePrefab, colliders.FLWheel.transform.position - Vector3.up * colliders.FLWheel.radius, Quaternion.identity, colliders.FLWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(smokePrefab, colliders.RRWheel.transform.position - Vector3.up * colliders.RRWheel.radius, Quaternion.identity, colliders.RRWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(smokePrefab, colliders.RLWheel.transform.position - Vector3.up * colliders.RLWheel.radius, Quaternion.identity, colliders.RLWheel.transform).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        speed = playerRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        CheckParticles();
        ApplyWheelPositions();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");

        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);
        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    void ApplyMotor()
    {
        colliders.RRWheel.motorTorque = motorPower * gasInput;
        colliders.RLWheel.motorTorque = motorPower * gasInput;
    }

    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        if (slipAngle < 120f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, playerRB.velocity + transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
    }

    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        colliders.FRWheel.GetGroundHit(out wheelHits[0]);
        colliders.FLWheel.GetGroundHit(out wheelHits[1]);
        colliders.RRWheel.GetGroundHit(out wheelHits[2]);
        colliders.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.5f;
        float brakeThreshold = 0.3f;
        float steeringThreshold = 0.5f;
        float minSpeed = 2f;

        CheckAndPlaySmoke(wheelParticles.FRWheel, wheelHits[0], brakeInput, steeringInput, speed, slipAllowance, brakeThreshold, steeringThreshold, minSpeed);
        CheckAndPlaySmoke(wheelParticles.FLWheel, wheelHits[1], brakeInput, steeringInput, speed, slipAllowance, brakeThreshold, steeringThreshold, minSpeed);
        CheckAndPlaySmoke(wheelParticles.RRWheel, wheelHits[2], brakeInput, steeringInput, speed, slipAllowance, brakeThreshold, steeringThreshold, minSpeed);
        CheckAndPlaySmoke(wheelParticles.RLWheel, wheelHits[3], brakeInput, steeringInput, speed, slipAllowance, brakeThreshold, steeringThreshold, minSpeed);
    }

    void CheckAndPlaySmoke(ParticleSystem ps, WheelHit hit, float brakeInput, float steeringInput, float speed, float slipAllowance, float brakeThreshold, float steeringThreshold, float minSpeed)
    {
        bool isSlipping = Mathf.Abs(hit.sidewaysSlip) + Mathf.Abs(hit.forwardSlip) > slipAllowance;
        bool isBrakingHard = brakeInput > brakeThreshold && speed > minSpeed;
        bool isSteeringHard = Mathf.Abs(steeringInput) > steeringThreshold && speed > minSpeed;
        bool isAccelerating = Mathf.Abs(gasInput) > 0.4f && speed > minSpeed;

        if (isSlipping || isBrakingHard || isSteeringHard || isAccelerating)
        {
            var emission = ps.emission;
            emission.rateOverTime = Mathf.Clamp(speed * 10f, 20f, 100f);
            if (!ps.isPlaying)
                ps.Play();
        }
        else
        {
            if (ps.isPlaying)
                ps.Stop();
        }
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;
}