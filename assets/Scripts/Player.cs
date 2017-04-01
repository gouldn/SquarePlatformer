using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class Player : MonoBehaviour {

    public float jumpHeight = 4f;
    public float timeToApex = .4f;
    public float moveSpeed;
    public GameObject projectilePrefab;
    public AudioClip jumpSound, fireSound, deathSound, checkSound, goalSound;
    public Material checkMaterial;
    public Sprite goalAchievedSprite;

    private float gravity;
    private float jumpVelocity;
    private int facingRight = 1;

    private Controller2D controller;
    private Bounds collisionBounds;
    private Vector3 velocity;

    public delegate void OnDestroy();
    public event OnDestroy DestroyCallback;

    public delegate void OnCheckpoint();
    public event OnCheckpoint CheckpointCallback;

    public delegate void OnGoal();
    public event OnGoal GoalCallback;

    void Start()
    {
        collisionBounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        controller = GetComponent<Controller2D>();

        gravity = -((2 * jumpHeight) / Mathf.Pow(timeToApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
    }

    void Update()
    {
        if(controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.x < 0 && facingRight == 1) {
            facingRight = -1;
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        } else if (input.x > 0 && facingRight == -1)
        {
            facingRight = 1;
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }

        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
            velocity.y = jumpVelocity;
            GameManager.audioSource.PlayOneShot(jumpSound);
        }

        if(Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
        {
            velocity.y = jumpVelocity / 8;
        }

        if (Input.GetKeyDown(KeyCode.F))
            Fire();

        if (facingRight == 1)
            velocity.x = input.x * moveSpeed;
        else velocity.x = input.x * -moveSpeed;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime, facingRight);
    }

    public void Fire()
    {
        Vector3 offset = collisionBounds.extents.x * transform.right;
        Instantiate(projectilePrefab, transform.position + offset, transform.rotation);

        GameManager.audioSource.PlayOneShot(fireSound);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Killer"))
        {
            Destroy(this.gameObject);
            if (DestroyCallback != null)
            {
                DestroyCallback();
            }
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().sprite = goalAchievedSprite;
            GameManager.audioSource.PlayOneShot(goalSound);
            GoalCallback();
        }
        else if (other.gameObject.CompareTag("JumpArrow"))
        {
            velocity.y = jumpVelocity;
        }
        else if (other.gameObject.CompareTag("Checkpoint"))
        {
            if(other.gameObject.GetComponent<Checkpoint>().active)
            {
                GameManager.audioSource.PlayOneShot(checkSound);
                other.gameObject.GetComponent<MeshRenderer>().material = checkMaterial;
                other.gameObject.GetComponent<Checkpoint>().active = false;
                CheckpointCallback();
            }
        }
    }
}
