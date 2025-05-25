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

    // Audio clips
    public AudioClip engineClip;
    public AudioClip brakeClip;
    public AudioClip driftClip;

    // Volumen para cada audio, controlable desde inspector
    [Range(0f, 1f)]
    public float engineVolume = 0.1f;
    [Range(0f, 1f)]
    public float brakeVolume = 1f;
    [Range(0f, 1f)]
    public float driftVolume = 1f;

    private AudioSource engineAudioSource;
    private AudioSource brakeAudioSource;
    private AudioSource driftAudioSource;

    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        InstantiateSmoke();

        // Setup audio sources
        engineAudioSource = gameObject.AddComponent<AudioSource>();
        engineAudioSource.clip = engineClip;
        engineAudioSource.loop = true;
        engineAudioSource.playOnAwake = true;
        engineAudioSource.volume = engineVolume;
        engineAudioSource.Play();

        brakeAudioSource = gameObject.AddComponent<AudioSource>();
        brakeAudioSource.clip = brakeClip;
        brakeAudioSource.loop = true;
        brakeAudioSource.playOnAwake = false;
        brakeAudioSource.volume = brakeVolume;

        driftAudioSource = gameObject.AddComponent<AudioSource>();
        driftAudioSource.clip = driftClip;
        driftAudioSource.loop = true;
        driftAudioSource.playOnAwake = false;
        driftAudioSource.volume = driftVolume;
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

        UpdateEngineSound();
        UpdateBrakeSound();
        UpdateDriftSound();
    }

    void CheckInput()
    {
        steeringInput = Input.GetAxis("Horizontal");

        gasInput = 0;
        brakeInput = 0;

        bool acelerar = Input.GetButton("Acelerar"); // R2
        bool frenar = Input.GetButton("Frenar");     // L2

        if (acelerar && !frenar)
        {
            gasInput = 1f; // Avanza
        }
        else if (frenar && !acelerar)
        {
            gasInput = -1f; // Marcha atrás
        }
        else if (frenar && acelerar)
        {
            brakeInput = 1f; // Ambos pulsados = freno
        }

        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);
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

    void UpdateEngineSound()
    {
        if (engineAudioSource == null) return;

        float minPitch = 0.5f;
        float maxPitch = 2f;

        float gasAbs = Mathf.Abs(gasInput);

        float targetPitch = Mathf.Lerp(minPitch, maxPitch, gasAbs);
        targetPitch *= Mathf.Lerp(0.5f, 1.5f, speed / 30f);
        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        float minVolume = 0.1f;
        float volume = Mathf.Clamp(gasAbs, minVolume, 1f);

        engineAudioSource.pitch = targetPitch;
        engineAudioSource.volume = volume * engineVolume;
    }

    void UpdateBrakeSound()
    {
        if (brakeAudioSource == null) return;

        float targetVolume = (brakeInput > 0.3f && speed > 2f) ? brakeVolume : 0f;

        if (targetVolume > 0f)
        {
            if (!brakeAudioSource.isPlaying)
                brakeAudioSource.Play();
            brakeAudioSource.volume = targetVolume;
        }
        else
        {
            if (brakeAudioSource.isPlaying)
                brakeAudioSource.Stop();
        }
    }

    void UpdateDriftSound()
    {
        if (driftAudioSource == null) return;

        float driftThreshold = 0.5f;
        bool isDrifting = false;

        WheelHit hit;

        if (colliders.FRWheel.GetGroundHit(out hit))
            if (Mathf.Abs(hit.sidewaysSlip) > driftThreshold) isDrifting = true;

        if (!isDrifting && colliders.FLWheel.GetGroundHit(out hit))
            if (Mathf.Abs(hit.sidewaysSlip) > driftThreshold) isDrifting = true;

        if (!isDrifting && colliders.RRWheel.GetGroundHit(out hit))
            if (Mathf.Abs(hit.sidewaysSlip) > driftThreshold) isDrifting = true;

        if (!isDrifting && colliders.RLWheel.GetGroundHit(out hit))
            if (Mathf.Abs(hit.sidewaysSlip) > driftThreshold) isDrifting = true;

        if (isDrifting && speed > 2f)
        {
            if (!driftAudioSource.isPlaying)
                driftAudioSource.Play();

            driftAudioSource.volume = Mathf.Clamp01(Mathf.Abs(hit.sidewaysSlip) * 2f) * driftVolume;
            driftAudioSource.pitch = Mathf.Lerp(0.8f, 1.2f, Mathf.Abs(hit.sidewaysSlip) * 2f);
        }
        else
        {
            if (driftAudioSource.isPlaying)
                driftAudioSource.Stop();
        }
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