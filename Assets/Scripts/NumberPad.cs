using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberPad : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] private TextMeshProUGUI mCodeScreen;

    [Header("Buttons")] 
    [SerializeField] private PressButton mButton0;
    [SerializeField] private PressButton mButton1;
    [SerializeField] private PressButton mButton2;
    [SerializeField] private PressButton mButton3;
    [SerializeField] private PressButton mButton4;
    [SerializeField] private PressButton mButton5;
    [SerializeField] private PressButton mButton6;
    [SerializeField] private PressButton mButton7;
    [SerializeField] private PressButton mButton8;
    [SerializeField] private PressButton mButton9;

    [Header("Settings")]
    [SerializeField] private string mRightCode;
    [SerializeField] private GameObject mCardPrefab;
    [SerializeField] private Transform mCardSpawnPoint;
    
    [SerializeField] private AudioClip mRightSound;
    [SerializeField] private AudioClip mWrongSound;
    [SerializeField] private AudioSource mAudioSource;
    

    private string _keyCode = "";
    private int _countClicked = 0;
    private bool _isWorking = true; // Нумпад работает
    void Update()
    {
        // Сброс счетчика
        if (_countClicked >= 4)
        {
            if (_keyCode == mRightCode)
            {
                mCodeScreen.SetText("Right Code!");
                mCodeScreen.color = Color.green;
                
                Instantiate(mCardPrefab, mCardSpawnPoint.position, mCardSpawnPoint.rotation);
                
                mAudioSource.PlayOneShot(mRightSound);
                
                _isWorking = false; // Выключаем обработку нажатий
            }
            else
            {
                _keyCode = "";
                mCodeScreen.SetText("Wrong Code!");
                mCodeScreen.color = Color.red;
                mAudioSource.PlayOneShot(mWrongSound);
            }
            
            _countClicked = 0;
        }

        if (_isWorking)
        {
            // Проверка нажатия кнопок
            clickButton(mButton0);
            clickButton(mButton1);
            clickButton(mButton2);
            clickButton(mButton3);
            clickButton(mButton4);
            clickButton(mButton5);
            clickButton(mButton6);
            clickButton(mButton7);
            clickButton(mButton8);
            clickButton(mButton9);
        }
    }

    // Обработка нажатия кнопки
    private void clickButton(TouchButton button)
    {
        if (button.isClicked)
        {
            _keyCode = _keyCode + button.value.ToString();
            mCodeScreen.color = Color.black;
            mCodeScreen.SetText(_keyCode);
            
            _countClicked++;
            
            button.isClicked = false;
        }
    }
    
    private void clickButton(PressButton button)
    {
        if (button.isClicked)
        {
            _keyCode = _keyCode + button.value.ToString();
            mCodeScreen.color = Color.black;
            mCodeScreen.SetText(_keyCode);
            
            _countClicked++;
            
            button.isClicked = false;
        }
    }
}
