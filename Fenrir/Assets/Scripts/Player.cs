using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 10;
    Rigidbody rb;

    Vector3 forward, right;

    GameManager gameManager;

    [HideInInspector] public int furia;
    [HideInInspector] public int crystals;

    public Text furyText;
    public Text crystalText;

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

    private float totalTimer = 0.5f;
    private float runningTimer;

    void Update()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }

        if(runningTimer <= 0)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Attack();
                runningTimer = totalTimer;
            }
        }
        else
        {
            runningTimer -= Time.deltaTime;
        }

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
        else if(other.CompareTag("Damage"))
        {
            PlayerDeath();
        }
        else if(other.CompareTag("Crystal"))
        {
            crystals += 10;
            crystalText.text = "CRISTAIS = " + crystals.ToString();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("SkillTreePoint"))
        {
            skillTreePanel.SetActive(true);
        }
    }
    public LayerMask enemyLayer;
    public GameObject attackVol;
    public GameObject skillTreePanel;

    public void Attack()
    {
        attackVol.SetActive(true);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5,  enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Enemy>().EnemyDeath();
        }
        StartCoroutine(DisableAttackVol());
    }
    IEnumerator DisableAttackVol()
    {
        yield return new WaitForSeconds(0.5f);
        attackVol.SetActive(false);
    }

    public void GetFuryCurrency()
    {
        furia += 5;
        furyText.text = "FÚRIA = " + furia.ToString();
    }

    void PlayerDeath()
    {
        // não perder fury currency nem as habilidades já adquiridas
        crystals = 0;
        gameManager.levelCount = 0;
        gameManager.NextLevel(1);
    }
}
