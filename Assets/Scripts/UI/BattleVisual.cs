using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisual : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private TextMeshProUGUI levelText;

    private Animator anim;

    private int currHealth;
    private int maxHealth;
    private int level;

    private const string LEVEL_ABB = "Lvl: ";

    private const string ATTACK_PARAM = "IsAttacking";
    private const string HIT_PARAM = "IsHit";
    private const string DEAD_PARAM = "IsDead";

    void Start()
    {
        anim = GetComponent<Animator>();
        SetStartingValues(100, 100, 1);
    }

    public void SetStartingValues(int currHealth, int maxHealth, int level)
    {
        this.currHealth = currHealth;
        this.maxHealth = maxHealth;
        this.level = level;
        levelText.text = LEVEL_ABB + this.level.ToString();
        UpdateHealthBar();
    }

    public void ChangeHealth(int currHealth)
    {
        this.currHealth = currHealth;
        UpdateHealthBar();

        if (currHealth <= 0)
        {
            PlayDeathAnim();
            Destroy(gameObject, 1f);
        }
    }

    public void UpdateHealthBar()
    {
        healthbar.maxValue = maxHealth;
        healthbar.value = currHealth;
    }

    public void PlayAttackAnim()
    {
        anim.SetTrigger(ATTACK_PARAM);
    }

    public void PlayHitAnim()
    {
        anim.SetTrigger(HIT_PARAM);
    }

    public void PlayDeathAnim()
    {
        anim.SetTrigger(DEAD_PARAM);
    }
}
