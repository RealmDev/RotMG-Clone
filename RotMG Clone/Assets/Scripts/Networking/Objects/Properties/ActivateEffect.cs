using System;
using System.Xml.Linq;
using Constants;
using Utils;

namespace Networking.Objects.Properties
{
    public class ActivateEffect
    {
        public string ObjectId { get; }

        public ActivateEffects Effect { get; }
        public int Stats { get; }
        public int Amount { get; }
        public float Range { get; }
        public float DurationSec { get; }
        public int DurationMS { get; }
        public int DurationMS2 { get; }
        public ConditionEffectIndex? ConditionEffect { get; }
        public float EffectDuration { get; }
        public int MaximumDistance { get; }
        public float Radius { get; }
        public int TotalDamage { get; }

        public int AngleOffset { get; }
        public int MaxTargets { get; }
        public string Id { get; }
        public int SkinType { get; }
        public string DungeonName { get; }
        public string LockedName { get; }
        public string Target { get; }
        public string Center { get; }
        public bool UseWisMod { get; }
        public float VisualEffect { get; }
        public uint? Color { get; }

        public ActivateEffect(XElement elem)
        {
            Effect = (ActivateEffects) Enum.Parse(typeof(ActivateEffects), elem.Value);

            Stats = elem.AttrDefault("stat", "0").ParseInt();

            Amount = elem.AttrDefault("amount", "0").ParseInt();

            Range = elem.AttrDefault("range", "0").ParseFloat();

            if (elem.Attribute("duration") != null)
            {
                DurationSec = elem.AttrDefault("duration", "0").ParseFloat();
                DurationMS = (int) (DurationSec * 1000);
            }

            DurationMS2 = elem.AttrDefault("duration2", "0").ParseInt();

            ConditionEffect =
                (ConditionEffectIndex) Enum.Parse(typeof(ConditionEffectIndex),
                    elem.AttrDefault("effect", "0"));

            ConditionEffect =
                (ConditionEffectIndex) Enum.Parse(typeof(ConditionEffectIndex), elem.AttrDefault("condEffect", "0"));

            EffectDuration = elem.AttrDefault("condDuration", "0").ParseFloat();

            MaximumDistance = elem.AttrDefault("maxDistance", "0").ParseInt();

            Radius = elem.AttrDefault("radius", "0").ParseFloat();

            TotalDamage = elem.AttrDefault("totalDamage", "0").ParseInt();

            ObjectId = elem.AttrDefault("objectId", "0");

            AngleOffset = elem.AttrDefault("angleOffset", "0").ParseInt();

            MaxTargets = elem.AttrDefault("maxTargets", "0").ParseInt();

            Id = elem.AttrDefault("id", "0");

            DungeonName = elem.AttrDefault("dungeonName", "0");

            SkinType = elem.AttrDefault("skinType", "0").ParseInt();

            LockedName = elem.AttrDefault("lockedName", "0");

            Color = (uint?) elem.AttrDefault("color", "0x0").ParseHex();

            Target = elem.AttrDefault("target", "0");
            UseWisMod = elem.Attribute("useWisMod") != null;

            VisualEffect = elem.AttrDefault("visualEffect", "0").ParseFloat();

            Center = elem.AttrDefault("center", "0");
        }
    }
}