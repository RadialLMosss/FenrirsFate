using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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
    public GameObject skillTreePanel;
    public LayerMask collectablePrizesLayer;
    public Image lifeBar;

    private float totalAttackTimer = 0.4f;
    private static float totalDashTimer = 0.4f;
    private float runningAttackTimer;
    private float runningDashTimer;

    //To Reset
    public static float baseLife = 10;
    public static float baseNormalDamage = 1;
    public static float baseStrongDamage = 1.5f;
    public static float baseDefense = 0;


    public static float lifePoints = 10;
    public static float totalLifePoints = 10;
    public static float defenseValue = 0;
    public static float normalDamageValue = 1;
    public static float strongDamageValue = 1.5f;
    //public static int damageValue = 1;

    public GameObject attackVol;

    public GameObject lifeRune;
    public GameObject furyRune;
    public GameObject courageRune;

    public static bool[] hasSkill = new bool[16];

    int dashCount;
    static int dashMaxCount = 1;
    bool isInvicible;

    public static float enemyBleedingDamage = 0.25f;
    static int crystalsMultipl = 1;
    static Player playerInst;

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
        lifeBar.color = new Color(lifeBar.color.r, lifeBar.color.g, 0, lifeBar.color.a);
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
    bool cantMove;
    public GameObject pausedPanel;
    private void Update()
    {
        if (runningAttackTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1") && !cantMove)
            {
                Attack();
                runningAttackTimer = totalAttackTimer;
            }
            else if(Input.GetButtonDown("Fire2") && !cantMove)
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
            skillTreePanel.SetActive(!skillTreePanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            pausedPanel.SetActive(!pausedPanel.activeSelf);
            if(Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }

    void FixedUpdate()
    {
        if((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && !cantMove)
        {
            Move();
        }
        if(!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") && (rb.velocity != Vector3.zero || rbSpeed != originalRbSpeed))
        {
            if(!cantMove)
            {
                audioSourceFootsteps.mute = true;
                rb.velocity = Vector3.zero;
                rbSpeed = originalRbSpeed;
                anim.SetBool("isWalking", false);
            }
        }

        if (runningDashTimer <= 0 && !isInvicible)
        {
            if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Jump")) && rbSpeed == originalRbSpeed)
            {
                if(!cantMove)
                {
                    StartCoroutine(Dash());
                }
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
        if(!cantMove)
        {
            Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = forward * speed * Time.deltaTime * Input.GetAxis("Vertical");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            if (heading == Vector3.zero)
                return;

            transform.forward = heading;
            anim.SetBool("isWalking", true);
            audioSourceFootsteps.mute = false;
            rb.velocity = heading * rbSpeed;
        }
    }

    public void Attack()
    {
        if(Random.Range(0, 2) == 0)
        {
            anim.SetTrigger("Claw");
        }
        else
        {
            anim.SetTrigger("Claw2");
        }
        audioSource.PlayOneShot(clips[1]);
        AttackEffect(3f, attackVol.transform, normalDamageValue);
    }
    public void Attack2()
    {
        anim.SetTrigger("Bite");
        audioSource.PlayOneShot(clips[0]);
        AttackEffect(3f, attackVol.transform, strongDamageValue);
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
                    hitCollider.GetComponent<Enemy>().TakeDamage(damage, hasSkill[10]);
                }
                else
                {
                    hitCollider.GetComponent<Enemy>().TakeDamage(damage, false);
                }
            }
            else if(hitCollider.gameObject.layer == 9)
            {
                hitCollider.GetComponent<CollectablePrize>().OpenChest();
            }
        }
    }

    // =====================================================================================
    bool cantBeDamaged;

    IEnumerator DamageRecovery()
    {
        cantBeDamaged = true;
        yield return new WaitForSeconds(1.5f);
        cantBeDamaged = false;
    }


    public void UpdateLifePoints(float lifeValue)
    {
        if(lifeValue < 0)
        {
            if(isInvicible || cantBeDamaged)
            {
                lifeValue = 0;
            }
            else
            {
                lifeValue += defenseValue/5;
            }
            StartCoroutine(DamageRecovery());
        }
        lifePoints += lifeValue;
        if(lifePoints > totalLifePoints)
        {
            lifePoints = totalLifePoints;
        }
        lifeBar.fillAmount = lifePoints / totalLifePoints;
        if(lifePoints <= 0)
        {
            audioSource.PlayOneShot(clips[3]);
            anim.SetTrigger("Death");
            StartCoroutine(DieDelay());
        }
    }
    public GameObject cineCamera;
    public GameObject mainCamera;

    public IEnumerator PlayChainsAnim()
    {
        cantMove = true;
        cineCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        rbSpeed = 0;
        anim.Play("Unchain");
        yield return new WaitForSeconds(9f);
        rbSpeed = originalRbSpeed;
        mainCamera.gameObject.SetActive(true);
        cineCamera.SetActive(false);
        cantMove = false;
    }


    IEnumerator DieDelay()
    {
        rbSpeed = 0;
        yield return new WaitForSeconds(1);
        PlayerDeath();
        rbSpeed = originalRbSpeed;
    }

    bool isGoingToOtherLevel;
    IEnumerator NextLevelDelay(Collider coll, int levelIndex)
    {
        isGoingToOtherLevel = true;
        yield return new WaitForSeconds(0.75f);
        coll.gameObject.SetActive(false);
        gameManager.NextLevel(levelIndex);
        isGoingToOtherLevel = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish") && !isGoingToOtherLevel)
        {
            StartCoroutine(NextLevelDelay(other, 1));
        }
        else if(other.CompareTag("Finish2") && !isGoingToOtherLevel)
        {
            StartCoroutine(NextLevelDelay(other, 2));
        }
        else if(other.CompareTag("Damage"))
        {
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
    

    public void GetCollectablePrizeEffect(CollectablePrize prize)
    {
        audioSource.PlayOneShot(clips[7]);
        switch (prize.type)
        {
            case CollectablePrize.Type.LifeOrbs:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        UpdateLifePoints(2);
                        break;

                    case CollectablePrize.Size.Medium:
                        UpdateLifePoints(4);
                        break;

                    case CollectablePrize.Size.Big:
                        UpdateLifePoints(6);
                        break;
                }
                break;

            case CollectablePrize.Type.FuryOrbs:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        UpdateFuryCurrency(10);
                        break;

                    case CollectablePrize.Size.Medium:
                        UpdateFuryCurrency(15);
                        break;

                    case CollectablePrize.Size.Big:
                        UpdateFuryCurrency(20);
                        break;
                }
                break;

            case CollectablePrize.Type.CrystalBag:
                switch (prize.size)
                {
                    case CollectablePrize.Size.Small:
                        UpdateCrystalCurrency(5);
                        break;

                    case CollectablePrize.Size.Medium:
                        UpdateCrystalCurrency(10);
                        break;

                    case CollectablePrize.Size.Big:
                        UpdateCrystalCurrency(15);
                        break;
                }
                break;

            case CollectablePrize.Type.LifeRune:
                totalLifePoints += 6;
                lifeBar.color = new Color(0, lifeBar.color.g, lifeBar.color.b, lifeBar.color.a);
                UpdateLifePoints(0);
                lifeRune.SetActive(true);
                break;

            case CollectablePrize.Type.FuryRune: //damage
                normalDamageValue += 2;
                strongDamageValue += 3;
                furyRune.SetActive(true);
                break;

            case CollectablePrize.Type.CourageRune: //defense
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
        crystalText.text = crystals.ToString() + "x";
    }

    public void UpdateFuryCurrency(int value)
    {
        fury += value;
        furyText.text = fury.ToString() + "x";
    }

    void PlayerDeath()
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
        lifeBar.color = new Color(255, lifeBar.color.g, lifeBar.color.b, lifeBar.color.a);
        totalLifePoints = baseLife;
        lifePoints = totalLifePoints;
        UpdateLifePoints(0);
    }
}
