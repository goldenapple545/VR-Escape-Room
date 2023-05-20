using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PressButton : XRButton
{
    [Header("TouchButton Data")]
    [SerializeField] private Material mHoverMaterial;

    [SerializeField] public int value;

    private Material _objectMaterial;
    public bool isClicked = false;

    protected override void Awake()
    {
        base.Awake();

        _objectMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        gameObject.GetComponent<MeshRenderer>().material = mHoverMaterial;
    }

    public void Click()
    {
        isClicked = true;
        gameObject.GetComponent<AudioSource>().Play();
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        
        gameObject.GetComponent<MeshRenderer>().material = _objectMaterial;
    }
}
