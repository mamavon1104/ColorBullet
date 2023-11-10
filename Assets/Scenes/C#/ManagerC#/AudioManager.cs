using System.Collections;
using System.Collections.Generic;
using Unity.XR.GoogleVr;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip bounceAudio;
    [SerializeField] AudioClip restorationAudio;
    [SerializeField] AudioClip dashAudio;
    [SerializeField] AudioClip shotAudio;
    [SerializeField] AudioClip hitPlayerAudio;
    [SerializeField] AudioClip hitBulletAudio;
    [SerializeField] AudioClip teleporterAudio;
    [SerializeField] AudioClip countDownAudio_012;
    [SerializeField] AudioClip countDownAudio_Go;
    [SerializeField] AudioClip checkAudio;
    [SerializeField] AudioClip checkAllAudio;

    [SerializeField] AudioClip finishAudio;
    [SerializeField] AudioClip[] finishAudioRank;

    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }
    private void Start()
    {
        GameMaster.audioManagerMaster = this;
    }
    public void BounceAudio()
    {
        audioSource.PlayOneShot(bounceAudio);
    }
    public void RestorationAudio()
    {
        audioSource.PlayOneShot(restorationAudio);
    }
    public void DashAudio()
    {
        audioSource.PlayOneShot(dashAudio);
    }
    public void ShotAudio()
    {
        audioSource.PlayOneShot(shotAudio);
    }
    public void HitPlayerAudio()
    {
        audioSource.PlayOneShot(hitPlayerAudio);
    }
    public void ChangeOtherBulletAudio()
    {
        audioSource.PlayOneShot(hitBulletAudio);
    }
    public void TeleporterAudio()
    {
        audioSource.PlayOneShot(teleporterAudio);
    }
    public void CountDownAudio(int i)
    {
        if (i < 3) //3,2,1(0,1,2”Ô†)‚ÌŽž‚ÍAudio012
        {
            audioSource.PlayOneShot(countDownAudio_012);
        }
        else audioSource.PlayOneShot(countDownAudio_Go);
    }
    public void CheckAudio()
    {
        audioSource.PlayOneShot(checkAudio);
    }
    public void CheckAllAudio()
    {
        audioSource.PlayOneShot(checkAllAudio);
    }
    public void FinishAudio()
    {
        audioSource.PlayOneShot(finishAudio);
    }
    public void FinishAudioRank(int rank)
    {
        audioSource.PlayOneShot(finishAudioRank[rank]);
    }
}
