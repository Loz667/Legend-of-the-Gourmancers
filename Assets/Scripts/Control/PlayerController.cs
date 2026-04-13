using LotG.Battle;
using LotG.Events;
using LotG.QuestSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LotG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int moveSpeed;
        [SerializeField] private LayerMask grassLayer;
        [SerializeField] private int stepsInGrass;
        [SerializeField] private int minStepsToEncounter;
        [SerializeField] private int maxStepsToEncounter;
        [SerializeField] private ParticleSystem craftParticles;

        private Rigidbody rb;
        private Animator anim;
        private SpriteRenderer playerSprite;

        private PartyManager partyManager;
        private QuestManager questManager;

        private Vector3 moveVelocity;
        private float stepTimer;
        private bool movementDisabled;
        private bool movingInGrass;
        private int stepsToEncounter;

        private const string IS_WALKING_PARAM = "IsWalking";
        private const string BATTLE_SCENE = "Battle_Scene";
        private const float TIME_PER_STEP = 0.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
            playerSprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            GameEventsManager.instance.inputEvents.OnMovePressed += MovePressed;
            GameEventsManager.instance.miscEvents.onDisablePlayerMovement += DisablePlayerMovement;
            GameEventsManager.instance.miscEvents.onEnablePlayerMovement += EnablePlayerMovement;
            GameEventsManager.instance.miscEvents.OnRecipeCrafted += RecipeCrafted;

            partyManager = FindFirstObjectByType<PartyManager>();

            if (partyManager.GetPosition() != Vector3.zero)
            {
                transform.position = partyManager.GetPosition();
            }

            questManager = FindFirstObjectByType<QuestManager>();

            CalculateStepsToNextEncounter();
        }

        private void OnDestroy()
        {
            GameEventsManager.instance.inputEvents.OnMovePressed -= MovePressed;
            GameEventsManager.instance.miscEvents.onDisablePlayerMovement -= DisablePlayerMovement;
            GameEventsManager.instance.miscEvents.onEnablePlayerMovement -= EnablePlayerMovement;
            GameEventsManager.instance.miscEvents.OnRecipeCrafted -= RecipeCrafted;
        }

        private void EnablePlayerMovement()
        {
            movementDisabled = false;
        }

        private void DisablePlayerMovement()
        {
            movementDisabled = true;
            moveVelocity = Vector3.zero;
            anim.SetBool(IS_WALKING_PARAM, false);
        }

        private void MovePressed(Vector2 moveDir)
        {
            moveVelocity = new Vector3(moveDir.x, 0f, moveDir.y).normalized * moveSpeed;

            if (movementDisabled)
            {
                moveVelocity = Vector3.zero;
            }
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = moveVelocity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
            movingInGrass = colliders.Length != 0 && moveVelocity != Vector3.zero;

            if (movingInGrass)
            {
                stepTimer += Time.fixedDeltaTime;
                if (stepTimer > TIME_PER_STEP)
                {
                    stepsInGrass++;
                    stepTimer = 0;

                    if (stepsInGrass >= stepsToEncounter)
                    {
                        partyManager.SetPosition(transform.position);
                        questManager.SaveQuest();
                        SceneManager.LoadScene(BATTLE_SCENE);
                    }
                }
            }
        }

        private void UpdateAnimations()
        {
            anim.SetBool(IS_WALKING_PARAM, moveVelocity != Vector3.zero);

            if (moveVelocity.x != 0 && moveVelocity.x < 0)
            {
                playerSprite.flipX = true;
            }
            else if (moveVelocity.x != 0 && moveVelocity.x > 0)
            {
                playerSprite.flipX = false;
            }
        }

        private void CalculateStepsToNextEncounter()
        {
            stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
        }

        private void RecipeCrafted()
        {
            Instantiate(craftParticles, transform.position, Quaternion.identity);
        }
    }
}
