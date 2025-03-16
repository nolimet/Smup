using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    /// <summary>
    /// Struct for particle sytems so i don't have to repeat much code
    /// </summary>
    [Serializable]
    public struct ParticleSystemStruct
    {
        public bool enabled;
        [FormerlySerializedAs("System")] public ParticleSystem system;
        public ParticleSystem.Particle[] Particles;
        public ParticleSystem.EmissionModule Emission;
        public int maxParticles;

        public void Setup()
        {
            maxParticles = system.main.maxParticles;
            if (Particles == null || Particles.Length < maxParticles)
                Particles = new ParticleSystem.Particle[maxParticles];
        }
    }
}
