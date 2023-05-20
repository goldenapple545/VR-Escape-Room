using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DoorHandle : XRGrabInteractable
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform doorHandle;
    [SerializeField] private Transform minPosition;
    [SerializeField] private Transform maxPosition;
    [SerializeField] private AudioSource mAudioSource;
    
    public float doorMass = 2;

    // Ручка возвращается на место при отпускании 
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        transform.position = doorHandle.position;
        transform.rotation = doorHandle.rotation;
        mAudioSource.Pause();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        mAudioSource.Play();
    }

    void Update()
    {
        // Проекция вектора ручки на направление начальной точки
        Vector3 projection = Vector3.Project(transform.position, -door.right);
        // Смещение проекции 
        projection += new Vector3(door.position.x, 0, -door.position.x + 0.1f);

        // Считаем насколько быстро нужно двигать дверь за ручкой
        Vector3 dist = transform.localPosition - door.localPosition;
        float volume = Vector3.Dot(dist, transform.position);
        float speed = Math.Abs(volume) * Time.deltaTime;
        
        mAudioSource.volume = speed*100;

        // Ограничения движения двери
        if (transform.position.z > minPosition.position.z)
        {
            door.position = Vector3.MoveTowards(door.position, minPosition.position + new Vector3(0, 0, -door.position.x + 0.1f), speed/doorMass);
            mAudioSource.volume = 0;
        }
        else if (transform.position.z < maxPosition.position.z)
        {
            door.position = Vector3.MoveTowards(door.position, maxPosition.position + new Vector3(0, 0, -door.position.x + 0.1f), speed/doorMass);
            mAudioSource.volume = 0;
        }
        else
        {
            door.position = Vector3.MoveTowards(door.position, projection, speed/doorMass);
        }

    }

}
