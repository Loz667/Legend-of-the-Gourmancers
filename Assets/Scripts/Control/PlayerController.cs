using LotG.Battle;
using LotG.Events;
using LotG.QuestSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace LotG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int moveSpeed;
        [SerializeField] private LayerMask grassLayer;
        [SerializeField] private int stepsInGrass;
        [SerializeField] private int minStepsToEncounter;
        [SerializeField] private int maxStepsToEncounter;

        private PlayerControls controls;
        private Rigidbody rb;
        private Animator anim;
        private SpriteRenderer playerSprite;

        private PartyManager partyManager;
        private QuestManager questManager;

        private Vector3 movement;
        private float stepTimer;
        private bool movingInGrass;
        private int stepsToEncounter;

        private const string IS_WALKING_PARAM = "IsWalking";
        private const string BATTLE_SCENE = "Battle_Scene";
        private const float TIME_PER_STEP = 0.5f;

        private void Awake()
        {
            controls = new PlayerControls();
            rb = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
            playerSprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Start()
        {
            GameEventsManager.instance.miscEvents.onDisablePlayerMovement += DisablePlayerMovement;
            GameEventsManager.instance.miscEvents.onEnablePlayerMovement += EnablePlayerMovement;

            partyManager = FindFirstObjectByType<PartyManager>();

            if (partyManager.GetPosition() != Vector3.zero)
            {
                transform.position = partyManager.GetPosition();
            }

            questManager = FindFirstObjectByType<QuestManager>();

            CalculateStepsToNextEncounter();
        }

        private void EnablePlayerMovement()
        {
            controls.Enable();
        }

        private void DisablePlayerMovement()
        {
            controls.Disable();
            anim.SetBool(IS_WALKING_PARAM, false);
        }

        private void Update()
        {
            float moveX = controls.Player.Move.ReadValue<Vector2>().x;
            float moveZ = controls.Player.Move.ReadValue<Vector2>().y;

            movement = new Vector3(moveX, 0, moveZ).normalized;

            anim.SetBool(IS_WALKING_PARAM, movement != Vector3.zero);

            if (moveX != 0 && moveX < 0)
            {
                playerSprite.flipX = true;
            }
            else if (moveX != 0 && moveX > 0)
            {
                playerSprite.flipX = false;
            }
        }

        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);

            Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
            movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

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

        private void OnDestroy()
        {
            GameEventsManager.instance.miscEvents.onDisablePlayerMovement -= DisablePlayerMovement;
            GameEventsManager.instance.miscEvents.onEnablePlayerMovement -= EnablePlayerMovement;
        }

        private void CalculateStepsToNextEncounter()
        {
            stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);

        }
    }
}
