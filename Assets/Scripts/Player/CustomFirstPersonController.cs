using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class CustomFirstPersonController : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] private float m_StepInterval;

    [SerializeField] private float _wallRunMaxDistance;
    [SerializeField] private float _wallRunSpeed;
    [SerializeField] private float _wallRunClimbFactor;
    [SerializeField] private float _wallRunChargeDepletionRate;
    [SerializeField] private float _wallRunRechargeRate;
    [SerializeField] private float _wallRunTilt;
    [SerializeField] private float _wallRunTiltSpeed;

    [SerializeField] private float _thrusterForce;
    [SerializeField] private float _thrusterWallImpluse;
    [SerializeField] private float _thrusterChargeDepletionRate;
    [SerializeField] private float _thrusterRechargeRate;
    [SerializeField] private bool _rechargeThrusterOnWallRun;

    [SerializeField] private float _slidingSpeed;
    [SerializeField] private float _slidingChargeDepletionRate;
    [SerializeField] private float _slidingRechargeRate;
    [SerializeField] private float _slidingCameraDrop;
    [SerializeField] private float _slidingCameraAngle;
    [SerializeField] private float _slidingRotationSpeed;


    [SerializeField] private Transform _cameraPivot;

    [SerializeField]
    private AudioClip[] m_FootstepSounds; // an array of footstep sounds that will be randomly selected from.

    [SerializeField] private AudioClip m_JumpSound; // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound; // the sound played when character touches back on ground.

    private PlayerInputController _playerInputController;
    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    [SerializeField] [ReadOnly] private bool _isWallRunning = false;
    private Vector3 _wallNormal = Vector3.zero;
    private Vector3 _wallRunDir = Vector3.zero;
    private float _wallRunChargeLeft = 100f;

    [SerializeField] [ReadOnly] private bool _isUsingThrusters = false;
    private float _thrusterChargeLeft = 100f;
    private bool _requiresThrusterJumpStart = false;
    private Vector3 _extraThrusterForce = Vector3.zero;

    [SerializeField] [ReadOnly] private bool _isSliding = false;
    private float _slidingChargeLeft = 100f;


    private class GizmosData
    {
        public Vector3 raycastDir;
    }

    private GizmosData _gizmosData = new GizmosData();

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        _playerInputController = GetComponent<PlayerInputController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //        Debug.Log("WallRunDir: " + _wallRunDir.normalized);
        Gizmos.DrawRay(transform.position, _wallNormal.normalized * 10);

//        Vector3 wallParallel = Vector3.Cross(_wallNormal.normalized, Vector3.up);


        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _wallRunDir.normalized * 10);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _gizmosData.raycastDir * 10);

