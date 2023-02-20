using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public class FadeParticleRenderer : FadeObjectBase
    {
        #region Variables

        //
        public ParticleSystem MainRender { get; set; }
        public ParticleSystem.Particle[] Particles { get; set; }

        #endregion

        #region Variables

        //
        public override void GetRenderObject()
        {
            MainRender = GetComponent<ParticleSystem>();
            StartColor = MainRender.main.startColor.color;
            StartAlphaSelf = StartColor.a;
            var maxParticle = GetMaxParticle();
            Particles = maxParticle > 0 ? new ParticleSystem.Particle[maxParticle] : null;
        }
        
        //
        public override void UpdateAlpha(float value)
        {
            var mainModule = MainRender.main;
            var newColor = new Color(StartColor.r, StartColor.g, StartColor.b, Mathf.Lerp(0, StartAlphaSelf, value));
            if (Particles != null)
            {
                var particleCount = MainRender.GetParticles(Particles);
                if (particleCount > 0)
                {
                    for (var index = 0; index < Particles.Length; index++)
                    {
                        Particles[index].startColor = newColor;
                    }
                }
                MainRender.SetParticles(Particles, particleCount);
            }
            mainModule.startColor = newColor;
        }

        //
        private int GetMaxParticle()
        {
            int maxParticles;
            if (MainRender.main.loop)
            {
                maxParticles = MainRender.main.maxParticles;
            }
            else
            {
                var emission = MainRender.emission;
                float maxRate = 0;
                switch (emission.rateOverTime.mode)
                {
                    case ParticleSystemCurveMode.Constant:
                        maxRate = emission.rateOverTime.constant;
                        break;
                    case ParticleSystemCurveMode.TwoConstants:
                        maxRate = emission.rateOverTime.constantMax;
                        break;
                }

                maxParticles = Mathf.CeilToInt(maxRate * MainRender.main.duration);
            }

            return maxParticles;
        }

        #endregion
    }
}