using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AapoMovement : MonoBehaviour
{
    private CharacterController controller;


    private float stashedSpeed;
    public float speed = 6f;

    [SerializeField] private float gravityValue;
    [SerializeField] private float fallValue;
    private float stashedGravityValue;
    [SerializeField] private float jumpHeight = 3f;

    private GameObject groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private Vector3 normalMovement;
    public bool isGrounded;
    public bool isMoving;

    [Header("FPS TUTORIAL")]
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;
    private float velocityY;
    private void Awake()
    {
        groundCheck = GameObject.Find("groundCheck");
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stashedSpeed = speed;
        stashedGravityValue = gravityValue;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravityValue * 2f * Time.deltaTime;

        Vector3 velocityTwo = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

        controller.Move(velocityTwo * Time.deltaTime);


        if (isGrounded && Input.GetButton("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
           
        }
    }
}
