using UnityEngine;
using System.Collections.Generic;
using Particle = UnityEngine.ParticleSystem.Particle;

namespace Brainamics.Core
{
    // Adopted from https://discussions.unity.com/t/access-to-the-particle-system-lifecycle-events/582469/5
    public class ParticleLifetimeEvents : MonoBehaviour
    {
        private readonly List<float> _aliveParticlesRemainingTime = new List<float>();
        private Particle[] _particles;
        private float _shortestTimeAlive = float.MaxValue;

        [SerializeField]
        private ParticleSystem _particleSystem;

        private event System.Action<Particle> _particleWasBorn;

        public event System.Action<Particle> ParticleWasBorn
        {
            add => _particleWasBorn += value;
            remove => _particleWasBorn -= value;
        }
        private void RaiseParticleWasBorn(Particle particle)
        {
            _particleWasBorn?.Invoke(particle);
        }

        private event System.Action _particleDied;
        public event System.Action ParticleDied
        {
            add => _particleDied += value;
            remove => _particleDied -= value;
        }
        private void RaiseParticleDied()
        {
            _particleDied?.Invoke();
        }

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

        private Particle[] GetYoungestParticles(int numPartAlive, Particle[] particles, ref float youngestParticleTimeAlive)
        {
            var youngestParticles = new List<Particle>();
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
                    youngestParticles.Add(particles[i]);
                }
            }
            return youngestParticles.ToArray();
        }

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
}