//        Gizmos.DrawWireSphere(transform.position, 10f);
    }


    // Update is called once per frame
    private void Update()
    {
        PlayerInputController.PlayerInput playerInput = _playerInputController.GetPlayerInput();

        RotateView();
        /*// the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButton("Jump");
        }*/

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }

        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;

        /*if (!_isWallRunning)
        {
            _wallRunChargeLeft += _wallRunRechargeRate * Time.deltaTime;
            if (_wallRunChargeLeft > 100)
            {
                _wallRunChargeLeft = 100;
            }
        }*/

        /*if (!_isUsingThrusters)
        {
            _thrusterChargeLeft += _thrusterRechargeRate * Time.deltaTime;
            if (_thrusterChargeLeft > 100)
            {
                _thrusterChargeLeft = 100;
            }
        }*/

        if (!_isSliding)
        {
            _slidingChargeLeft += _slidingRechargeRate * Time.deltaTime;
            if (_slidingChargeLeft > 100)
            {
                _slidingChargeLeft = 100;
            }
        }
    }

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        Move(speed);

        if (m_CharacterController.isGrounded)
        {
            _wallRunChargeLeft = 100f;
            _thrusterChargeLeft = 100f;

            _extraThrusterForce = Vector3.zero;

//            _requiresThrusterJumpStart = false;
        }

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }

    private void Move(float speed)
    {
        float targetZRotation = 0;
        bool wasWallRunning = _isWallRunning;

        if (Slide())
        {
        }
        else if (WallRun(speed))
        {
//            Debug.Log("Wall Run: " + m_MoveDir);
            targetZRotation = _wallRunTilt;
            if (Vector3.Angle(transform.right, _wallNormal) <= 90)
            {
                targetZRotation *= -1;
            }

            _requiresThrusterJumpStart = true;

            if (_rechargeThrusterOnWallRun)
            {
                _thrusterChargeLeft = 100;
            }
        }
        else
        {
            GroundMove(speed);
        }

        if (_playerInputController.GetPlayerInput().JumpStart)
        {
            Debug.Log("Jumping");
        }

        if (wasWallRunning || !_isWallRunning)
        {
            UseThrusters();
        }

        float rotationLerpSpeed = _wallRunTiltSpeed;

        float xRot = 0;
        if (_isSliding)
        {
            xRot = _slidingCameraAngle;
            rotationLerpSpeed = _slidingRotationSpeed;
        }

        Quaternion targetRotation = Quaternion.Euler(xRot,
            _cameraPivot.rotation.eulerAngles.y, targetZRotation);

        _cameraPivot.rotation =
            Quaternion.Lerp(_cameraPivot.rotation, targetRotation, Time.fixedDeltaTime * rotationLerpSpeed);

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
    }

    private void GroundMove(float speed)
    {
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
            m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;
        }
        else
        {
//            Debug.Log("Applying Gravity");
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
    }

    private bool WallRun(float speed)
    {
        PlayerInputController.PlayerInput playerInput = _playerInputController.GetPlayerInput();

        Vector3 move = new Vector3(m_Input.x, 0, m_Input.y);

        Vector3 moveWorldDir = transform.TransformDirection(move.normalized);
        _gizmosData.raycastDir = moveWorldDir;

        if (move.z > 0 && _wallRunChargeLeft > 0)
        {
            int playerLayerMask = LayerMask.NameToLayer("Player");
            int wallRunnableLayerMask = LayerMask.GetMask("WallRunnable");

            if (!_isWallRunning && (playerInput.JumpStart || (!m_CharacterController.isGrounded && playerInput.Jump)))
            {
                Vector3 raycastDir1 = moveWorldDir;
                Vector3 raycastDir2 = Vector3.zero;

                if (!m_CharacterController.isGrounded)
                {
                    if (_extraThrusterForce.magnitude > 0)
                    {
                        Vector3 localDir = transform.InverseTransformDirection(_extraThrusterForce.normalized);
                        localDir.y = 0;
                        localDir.z = move.z;
                        raycastDir1 = transform.TransformDirection(localDir);
                    }
                    else
                    {
                        Vector3 localDir = new Vector3(-1, 0, move.z);
                        raycastDir1 = transform.TransformDirection(localDir);
                        localDir = new Vector3(1, 0, move.z);
                        raycastDir2 = transform.TransformDirection(localDir);
                    }
                }

                RaycastHit hit;
                if ((Physics.Raycast(transform.position, raycastDir1, out hit,
                         _wallRunMaxDistance, wallRunnableLayerMask) ||
                     Physics.Raycast(transform.position, raycastDir2, out hit,
                         _wallRunMaxDistance, wallRunnableLayerMask)))
                {
                    _isWallRunning = true;

                    _wallNormal = hit.normal;

                    Vector3 wallNormalYLess = _wallNormal;
                    wallNormalYLess.y = 0;

                    Vector3 moveYLess = moveWorldDir;
                    moveYLess.y = 0;

                    Vector3 forwardYLess = transform.forward;
                    forwardYLess.y = 0;

                    float angle = Vector3.Angle(wallNormalYLess, moveYLess);

                    float compAngle = 90 - angle;

                    float diff = moveYLess.magnitude * Mathf.Sin(compAngle * Mathf.Deg2Rad);

                    _wallRunDir = moveYLess - (wallNormalYLess.normalized * diff);
                    if (Vector3.Angle(_wallRunDir, moveYLess) > 90)
                    {
                        _wallRunDir *= -1;
                    }

                    _extraThrusterForce = Vector3.zero;
                }
            }

            if (_isWallRunning)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -_wallNormal, out hit, _wallRunMaxDistance,
                    wallRunnableLayerMask))
                {
                    m_MoveDir = _wallRunDir * _wallRunSpeed;
                    m_MoveDir.y = _wallRunChargeLeft * _wallRunClimbFactor * Time.fixedDeltaTime;

                    _wallRunChargeLeft -= _wallRunChargeDepletionRate * Time.fixedDeltaTime;
                    if (_wallRunChargeLeft < 0)
                    {
                        _wallRunChargeLeft = 0;
                    }

                    _isWallRunning = true;
                }
                else
                {
                    _isWallRunning = false;
                }

                return true;
            }
        }

        if (_isWallRunning)
        {
            _wallRunChargeLeft = 0;
        }

        _isWallRunning = false;
        return false;
    }


    private void UseThrusters()
    {
        PlayerInputController.PlayerInput playerInput = _playerInputController.GetPlayerInput();
        _isUsingThrusters = false;

        if (_thrusterChargeLeft > 0)
        {
            Debug.Log("Using Thruster...");
            if (_requiresThrusterJumpStart && playerInput.JumpStart)
            {
                _requiresThrusterJumpStart = false;

                Debug.Log("Removes Jump Start requirement");
            }

            if (_isWallRunning && playerInput.JumpStart)
            {
                _extraThrusterForce = _wallNormal.normalized * _thrusterWallImpluse;
                _extraThrusterForce += _wallRunDir.normalized * 100;
                _extraThrusterForce.y = 0;

                _extraThrusterForce = _extraThrusterForce.normalized *
                                      (Mathf.Clamp(_extraThrusterForce.magnitude, 0, _thrusterWallImpluse));

                _isWallRunning = false;
                _wallRunChargeLeft = 100;
            }

            if (playerInput.Jump && !_requiresThrusterJumpStart)
            {
                m_MoveDir.y = _thrusterForce * Time.fixedDeltaTime;
                _thrusterChargeLeft -= _thrusterChargeDepletionRate * Time.fixedDeltaTime;
                if (_thrusterChargeLeft < 0)
                {
                    _thrusterChargeLeft = 0;
                }

                _isUsingThrusters = true;
            }
        }

        m_MoveDir += _extraThrusterForce * Time.fixedDeltaTime;
        _extraThrusterForce = Vector3.Lerp(_extraThrusterForce, Vector3.zero, Time.fixedDeltaTime);
    }

    private bool Slide()
    {
        PlayerInputController.PlayerInput playerInput = _playerInputController.GetPlayerInput();

        if (!_isWallRunning && !_isSliding && playerInput.SlideStart && _slidingChargeLeft >= 100)
        {
            _isSliding = true;
        }

        if (_isSliding && (playerInput.Slide || playerInput.SlideStart) && _slidingChargeLeft > 0)
        {
            Debug.Log("Sliding..");

            m_MoveDir = transform.forward * _slidingSpeed * Time.fixedDeltaTime;
            m_MoveDir.y = 0;

            _slidingChargeLeft -= _slidingChargeDepletionRate * Time.fixedDeltaTime;
            if (_slidingChargeLeft < 0)
            {
                _slidingChargeLeft = 0;
            }

            _isSliding = true;
            return true;
        }

        _isSliding = false;
        return false;
    }

    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle +=
                (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;

        if (_isSliding)
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y -= _slidingCameraDrop;

            m_Camera.transform.localPosition =
                Vector3.Lerp(m_Camera.transform.localPosition, newCameraPosition, Time.fixedDeltaTime);
            return;
        }

        if (!m_UseHeadBob)
        {
            return;
        }

        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                    (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }

        m_Camera.transform.localPosition = newCameraPosition;
    }


    private void GetInput(out float speed)
    {
        PlayerInputController.PlayerInput playerInput = _playerInputController.GetPlayerInput();

        // Read input

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !playerInput.Sprint;
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(playerInput.Move.x, playerInput.Move.z);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }

        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}