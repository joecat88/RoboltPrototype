using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class RoboltBase : MonoBehaviour
{

    // costanti
    protected const float ROTATION_SPEED = 10f;
    protected const float MOVE_SPEED_DEFAULT = 5.0f;
    protected const float MOVE_SPEED_JUMP = 4.0f;
    protected const float MOVE_SPEED_HITTED = 2.0f;
    protected const float PROJECTOR_Y_OFFSET = 0.2f;

    protected const float SHADOW_SPEED = 15.0f;
    protected const float GRAVITY_JETPACK= -9.8f;
    public const float MAX_JUMP_HEIGHT = 4f;
    //public const float MAX_JUMP_HEIGHT_GEAR_S = 6f;
    public const float MAX_JUMP_TIME = 1f;
    protected const float FALL_MULTIPLIER = 2.5f;

    protected float gravityGround;
    protected float initialJumpVelocity;

    // reference
    public CinemachineFreeLook cinemachine;
    public CinemachineInputProvider cinemachineScript;
    public Transform followCamera;
    public Transform followCameraJetPack;
    public Transform projectorT;
    public LayerMask groundLayers;
    public Animator blackScreenAnimator;
    protected Animator animator;
    protected Transform cameraMain;
    protected DecalProjector projector;
    protected CharacterController charController;

    // variabli
    [SerializeField] protected bool isGroundedPlayer;
    [SerializeField] protected bool isHandleAnimationBlocked;
    [SerializeField] protected bool isStoppedPlayerSessions;
    [SerializeField] protected Vector3 currentVelocity;
    [SerializeField] protected Vector3 jetPackMove;
    //[SerializeField] protected Vector3 lastCheckPoint;


    // Input comandi
    [SerializeField] protected InputActionReference movementControl;
    [SerializeField] protected InputActionReference jumpControl;
    [SerializeField] protected InputActionReference leftTrigger;
    [SerializeField] protected InputActionReference rightTrigger;
    //[SerializeField] protected InputActionReference triangle;

    protected virtual void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        cameraMain = GameObject.FindWithTag("MainCamera").transform;
        projector = projectorT.GetComponent<DecalProjector>();

    }
}
