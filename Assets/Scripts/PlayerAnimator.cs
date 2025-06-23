using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    [SerializeField] private PlayerController player;
    private Animator _anim;

    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _anim.SetBool(IS_WALKING, player.IsWalking());
    }
}
