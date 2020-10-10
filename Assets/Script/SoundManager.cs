using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip warpWeaponSound;
    public AudioClip slashSwordSound;
    public AudioClip noneBulletSound;


    // Start is called before the first frame update
    void Start()
    {   
    }
    // Update is called once per frame
    void Update()
    {

    }

    //サウンドの再生
    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
