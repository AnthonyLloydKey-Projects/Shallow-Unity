using Mirror;
using Pc.Character.Scripts;
using UnityEngine;

public class CharacterController : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float walkSpeed = 0.04f;
    [SerializeField] private float sprintSpeed = 0.1f;
    [SerializeField] private float speed = 0.04f;
    [SerializeField] float speeds = 10f;
    [SerializeField] float sprint = 1f;
    
    private RaycastHit slopeRaycastHit;
    private Vector3 slopeMoveDirection;
    private float horizontalAxis;
    private float verticalAxis;
    private ITorch torch;
    private IAnimate animations;
    private IInteract collect;
    
    private void Start()
    {
        torch = GetComponent<ITorch>();
        animations = GetComponent<IAnimate>();
        animations.Initiate(anim);
        collect = GetComponent<IInteract>();
    }

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        
        StopMoving();
        Move();
        WalkingAnimation();
        Sprint();
        KeyboardInput();
    }

    public CharacterAnimations GetAnimations()
    {
        return (CharacterAnimations)animations;
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            torch.CmdToggle(this);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            collect.Pickup(this);
        }
    }

    public bool IsWalking()
    {
        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            return true;
        }
        
        return false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeRaycastHit, 4 / 2 + 0.5f))
        {
            if (slopeRaycastHit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    private void Move()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        var movement = transform.forward * verticalAxis + transform.right * horizontalAxis;
        movement.Normalize();
        
        slopeMoveDirection = Vector3.ProjectOnPlane(movement, slopeRaycastHit.normal);
        
        if (OnSlope())
        {
            rigidbody.AddForce(slopeMoveDirection * speeds * sprint, ForceMode.Acceleration);
        }
        else
        {
            rigidbody.AddForce(movement * speeds * sprint, ForceMode.Force);
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            anim.speed = 2f;
            sprint = 2;
        }
        else
        {
            speed = walkSpeed;
            anim.speed = 1f;
            sprint = 1;
        }
    }

    private void StopMoving()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            horizontalAxis = 0;
            verticalAxis = 0;
        }
    }

    private void WalkingAnimation()
    {
        anim.SetFloat("vertical", verticalAxis + speed);
        anim.SetFloat("horizontal", horizontalAxis + speed);
    }
}