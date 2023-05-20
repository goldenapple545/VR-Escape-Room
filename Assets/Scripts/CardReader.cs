using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    // [SerializeField] private Collider mCollider1;
    // [SerializeField] private Collider mCollider2;
    // [SerializeField] private Collider mCollider3;
    
    [Header("CardReader Data")]
    [SerializeField] private GameObject mHandleToEnable;
    [SerializeField] private GameObject mDoorLock;
    [Header("Switch Light")]
    [SerializeField] private MeshRenderer mReaderLight;
    [SerializeField] private Material scanMaterial;
    [Header("Audio")] 
    [SerializeField] private AudioSource mAudioSource;
    [SerializeField] private AudioSource mPadlockAudioSource;
    [SerializeField] private AudioClip mWrongAudio;
    [SerializeField] private AudioClip mRightAudio;


    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Transform _cardTransform;
    private float _allowedUprightErrorRange = 0.3f; // Погрешность
    private bool _swipIsValid;
    
    private Material _defaultMaterial;

    private void Update()
    {
        if (_cardTransform != null)
        {
            // _cardTransform.up здесь мы получаем зеленый вектор дочернего компонента AttachPoint , а не самого объекта как ожидалось
            Vector3 keycardUp = _cardTransform.up; 
            Vector3 keycardForward = _cardTransform.forward; // Синий вектор AttachPoint
            float dotUp = Vector3.Dot(keycardUp, Vector3.up); // Vector3.up глобальный зеленый вектор
            float dotForward = Vector3.Dot(keycardForward, Vector3.right);

            if (dotUp < 1 - _allowedUprightErrorRange || dotForward < 1 - _allowedUprightErrorRange) // Если отличия по двум осям больше погрешности, то ... 
            {
                _swipIsValid = false;
                mAudioSource.PlayOneShot(mWrongAudio);
            }
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        base.CanSelect(interactable);
        return false;
    }

    protected void OnTriggerEnter(Collider other)
    {

        _defaultMaterial = mReaderLight.material;
        mReaderLight.material = scanMaterial;

        _cardTransform = other.transform;
        _startPoint = _cardTransform.position;
        _swipIsValid = true;
    }

    protected void OnTriggerExit(Collider other)
    {
        mReaderLight.material = _defaultMaterial;

        _endPoint = other.transform.position - _startPoint; // Измеряем пройденное расстояние в Trigger

        if (_swipIsValid && _endPoint.y < -0.20f) // Если оси совпадают и расстояние больше заданного, от отпираем дверь
        {
            mDoorLock.SetActive(false);
            mHandleToEnable.SetActive(true);
            mAudioSource.PlayOneShot(mRightAudio);
            mPadlockAudioSource.Play();
        }
        
        _cardTransform = null;
    }
    
    
}
