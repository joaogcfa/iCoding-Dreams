using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioControle : MonoBehaviour {
    [SerializeField] private AudioClip tickSource;
    [SerializeField] private AudioClip tockSource;
    [SerializeField] private AudioClip alarmSource;

    [SerializeField] private float timeBetweenTicksDelta = 0.0001f;
    [SerializeField] private float timeBetweenTicksMin = 0.2f;
    [SerializeField] private float timeBetweenTicksMax = 1.5f;
    [SerializeField] private int quantTicksToInvoke = 2;

    [SerializeField] private UnityEvent onTick;
    [SerializeField] private UnityEvent onNTicks;
    [SerializeField] private UnityEvent onAlarm;

    private float timeSinceLastTick = 0f;
    private int ticks = 0;

    private float currentTimeBetweenTicks;

    void Start() {
        currentTimeBetweenTicks = timeBetweenTicksMax;
    }


    void Update() {
        timeSinceLastTick += Time.deltaTime;

        if (timeSinceLastTick >= currentTimeBetweenTicks) {
            timeSinceLastTick = 0f;
            currentTimeBetweenTicks -= timeBetweenTicksDelta;
            if (currentTimeBetweenTicks > timeBetweenTicksMin) {
                AudioSource.PlayClipAtPoint(tickSource, transform.position);
                onTick.Invoke();
                ticks++;
                if (ticks >= quantTicksToInvoke) {
                    ticks = 0;
                    onNTicks.Invoke();
                }
            } else {
                AudioSource.PlayClipAtPoint(alarmSource, transform.position);
                onAlarm.Invoke();
            }
        }
    }
}