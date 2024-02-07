using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager
{
    private GameObject mouseClickParticlePrefab;
    private GameObject _mouseClickParticle;

    public void Init()
    {
        mouseClickParticlePrefab = Resources.Load<GameObject>("Particles/MouseClickParticle");
    }
    
    public void CreateMouseClickParticle(Vector3 pos)
    {
        // if (_mouseClickParticle) Destroy(_mouseClickParticle);
        //
        // _mouseClickParticle = Instantiate(mouseClickParticlePrefab, pos, mouseClickParticlePrefab.transform.rotation);
    }
}
