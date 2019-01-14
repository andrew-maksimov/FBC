using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    [SerializeField]
    private float tapForce = 10;
    [SerializeField]
    private float tiltSmooth = 5;

    private Rigidbody2D rigidBody2D;

    private Quaternion downRotation = Quaternion.Euler(0, 0, -20);
    private Quaternion forwardRotation = Quaternion.Euler(0, 0, 40);

    public AudioSource tapSound;
    public AudioSource facedSound;

    public delegate void BirdDelegate();
    public static event BirdDelegate OnFaced;
    public static event BirdDelegate OnScored;

    #region MonoBehaviour CallBacks

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.simulated = false;
        transform.position = Vector3.up * 10;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.GameState == GameState.Play)
        {
            rigidBody2D.velocity = Vector2.zero;
            transform.rotation = forwardRotation;
            rigidBody2D.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapSound.Play();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnEnable()
    {
        GameManager.OnStarted += OnGameStarted;
    }

    void OnDisable()
    {
        GameManager.OnStarted -= OnGameStarted;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Pipe")
        {
            OnFaced?.Invoke();
            facedSound.Play();
        }
        else if (collider.gameObject.tag == "Boundary")
        {
            OnScored?.Invoke();
        }
    }

    #endregion

    void OnGameStarted()
    {
        rigidBody2D.velocity = Vector3.zero;
        rigidBody2D.simulated = true;
        transform.position = Vector3.zero;
    }
}