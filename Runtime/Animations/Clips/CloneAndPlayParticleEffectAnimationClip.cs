using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Particle System/Clone and Play")]
    public class CloneAndPlayParticleEffectAnimationClip : AnimationClipBase
    {
        private LevelObject _levelObject;
    
        public ParticleSystem ParticleSystem;
        public bool ReparentToLevel = true;
        public bool PlayParticleSystem;
    
        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var scanner = GameSceneScanner.LocateByActiveScene();
            var levelScanner = scanner.Level;
    
            var parent = ParticleSystem.transform.parent;
            if (ReparentToLevel)
                parent = levelScanner.LevelObject.transform;
            var orgTransform = ParticleSystem.transform;
            var particleSystem = Object.Instantiate(ParticleSystem, orgTransform.position, orgTransform.rotation, parent);
            particleSystem.gameObject.SetActive(true);
    
            if (PlayParticleSystem)
                particleSystem.Play();
            yield break;
        }
    }
}
