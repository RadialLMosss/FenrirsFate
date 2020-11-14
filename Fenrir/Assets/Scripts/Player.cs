using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float speed = 350f;
    float rbSpeed = 7.5f;
    float dashSpeed = 50f;
    Rigidbody rb;

    Vector3 forward, right;

    GameManager gameManager;

    [HideInInspector] public static int furia;
    [HideInInspector] public static int crystals;
    public static int enemiesToDefeat;

    public Text furyText;
    public Text crystalText;
    public ShopManager shopManager;

    public LayerMask enemyLayer;
    public GameObject attackVol;
    public GameObject skillTreePanel;
    public LayerMask collectablePrizesLayer;
    public Image lifeBar;

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

    private float totalTimer = 0.4f;
    private float runningWalkTimer;
    private float runningDashTimer;
    float dashDuration = 0.13f;

    // =====================================================================================

    private void Update()
    {
        if (runningWalkTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                runningWalkTimer = totalTimer;
            }
        }
        else
        {
            runningWalkTimer -= Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }
        if(!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
        {
            rb.velocity = Vector3.zero;
        }

        if (runningDashTimer <= 0)
        {
            if(Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            runningDashTimer -= Time.deltaTime;
        }

    }
    //Dash
    IEnumerator Dash()
    {
        float originalSpeed = rbSpeed;
        rbSpeed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rbSpeed = originalSpeed;
        runningDashTimer = totalTimer;
    }

    private void Move()
    {
        Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * speed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if (heading == Vector3.zero)
            return;

        transform.forward = heading;

        //transform.position += rightMovement;
        //transform.position += upMovement;
        rb.velocity = heading * rbSpeed;
    }

    public void Attack()
    {
        attackVol.SetActive(true);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Enemy>().EnemyDeath();
        }

        Collider[] hitColliders2 = Physics.OverlapSphere(transform.position, 6, collectablePrizesLayer);
        foreach (Collider hitCollider in hitColliders2)
        {
            hitCollider.GetComponent<CollectablePrize>().OpenChest();
        }
        StartCoroutine(DisableAttackVol());
    }
    IEnumerator DisableAttackVol()
    {
        yield return new WaitForSeconds(0.15f);
        attackVol.SetActive(false);
    }

    // =====================================================================================
    static float lifePoints = 10;
    public void UpdateLifePoints(float value)
    {
        lifePoints += value;
        lifeBar.fillAmount = lifePoints / 10;
        if(lifePoints <= 0)
        {
            PlayerDeath();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            other.gameObject.SetActive(false);
            gameManager.NextLevel(1);
        }
        else if(other.CompareTag("Finish2"))
        {
            other.gameObject.SetActive(false);
            gameManager.NextLevel(2);
        }
        else if(other.CompareTag("Damage"))
        {
            UpdateLifePoints(-2);
        }
        else if(other.CompareTag("Crystal"))
        {
            Destroy(other.gameObject);
            UpdateCrystalCurrency(10);
        }
        else if(other.CompareTag("SkillTreePoint"))
        {
            skillTreePanel.SetActive(true);
        }
        else if (other.GetComponent<CollectablePrize>())
        {
            CollectablePrize prize = other.GetComponent<CollectablePrize>();
            if (prize.version == CollectablePrize.Version.Shop)
            {
                shopManager.ShowShopWindow(prize);
            }
            else
            {
                if(prize.isChestOpened)
                {
                    GetCollectablePrizeEffect(prize);
                }
            }
        }
    }

    // ==================================================================================================

    public static void GetCollectablePrizeEffect(CollectablePrize prize)
    {
        switch (prize.type)
        {
            case CollectablePrize.Type.LifePotion:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;

            case CollectablePrize.Type.FuryPotion:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;

            case CollectablePrize.Type.CrystalBag:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;

            case CollectablePrize.Type.LifeRune:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;

            case CollectablePrize.Type.FuryRune:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;

            case CollectablePrize.Type.CourageRune:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        break;

                    case CollectablePrize.Size.Medium:
                        break;

                    case CollectablePrize.Size.Big:
                        break;
                }
                break;
        }

        Destroy(prize.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("SkillTreePoint"))
        {
            skillTreePanel.SetActive(false);
        }
    }


    public void UpdateCrystalCurrency(int value)
    {
        crystals += value;
        crystalText.text = "CRISTAIS = " + crystals.ToString();
    }

    public void UpdateFuryCurrency(int value)
    {
        furia += value;
        furyText.text = "FÚRIA = " + furia.ToString();
    }

    void PlayerDeath()
    {
        crystals = 0;
        crystalText.text = "CRISTAIS = " + crystals.ToString();
        GameManager.levelCount = -1;
        gameManager.NextLevel(1);
        lifePoints = 10;
        UpdateLifePoints(0);
    }
}
