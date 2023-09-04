using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float speed;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float smooth;

    [SerializeField]
    private float jumpForce;

    private float vertical;
    private float horizontal;

    private Vector3 movement;
    private Vector3 smoothMovement;

    private float height;
    private float smoothHeight;

    private CharacterController player;
    private float fastRun;

    private Animator playerAnimator;

    float yRot = 0;
    float xRot = 0;

    public Camera playerCamera;

    private void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 forward = transform.forward * vertical;
        Vector3 direction = transform.right * horizontal;
        movement = (forward + direction) * speed;

        if (Input.GetKey(KeyCode.LeftShift))
            fastRun = 2f;
        else
            fastRun = 0;

        if (player.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            height = jumpForce;
        }
        else if (!player.isGrounded)
        {
            height -= gravity * Time.deltaTime;
        }


        smoothMovement = Vector3.Lerp(smoothMovement, movement, smooth * Time.deltaTime);

        smoothHeight = Mathf.Lerp(smoothHeight, height, smooth * Time.deltaTime);
        smoothMovement.y = smoothHeight;

        player.Move(smoothMovement * Time.deltaTime * (speed + fastRun));

        xRot += Input.GetAxis("Mouse X");
        yRot += Input.GetAxis("Mouse Y");

        //playerCamera.transform.rotation = Quaternion.Euler(-yRot, playerCamera.transform.rotation.y, playerCamera.transform.rotation.z);
        transform.rotation = Quaternion.Euler(0f, xRot, 0f);

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAnimator.SetTrigger("attack");
        }
    }
}