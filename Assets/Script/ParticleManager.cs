using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem shootParticle;
    public ParticleSystem WarpWeaponParticle;
    public ParticleSystem slashSwordParticle;

    // Start is called before the first frame update
    void Start()
    {    
    }
    // Update is called once per frame
    void Update()
    {
    }

    //パーティクルの再生
    public void PlayParticle(ParticleSystem particle) {

        particle.Play();
    }

}
