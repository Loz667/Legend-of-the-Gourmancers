using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingAnimation : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    
    private Animator _anim;
    
    private const string CUT_TRIGGER = "Cut";

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += PlayerCutObject;
    }

    private void PlayerCutObject(object sender, EventArgs e)
    {
        _anim.SetTrigger(CUT_TRIGGER);
    }
}
