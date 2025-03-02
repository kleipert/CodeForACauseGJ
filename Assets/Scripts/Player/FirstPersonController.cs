using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 20.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 30.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 0.7f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("Boost Strength")]
		public float BoostStrength = 100f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float GravityBase = -15.0f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;
		[Tooltip("Which movement ability has the player at the moment. 1 = Sprint, 2 = Jetpack, 3 = ")]
		public int MovementAbility = 1;
		[Tooltip("How much energy the jetpack possesses. The engine default is 100f")]
		public float JetpackStorage = 100.0f;
		[Tooltip("How much energy the jetpack losses. The engine default is 100f")]
		public float JetpackLoss = 75.0f;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public float DashSpeed = 5.0f;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public float DashDuration = 1.0f;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public float BaseDuration = 1.0f;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public bool DashActive = false;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public float DashCooldown = 0.0f;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public Vector3 SaveVector = Vector3.zero;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public Vector2 SaveInput = Vector2.zero;
		[Tooltip("How fast the player dash. The engine default is 100f")]
		public Vector2 SaveVelocity = Vector3.zero;
		

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = 0.6f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		[Tooltip("What layers the character uses to grapple")]
		public LayerMask GrappleLayers;
		[Tooltip("Is the player currently grappling")]
		public bool IsGrappling;
		[FormerlySerializedAs("GrappleTarget")] [Tooltip("GrappleTarget")]
		public Vector3 GrappleDirection;
		[Tooltip("GrapplePosition")]
		public Vector3 GrapplePosition;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;
		private LineRenderer _lr;

		// player
		private float _speed;
		private float _rotationVelocity;
		[SerializeField]private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private InputSettingsInput _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
				_lr = GetComponent<LineRenderer>();
				_lr.material = new Material(Shader.Find("Sprites/Default"));
				_lr.widthMultiplier = 0.1f;
				_lr.enabled = false;
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<InputSettingsInput>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = MoveSpeed;
			/*if (_input.jump && MovementAbility == 1)
				targetSpeed = SprintSpeed;
			else
				targetSpeed = MoveSpeed;*/
			

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{

			if (IsGrappling)
			{
				_controller.Move(GrappleDirection.normalized * (Time.deltaTime * SprintSpeed));
				_input.move = Vector2.zero;
				if ((GrapplePosition - transform.position).magnitude <= 2)
				{
					IsGrappling = false;
					_lr.enabled = false;
					GrappleDirection = Vector3.zero;
				}
				return;
			}
			
			if (_input.jump && MovementAbility == 3)
			{
				Gravity = 0;
				RaycastHit rc_hit;
				if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out rc_hit, 50,
					    GrappleLayers))
				{
					IsGrappling = true;
					_lr.positionCount = 2;
					
					GrapplePosition = rc_hit.point;
					_lr.SetPosition(0, transform.position);
					_lr.SetPosition(1, GrapplePosition);
					_lr.enabled = true;
					GrappleDirection = rc_hit.point - transform.position;
				}
			}
			else
				Gravity = GravityBase;
			
			if (_verticalVelocity < Gravity / 2 && Grounded)
			{
				_verticalVelocity = Gravity / 2;
				if (JetpackStorage < 100)
				{
					JetpackStorage += (JetpackLoss * Time.deltaTime);
				}
			}
			
			if (_input.jump)
			{
				if (MovementAbility == 2 && JetpackStorage >= 0.0f)
				{
					_verticalVelocity = Mathf.Sqrt(BoostStrength * -2f * Gravity * Time.deltaTime);
					JetpackStorage -= (JetpackLoss * Time.deltaTime);
				}
					
			}
			
			if (MovementAbility == 1 && _input.jump && !DashActive && DashCooldown <= 0.0f)
			{
				DashActive = true;
				if (_input.move != Vector2.zero)
				{
					SaveVector = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
					SaveInput = _input.move;
					SaveVelocity = _controller.velocity;
				}
			}
			
			if (DashDuration > 0.0f && DashActive)
			{
				Vector3 globalPosition = transform.TransformDirection(SaveVector).normalized;
				_controller.Move(globalPosition * (Time.deltaTime * DashSpeed));
				DashDuration -= Time.deltaTime;
				_input.move = Vector2.zero;
			}

			if (DashDuration <= 0.0f)
			{
				SaveVector = Vector3.zero;
				DashDuration = BaseDuration;
				DashActive = false;
				DashCooldown = 3.0f;
				if (Input.anyKey)
				{
					_input.move = SaveInput;
					_controller.Move(SaveVelocity * Time.deltaTime);
				}
			}
			
			if (DashCooldown >= 0.0f && !DashActive)
			{
				DashCooldown -= Time.deltaTime;
			}
			
			_verticalVelocity += Gravity * Time.deltaTime;
			
			
			
			/*
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
			*/
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}
