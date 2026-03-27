using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum MoveState
{
    Fall,
    Walk,
    Jump,
    Dash,
    Idle
}

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb2D;
    BoxCollider2D boxCollider;

    //STATES
    Dictionary<MoveState, BaseStateAbility> states = new Dictionary<MoveState, BaseStateAbility>();
    Dictionary<MoveState, List<Transition>> transitions = new Dictionary<MoveState, List<Transition>>();

    [Header("State")]
    [SerializeField]
    MoveState currentStateType;
    MoveState tempStateType;

    //float currentAngle;
    float moveDirection;

    [Header("Ground Info")]
    [SerializeField]
    LayerMask mask;
    bool isGrounded;
    Vector2 groundNormal;
    RaycastHit2D hit2D;

    [Header("Jump")]
    bool jumpQueued;
    float jumpTimer = 0;
    bool jumpHeldDown;
    bool jumpPerformed;

    [Header("Edge Detection")]
    [SerializeField]
    float skinDepth = 0.15f;

    float coyoteTimer = 0f;
    [Header("Stats")]
    [SerializeField]
    float coyoteTime;
    [SerializeField]
    float jumpQueueTime;
    Vector2 movement;

    bool debug;
    public bool Debug {get {return debug;}  
        set
        { 
            if (value == false)
            {
                text.gameObject.SetActive(false);
                if (TryGetComponent<TrailRenderer>(out TrailRenderer trail))
                {
                    trail.enabled = false;
                }
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (value == true)
            {
                text.gameObject.SetActive(true);
                if (TryGetComponent<TrailRenderer>(out TrailRenderer trail))
                {
                    trail.enabled = true;
                } 
           }
            debug = value;
        }
    }
    [HideInInspector]
    public GameObject jumpMarker;
    [HideInInspector]
    public TextMeshProUGUI text;
//
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        CreateStateMachine();
        CreateTransitions();
    }

    void Start()
    {
        currentStateType = MoveState.Idle;
        states[currentStateType].EnterState();
    }

    void Update()
    {
        UpdateState();
        CheckGrounded();
        CheckJump();
        states[currentStateType].UpdateState();
    }

    void FixedUpdate()
    {
        CheckEdges();
        states[currentStateType].FixedUpdateState(movement);
    }

    #region State Machine

    void CreateStateMachine()
    {
        // states[MoveState.Walk] = new GroundState(rb2D, playerStats);
        // states[MoveState.Jump] = new JumpState(rb2D, playerStats);
        // states[MoveState.Fall] = new FallState(rb2D, playerStats);
        //states[MoveState.Hang] = new HangState(rb2D, playerStats);

        foreach(MoveState state in Enum.GetValues(typeof(MoveState)))
        {
            transitions[state] = new List<Transition>();
        }
    }

    void CreateTransitions()
    {
        transitions[MoveState.Walk].Add(new Transition(MoveState.Walk, MoveState.Jump, () => isGrounded && jumpQueued));
        transitions[MoveState.Walk].Add(new Transition(MoveState.Walk, MoveState.Fall, () => !isGrounded));
        transitions[MoveState.Walk].Add(new Transition(MoveState.Walk, MoveState.Idle, () => isGrounded && rb2D.linearVelocity == Vector2.zero && moveDirection == 0));

        transitions[MoveState.Fall].Add(new Transition(MoveState.Fall, MoveState.Walk, () => isGrounded));
        transitions[MoveState.Fall].Add(new Transition(MoveState.Fall, MoveState.Jump, () => !isGrounded && jumpQueued && coyoteTimer < coyoteTime && !jumpPerformed));
        transitions[MoveState.Fall].Add(new Transition(MoveState.Fall, MoveState.Idle, () => isGrounded && rb2D.linearVelocity == Vector2.zero && moveDirection == 0));

        transitions[MoveState.Jump].Add(new Transition(MoveState.Jump, MoveState.Fall, () => (!isGrounded && !jumpHeldDown) || (!isGrounded && CheckVerticalSpeed())));
        //transitions[MoveState.Jump].Add(new Transition(MoveState.Jump, MoveState.Hang, () => !isGrounded && GravityController.Instance.doneChanging && CheckVerticalSpeed()));
        transitions[MoveState.Jump].Add(new Transition(MoveState.Jump, MoveState.Walk, () => isGrounded && CheckVerticalSpeed() && moveDirection != 0));
        transitions[MoveState.Jump].Add(new Transition(MoveState.Jump, MoveState.Idle, () => isGrounded && rb2D.linearVelocity == Vector2.zero && moveDirection == 0));
        // transitions[MoveState.Hang].Add(new Transition(MoveState.Hang, MoveState.Walk, () => isGrounded));
        //TODO: I really don't like this but this works for testing, find better solution
        //transitions[MoveState.Hang].Add(new Transition(MoveState.Hang, MoveState.Fall, () => !isGrounded && (((HangState)states[MoveState.Hang]).IsDone() || !jumpHeldDown)));

        transitions[MoveState.Idle].Add(new Transition(MoveState.Idle, MoveState.Jump, () => isGrounded && jumpQueued));
        transitions[MoveState.Idle].Add(new Transition(MoveState.Idle, MoveState.Walk, () => isGrounded && moveDirection!= 0));
        transitions[MoveState.Idle].Add(new Transition(MoveState.Idle, MoveState.Fall, () => !isGrounded));

    }

    bool CheckVerticalSpeed()
    {
        Vector2 direction = -transform.up;
        float directionalVelocity = Vector2.Dot(rb2D.linearVelocity,direction);
        return Mathf.Abs(directionalVelocity) <= 1f;
    }
    
    void UpdateState()
    {
        foreach(var transition in transitions[currentStateType])
        {
            tempStateType = transition.CheckState();
            if (tempStateType != currentStateType)
            {
                states[currentStateType].ExitState();
                currentStateType = tempStateType;
                EnterState();
                states[currentStateType].EnterState();
                return;
            }
        }
    }

    //TODO: I don't like this shit either
    void EnterState()
    {
        switch(currentStateType)
        {
            case MoveState.Jump:
                jumpQueued = false;
                jumpPerformed = true;
                break;
            case MoveState.Fall:
                break;
            case MoveState.Walk:
                break;
            case MoveState.Idle:
                break;
        }
        if (debug)
        {
            DebugState();
        }
    }

    void DebugState()
    {
        text.text = currentStateType.ToString();
        switch(currentStateType)
        {
            case MoveState.Jump:
                sprite.color = Color.blue;
                text.color = Color.blue;
                break;
            case MoveState.Fall:
                Instantiate(jumpMarker, transform.position, transform.rotation);
                sprite.color = Color.red;
                text.color = Color.red;
                break;
            case MoveState.Walk:
                sprite.color = Color.green;
                text.color = Color.green;
                break;
            case MoveState.Idle:
                sprite.color = Color.white;
                text.color = Color.white;
                break;
        }
    }

    void CheckGrounded()
    {
        float scale = transform.lossyScale.y/2f;
        hit2D = Physics2D.BoxCast(transform.position, new Vector2(1f,0.05f), Vector2.Angle(Vector2.down, -transform.up), -transform.up, scale, mask);
        if (hit2D == true && isGrounded == false)
        {
            jumpPerformed = false;
        }
        isGrounded = hit2D;
        if(hit2D == false)
        {
            if (coyoteTimer < coyoteTime && !jumpQueued)
            {
                coyoteTimer += Time.deltaTime;
            }
            isGrounded = false;
            movement = moveDirection * transform.right;
            return;
        }
        coyoteTimer = 0f;

        groundNormal = Vector2.Perpendicular(hit2D.normal);
        
        // currentAngle = Mathf.Abs(Mathf.Abs(Vector2.Angle(Vector2.right, groundNormal)) -180);
        // //isGrounded = playerStats._MaxMoveAngle > currentAngle;

        movement = Vector2.Dot(transform.right * moveDirection, groundNormal) * groundNormal * Mathf.Abs(moveDirection);
    }

    void CheckEdges()
    {
        Vector2 topLeft = transform.TransformPoint(new Vector2(-boxCollider.size.x/2,boxCollider.size.y/2));
        Vector2 skinLeft = new Vector2(topLeft.x+skinDepth, topLeft.y);

        Vector2 topRight = transform.TransformPoint(new Vector2(boxCollider.size.x/2,boxCollider.size.y/2));
        Vector2 skinRight = new Vector2(topRight.x-skinDepth, topRight.y);

        
    }

    void CheckJump()
    {
        if (jumpQueued && !isGrounded)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpQueueTime)
            {
                jumpQueued = false;
                jumpTimer = 0f;
            }
        }
    }

#endregion

#region AbilityEquip

    public void Equip(BaseStateAbility state)
    {
        states[state.state] = state;
    }

#endregion

#region Input

public void SetMoveDirection(float direction)
    {
        moveDirection = direction;
    }

public void StartJump()
    {
        jumpQueued = true;
        jumpHeldDown = true;
    }

public void EndJump()
    {
        jumpHeldDown = false;
    }

    #endregion
}


[CustomEditor(typeof(Player))]
public class Player_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var script = (Player)target;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);

        script.Debug = EditorGUILayout.Toggle(label: "Debug", script.Debug);

        if (script.Debug == false)
        {
            return;
        }

        script.jumpMarker = EditorGUILayout.ObjectField(label: "Jump Marker", script.jumpMarker, typeof(GameObject), allowSceneObjects: true) as GameObject;
        script.text = EditorGUILayout.ObjectField(label: "Text Field", script.text, typeof(TextMeshProUGUI), allowSceneObjects: true) as TextMeshProUGUI;
    }
}