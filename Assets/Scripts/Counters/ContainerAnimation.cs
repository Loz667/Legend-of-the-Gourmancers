using System;
using UnityEngine;

public class ContainerAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    
    private Animator _anim;
    
    private const string OPEN_CLOSE_TRIGGER = "OpenClose";

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += PlayerGrabbedObject;
    }

    private void PlayerGrabbedObject(object sender, EventArgs e)
    {
        _anim.SetTrigger(OPEN_CLOSE_TRIGGER);
    }
}
