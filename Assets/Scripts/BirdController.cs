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

    #region MonoBehaviour CallBacks
    
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rigidBody2D.velocity = Vector2.zero;
            transform.rotation = forwardRotation;
            rigidBody2D.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Pipe")
        {
            Debug.Log("GameOver");
        }
    }

    #endregion
}