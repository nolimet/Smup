using UnityEngine;

namespace Util
{
    /// <summary>
    /// Struct for particle sytems so i don't have to repeat much code
    /// </summary>
    [System.Serializable]
    public struct ParticleSystemStruct
    {
        public bool enabled;
        public ParticleSystem System;
        public ParticleSystem.Particle[] Particles;
        public ParticleSystem.EmissionModule Emission;
        public int maxParticles;

        public void setup()
        {
            maxParticles = System.main.maxParticles;
            if (Particles == null || Particles.Length < maxParticles)
                Particles = new ParticleSystem.Particle[maxParticles];
        }
    }
}