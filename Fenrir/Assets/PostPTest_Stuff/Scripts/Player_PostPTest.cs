using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_PostPTest : MonoBehaviour
{
    static float speed = 350f;
    static float originalRbSpeed = 10f;
    static float rbSpeed = 10f;
    float dashSpeed = 50f;
    static float dashDuration = 0.15f;
    Rigidbody rb;

    Vector3 forward, right;

    GameManager gameManager;

    [HideInInspector] public static int fury;
    [HideInInspector] public static int crystals;
    public static int enemiesToDefeat;

    public Text furyText;
    public Text crystalText;
    public ShopManager shopManager;

    public LayerMask damagableLayers;
    public GameObject attackVol;
    public GameObject attackVol2;
    public GameObject skillTreePanel;
    public LayerMask collectablePrizesLayer;
    public Image lifeBar;
    public RectTransform lifeBarRect;
    public RectTransform lifeBarBGRect;

    private float totalAttackTimer = 0.4f;
    private static float totalDashTimer = 0.4f;
    private float runningAttackTimer;
    private float runningDashTimer;

    //To Reset
    public static float baseLife = 10;
    public static float baseNormalDamage = 1;
    public static float baseStrongDamage = 2;
    public static float baseDefense = 0;


    public static float lifePoints = 10;
    public static float totalLifePoints = 10;
    public static float defenseValue = 0;
    public static float normalDamageValue = 1;
    public static float strongDamageValue = 2;
    //public static int damageValue = 1;

    public Text itemEffectDebug;

    public GameObject lifeRune;
    public GameObject furyRune;
    public GameObject courageRune;

    public static bool[] hasSkill = new bool[16];

    int dashCount;
    static int dashMaxCount = 1;
    bool isInvicible;

    public static float enemyBleedingDamage = 0.25f;
    static int crystalsMultipl = 1;
    static Player_PostPTest playerInst;

    public bool playing = false;
    public Animator anim;
    public AudioSource audioSource;
    public AudioSource audioSourceFootsteps;    
    public AudioClip[] clips;

    

    public static void EnableSkill(int skillIndex)
    {
        hasSkill[skillIndex] = true;
        switch (skillIndex)
        {
            // BRANCH 1 - L
            case 0: 
                // Ataque Mordida mais forte
                strongDamageValue += 1;
                baseStrongDamage += 1;
                break;

            case 1: // Ataque Mordida maior alcance
                break;

            case 2: 
                // Mordida faz inimigos sofrerem + dano por um tempo
                // hasSkill[2] set to true is enough
                break;

            case 3: 
                // Mordida empurra inimigos (exceto boss)
                // hasSkill[3] set to true is enough
                break;

            // BRANCH 2
            case 4: 
                // Dash delay menor
                totalDashTimer /= 2;
                break;

            case 5: 
                // Dash percorre maior distância
                dashDuration *= 2;
                break;

            case 6: 
                // Maior Velocidade de Movimento
                speed *= 1.25f;
                originalRbSpeed *= 1.25f;
                rbSpeed *= 1.25f;
                break;

            case 7: 
                // Da +1 recarga de Dash
                dashMaxCount++;
                break;

            // BRANCH 3
            case 8: 
                // Ataque Garra + forte
                normalDamageValue += 1;
                baseNormalDamage += 1;
                break;

            case 9: // Garra pode refletir ataques à distância (exceto boss)
                break;

            case 10:
                // Garra faz inimigos sangrarem por um tempo
                //hasSkill[10] set to true is enough
                break;

            case 11:
                // Aumenta dano de sangramento
                enemyBleedingDamage *= 2;
                break;

            // BRANCH 4 - R
            case 12:
                // - Dano sofrido
                defenseValue *= 1.2f;
                baseDefense *= 1.2f;
                break;

            case 13:
                // + vida
                playerInst.EnableSkillMoreLife();
                break;

            case 14: 
                // Minérios obtidos
                crystalsMultipl *= 2;
                break;

            case 15:
                // Descontos Ferreiro
                //hasSkill[15] set to true is enough
                break;
        }
    }


    void EnableSkillMoreLife()
    {
        baseLife += 4;
        totalLifePoints = baseLife;
        lifeBarBGRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseLife * 40);
        lifeBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseLife * 40);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        playerInst = this;
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
        if(playing)
        {
            if (runningAttackTimer <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                    runningAttackTimer = totalAttackTimer;
                }
                else if(Input.GetButtonDown("Fire2"))
                {
                    Attack2();
                    runningAttackTimer = totalAttackTimer*2;
                }
            }
            else
            {
                runningAttackTimer -= Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                skillTreePanel.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if(playing)
        {
            if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }
        if(!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") && (rb.velocity != Vector3.zero || rbSpeed != originalRbSpeed))
        {
            anim.SetBool("isWalking", false);
            audioSourceFootsteps.mute = true;
            rb.velocity = Vector3.zero;
            rbSpeed = originalRbSpeed;
        }

        if (runningDashTimer <= 0 && !isInvicible)
        {
            if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Jump")) && rbSpeed == originalRbSpeed)
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            runningDashTimer -= Time.deltaTime;
        }
        }
    }

    //Dash
    IEnumerator Dash()
    {
        isInvicible = true;
        rbSpeed = dashSpeed;
        anim.SetTrigger("Dash");
        audioSource.PlayOneShot(clips[2]);
        yield return new WaitForSeconds(dashDuration);
        rbSpeed = originalRbSpeed;
        isInvicible = false;
        dashCount++;
        if(dashCount >= dashMaxCount)
        {
            dashCount = 0;
            runningDashTimer = totalDashTimer;
        }
        else
        {
            StartCoroutine(SecondDashTime());
        }

    }

    IEnumerator SecondDashTime()
    {
        yield return new WaitForSeconds(dashDuration);
        runningDashTimer = totalDashTimer;
    }

    private void Move()
    {
        Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * speed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if (heading == Vector3.zero)
            return;

        transform.forward = heading;
        anim.SetBool("isWalking", true);
        audioSourceFootsteps.mute = false;
        //transform.position += rightMovement;
        //transform.position += upMovement;
        rb.velocity = heading * rbSpeed;
    }

    public void Attack()
    {
        attackVol.SetActive(true);
        anim.SetTrigger("Bite");
        audioSource.PlayOneShot(clips[0]);
        AttackEffect(2f, attackVol.transform, normalDamageValue);
        StartCoroutine(DisableAttackVol(attackVol));
    }
    public void Attack2()
    {
        attackVol2.SetActive(true);
        anim.SetTrigger("Claw");
        audioSource.PlayOneShot(clips[1]);
        AttackEffect(3.5f, attackVol2.transform, strongDamageValue * 2);
        StartCoroutine(DisableAttackVol(attackVol2));
    }
    void AttackEffect(float range, Transform damagePoint, float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(damagePoint.position, range, damagableLayers);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.layer == 8)
            {
                if(damage == normalDamageValue)
                {
                    hitCollider.GetComponent<Enemy_PostPTest>().TakeDamage(damage, hasSkill[10]);
                }
                else
                {
                    hitCollider.GetComponent<Enemy_PostPTest>().TakeDamage(damage, false);
                }
            }
            else if(hitCollider.gameObject.layer == 9)
            {
                hitCollider.GetComponent<CollectablePrize>().OpenChest();
            }
        }
    }

    IEnumerator DisableAttackVol(GameObject obj)
    {
        yield return new WaitForSeconds(0.15f);
        obj.SetActive(false);
    }

    // =====================================================================================

    public void UpdateLifePoints(float lifeValue)
    {
        if(lifeValue < 0)
        {
            if(isInvicible)
            {
                lifeValue = 0;
            }
            else
            {
                lifeValue += defenseValue/5;
            }
        }
        lifePoints += lifeValue;
        if(lifePoints > totalLifePoints)
        {
            lifePoints = totalLifePoints;
        }
        lifeBar.fillAmount = lifePoints / totalLifePoints;
        if(lifePoints <= 0)
        {
            playing = false;
            audioSource.PlayOneShot(clips[3]);
            anim.SetTrigger("Death");
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
            other.GetComponentInParent<Enemy_PostPTest>().anim.SetTrigger("Attack");
            AudioClip clipEnemy = other.GetComponentInParent<Enemy_PostPTest>().clips[0];
            other.GetComponentInParent<Enemy_PostPTest>().audioSource.PlayOneShot(clipEnemy);
            UpdateLifePoints(-2);
        }
        else if(other.CompareTag("Crystal"))
        {
            Destroy(other.gameObject);
            UpdateCrystalCurrency(10);
            audioSource.PlayOneShot(clips[5]);
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
            else if(prize.version == CollectablePrize.Version.LootBox)
            {
                shopManager.ShowLootBoxShopWindow(prize);
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
        audioSource.PlayOneShot(clips[7]);
        switch (prize.type)
        {
            case CollectablePrize.Type.LifeOrbs:
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

            case CollectablePrize.Type.FuryOrbs:
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
                totalLifePoints += 6;
                lifeBarBGRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalLifePoints * 40);
                lifeBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalLifePoints * 40);
                UpdateLifePoints(0);
                lifeRune.SetActive(true);
                break;

            case CollectablePrize.Type.FuryRune: //damage
                StartCoroutine(ItemEffectDebug_UI("FuryRune: Damage x2"));
                normalDamageValue += 2;
                strongDamageValue += 3;
                furyRune.SetActive(true);
                break;

            case CollectablePrize.Type.CourageRune: //defense
                StartCoroutine(ItemEffectDebug_UI("CourageRune: Defense x2"));
                defenseValue = 2.5f;
                courageRune.SetActive(true);
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
        if(crystals > 0)
        {
            value *= crystalsMultipl;
        }
        crystals += value;
        crystalText.text = "CRISTAIS = " + crystals.ToString();
    }

    public void UpdateFuryCurrency(int value)
    {
        audioSource.PlayOneShot(clips[6]);
        fury += value;
        furyText.text = "FÚRIA = " + fury.ToString();
    }

    public void PlayerDeath()
    {
        if(playing)
        {
            GameManager.levelCount = -1;
            gameManager.NextLevel(1);
            UpdateCrystalCurrency(-crystals);
            courageRune.SetActive(false);
            furyRune.SetActive(false);
            lifeRune.SetActive(false);
            defenseValue = baseDefense;
            normalDamageValue = baseNormalDamage;
            strongDamageValue = baseStrongDamage;
            totalLifePoints = baseLife;
            lifePoints = totalLifePoints;
            lifeBarBGRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseLife * 40);
            lifeBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseLife * 40);
            UpdateLifePoints(0);
        }
    }
}

