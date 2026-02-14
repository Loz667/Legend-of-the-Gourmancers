using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LotG.UI.Battle
{
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

        void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void SetStartingValues(int currHealth, int maxHealth, int level)
        {
            this.currHealth = currHealth;
            this.maxHealth = maxHealth;
            this.level = level;
            levelText.text = LEVEL_ABB + this.level.ToString();
            UpdateHealthBar();
        }

        public void AdjustPlayerHealth(int currHealth)
        {
            this.currHealth = currHealth;
            //if (currHealth <= 0)
            //{
            //    PlayDeathAnim();
            //    Destroy(gameObject, 1f);
            //}
            UpdateHealthBar();
        }

        public void AdjustEnemyHealth(int currHealth)
        {
            this.currHealth = currHealth;
            //if (currHealth >= maxHealth)
            //{
            //    PlayDeathAnim();
            //    Destroy(gameObject, 1f);
            //}
            UpdateHealthBar();
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
}
