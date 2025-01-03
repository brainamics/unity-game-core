using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Particle = UnityEngine.ParticleSystem.Particle;

namespace Brainamics.Core
{
    // Adopted from https://discussions.unity.com/t/access-to-the-particle-system-lifecycle-events/582469/5
    public class ParticleLifetimeEvents : MonoBehaviour
    {
        private readonly List<float> _aliveParticlesRemainingTime = new List<float>();
        private readonly List<Particle> _youngestParticles = new();
        
        private Particle[] _particles;
        private float _shortestTimeAlive = float.MaxValue;

        [SerializeField]
        private ParticleSystem _particleSystem;

        public UnityEvent<Particle> OnParticleBorn;
        public UnityEvent OnParticleDied;

        private void Awake()
        {
            _particles = new Particle[_particleSystem.main.maxParticles];
        }
        
        private void LateUpdate()
        {
            TryBroadcastParticleDeath();

            if (_particleSystem.particleCount == 0)
                return;

            var numParticlesAlive = _particleSystem.GetParticles(_particles);

            var youngestParticleTimeAlive = float.MaxValue;
            var youngestParticles = GetYoungestParticles(numParticlesAlive, _particles, ref youngestParticleTimeAlive);
            if (_shortestTimeAlive > youngestParticleTimeAlive)
            {
                for (var i = 0; i < youngestParticles.Length; i++)
                {
                    RaiseParticleWasBorn(youngestParticles[i]);
                    _aliveParticlesRemainingTime.Add(youngestParticles[i].remainingLifetime);
                }
            }
            _shortestTimeAlive = youngestParticleTimeAlive;
        }
        
        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void TryBroadcastParticleDeath()
        {
            for (var i = _aliveParticlesRemainingTime.Count - 1; i > -1; i--)
            {
                _aliveParticlesRemainingTime[i] -= Time.deltaTime;
                if (_aliveParticlesRemainingTime[i] > 0)
                    continue;
                _aliveParticlesRemainingTime.RemoveAt(i);
                RaiseParticleDied();
            }
        }

        private List<Particle> GetYoungestParticles(int numPartAlive, Particle[] particles, ref float youngestParticleTimeAlive)
        {
            _youngestParticles.Clear();
            for (var i = 0; i < numPartAlive; i++)
            {
                var timeAlive = particles[i].startLifetime - particles[i].remainingLifetime;
                if (timeAlive < youngestParticleTimeAlive)
                {
                    youngestParticleTimeAlive = timeAlive;
                }
            }
            for (var i = 0; i < numPartAlive; i++)
            {
                var timeAlive = particles[i].startLifetime - particles[i].remainingLifetime;
                if (Mathf.Approximately(timeAlive, youngestParticleTimeAlive))
                {
                    _youngestParticles.Add(particles[i]);
                }
            }
            return _youngestParticles;
        }
        
        private void RaiseParticleWasBorn(Particle particle)
        {
            OnParticleBorn.Invoke(particle);
        }

        private void RaiseParticleDied()
        {
            OnParticleDied.Invoke();
        }
    }
}
