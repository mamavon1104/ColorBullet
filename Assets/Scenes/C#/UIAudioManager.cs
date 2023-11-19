using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField] AudioClip selectAudio;
    [SerializeField] AudioClip decisionAudio;
    [SerializeField] AudioClip cantAudio;
    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SelectAudio()
    {
        audioSource.PlayOneShot(selectAudio);
    }

    public void DecisionAudio()
    {
        audioSource.PlayOneShot(decisionAudio);
    }
    public void CantAudio()
    {
        audioSource.PlayOneShot(cantAudio);
    }
}
