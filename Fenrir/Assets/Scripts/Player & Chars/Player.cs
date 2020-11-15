using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float speed = 350f;
    float originalRbSpeed = 10f;
    float rbSpeed = 10f;
    float dashSpeed = 50f;
    float dashDuration = 0.15f;
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
    public Image lifeBarBG;

    private float totalTimer = 0.4f;
    private float runningWalkTimer;
    private float runningDashTimer;
    static float lifePoints = 10;
    float totalLifePoints = 10;
    float defenseValue;
    int damageValue = 1;

    public Text itemEffectDebug;

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
            rbSpeed = originalRbSpeed;
        }

        if (runningDashTimer <= 0)
        {
            if(Input.GetButtonDown("Fire2") && rbSpeed == originalRbSpeed)
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
        rbSpeed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rbSpeed = originalRbSpeed;
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
            hitCollider.GetComponent<Enemy>().TakeDamage(damageValue);
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

    public void UpdateLifePoints(float lifeValue)
    {
        if(lifeValue < 0)
        {
            lifeValue += lifeValue * defenseValue/10;
        }
        lifePoints += lifeValue;
        if(lifePoints > totalLifePoints)
        {
            lifePoints = totalLifePoints;
        }
        lifeBar.fillAmount = lifePoints / totalLifePoints;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<SpikeTrap>())
        {
            SpikeTrap spikeTrap = collision.gameObject.GetComponent<SpikeTrap>();
            if(spikeTrap.trapType == SpikeTrap.TrapType.IfTouched)
            {
                StartCoroutine(spikeTrap.DelayedSpikes());
            }
        }
    }

    // ==================================================================================================
    
    IEnumerator ItemEffectDebug_UI(string description)
    {
        itemEffectDebug.text = description;
        yield return new WaitForSeconds(3);
        itemEffectDebug.text = "...";
    }

    public void GetCollectablePrizeEffect(CollectablePrize prize)
    {
        switch (prize.type)
        {
            case CollectablePrize.Type.LifePotion:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        StartCoroutine(ItemEffectDebug_UI("LifePotionSmall: +2 HP"));
                        UpdateLifePoints(2);
                        break;

                    case CollectablePrize.Size.Medium:
                        StartCoroutine(ItemEffectDebug_UI("LifePotionMedium: +4 HP"));
                        UpdateLifePoints(4);
                        break;

                    case CollectablePrize.Size.Big:
                        StartCoroutine(ItemEffectDebug_UI("LifePotionBig: +6 HP"));
                        UpdateLifePoints(6);
                        break;
                }
                break;

            case CollectablePrize.Type.FuryPotion:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        StartCoroutine(ItemEffectDebug_UI("FuryPotionSmall: +10 Fury"));
                        UpdateFuryCurrency(10);
                        break;

                    case CollectablePrize.Size.Medium:
                        StartCoroutine(ItemEffectDebug_UI("FuryPotionMedium: +15 Fury"));
                        UpdateFuryCurrency(15);
                        break;

                    case CollectablePrize.Size.Big:
                        StartCoroutine(ItemEffectDebug_UI("FuryPotionBig: +20 Fury"));
                        UpdateFuryCurrency(20);
                        break;
                }
                break;

            case CollectablePrize.Type.CrystalBag:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        StartCoroutine(ItemEffectDebug_UI("CrystalBagSmall: +5 Crystals"));
                        UpdateCrystalCurrency(5);
                        break;

                    case CollectablePrize.Size.Medium:
                        StartCoroutine(ItemEffectDebug_UI("CrystalBagSmall: +10 Crystals"));
                        UpdateCrystalCurrency(10);
                        break;

                    case CollectablePrize.Size.Big:
                        StartCoroutine(ItemEffectDebug_UI("CrystalBagSmall: +15 Crystals"));
                        UpdateCrystalCurrency(15);
                        break;
                }
                break;

            case CollectablePrize.Type.LifeRune:
                StartCoroutine(ItemEffectDebug_UI("LifeRune: MaxLife 10 -> 16"));
                totalLifePoints = 16;
                lifeBarBG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 640f);
                lifeBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 640f);
                UpdateLifePoints(0);
                break;

            case CollectablePrize.Type.FuryRune: //damage
                StartCoroutine(ItemEffectDebug_UI("FuryRune: Damage x2"));
                damageValue = 2; 
                break;

            case CollectablePrize.Type.CourageRune: //defense
                StartCoroutine(ItemEffectDebug_UI("CourageRune: Defense x2"));
                defenseValue = 2.5f;
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
        GameManager.levelCount = -1;
        gameManager.NextLevel(1);
        UpdateCrystalCurrency(-crystals);
        defenseValue = 0;
        damageValue = 1;
        totalLifePoints = 10;
        lifePoints = totalLifePoints;
        lifeBarBG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400f);
        lifeBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400f);
        UpdateLifePoints(0);
    }
}
