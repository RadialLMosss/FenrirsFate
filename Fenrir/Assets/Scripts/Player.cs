using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 10;
    Rigidbody rb;

    Vector3 forward, right;

    GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        if(Input.anyKey)
        {
            Move();
        }
        //transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);

    }

    private void Move()
    {

        Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * speed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if (heading == Vector3.zero)
            return;

        transform.forward = heading;

        transform.position += rightMovement;
        transform.position += upMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            gameManager.NextLevel(1);
        }
        else if(other.CompareTag("Finish2"))
        {
            gameManager.NextLevel(2);
        }
    }
}
