using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Constants;
using JetBrains.Annotations;
using Utils;

namespace Networking.Objects.Properties
{
    public class ProjectileProperties
    {
        public byte BulletType { get; }
        public string ObjectId { get; }

        public float Lifetime { get; }
        public float Speed { get; }
        public int Size { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }

        public List<ConditionEffect> Effects { get; }

        public bool MultiHit { get; }
        public bool PassesCover { get; }
        public bool ArmorPiercing { get; }
        public bool ParticleTrail { get; }
        public bool Wavy { get; }
        public bool Parametric { get; }
        public bool Boomerang { get; }
        public float Amplitude { get; }
        public float Frequency { get; }
        public float Magnitude { get; }

        public Dictionary<ConditionEffect, bool> IsPetEffect { get; }

        public bool FaceDir { get; }
        public bool NoRotation { get; }

        public ProjectileProperties(XElement projectile)
        {
            BulletType = (byte) projectile.AttrDefault("id", "0").ParseInt();
            ObjectId = projectile.ElemDefault("ObjectId", "");

            Lifetime = projectile.ElemDefault("LifetimeMS", "0").ParseFloat();
            Speed = projectile.ElemDefault("Speed", "0").ParseFloat();
            Size = projectile.ElemDefault("Size", "0").ParseInt();


            if (projectile.HasOwnProperty("Damage"))
            {
                MinDamage = projectile.ElemDefault("Damage", "0").ParseInt();
            }
            else
            {
                MinDamage = projectile.ElemDefault("MinDamage", "0").ParseInt();
                MaxDamage = projectile.ElemDefault("MaxDamage", "0").ParseInt();
            }


            foreach (var condEffectXml in projectile.Elements("ConditionEffect"))
            {
                if (Effects == null)
                {
                    Effects = new List<ConditionEffect>();
                }

                Effects.Add(new ConditionEffect(condEffectXml));

                if (condEffectXml.AttrDefault("target", "0)") == "1")
                {
                    if (IsPetEffect == null)
                    {
                        IsPetEffect = new Dictionary<ConditionEffect, bool>();
                    }

                    IsPetEffect[new ConditionEffect(condEffectXml)] = true;
                }
            }

            MultiHit = projectile.HasOwnProperty("MultiHit");
            PassesCover = projectile.HasOwnProperty("PassesCover");
            ArmorPiercing = projectile.HasOwnProperty("ArmorPiercing");
            ParticleTrail = projectile.HasOwnProperty("ParticleTrail");

            // TODO Particle Trail

            Wavy = projectile.HasOwnProperty("Wavy");
            Parametric = projectile.HasOwnProperty("Parametric");
            Boomerang = projectile.HasOwnProperty("Boomerang");
            Amplitude = projectile.ElemDefault("Amplitude", "0").ParseFloat();
            Frequency = projectile.ElemDefault("Frequency", "0").ParseFloat();
            Magnitude = projectile.ElemDefault("Magnitude", "0").ParseFloat();
            FaceDir = projectile.HasOwnProperty("FaceDir");
            NoRotation = projectile.HasOwnProperty("NoRotation");
        }
    }
}