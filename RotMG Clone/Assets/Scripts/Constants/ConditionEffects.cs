using System;
using System.Xml.Linq;
using Utils;

namespace Constants
{
    [Flags]
    public enum ConditionEffects : ulong
    {
        Dead = 1 << 0,
        Quiet = 1 << 1,
        Weak = 1 << 2,
        Slowed = 1 << 3,
        Sick = 1 << 4,
        Dazed = 1 << 5,
        Stunned = 1 << 6,
        Blind = 1 << 7,
        Hallucinating = 1 << 8,
        Drunk = 1 << 9,
        Confused = 1 << 10,
        StunImmune = 1 << 11,
        Invisible = 1 << 12,
        Paralyzed = 1 << 13,
        Speedy = 1 << 14,
        Bleeding = 1 << 15,
        ArmorBreakImmune = 1 << 16,
        Healing = 1 << 17,
        Damaging = 1 << 18,
        Berserk = 1 << 19,
        Paused = 1 << 20,
        Stasis = 1 << 21,
        StasisImmune = 1 << 22,
        Invincible = 1 << 23,
        Invulnerable = 1 << 24,
        Armored = 1 << 25,
        ArmorBroken = 1 << 26,
        Hexed = 1 << 27,
        NinjaSpeedy = 1 << 28,
        Unstable = 1 << 29,
        Darkness = 1 << 30,
        SlowedImmune = (ulong) 1 << 31,
        DazedImmune = (ulong) 1 << 32,
        ParalyzeImmune = (ulong) 1 << 33,
        Petrify = (ulong) 1 << 34,
        PetrifyImmune = (ulong) 1 << 35,
        PetDisable = (ulong) 1 << 36,
        Curse = (ulong) 1 << 37,
        CurseImmune = (ulong) 1 << 38,
        HPBoost = (ulong) 1 << 39,
        MPBoost = (ulong) 1 << 40,
        AttBoost = (ulong) 1 << 41,
        DefBoost = (ulong) 1 << 42,
        SpdBoost = (ulong) 1 << 43,
        VitBoost = (ulong) 1 << 44,
        WisBoost = (ulong) 1 << 45,
        DexBoost = (ulong) 1 << 46
    }

    // Note : In the original client, these are +1 and then decremented later.
    public enum ConditionEffectIndex
    {
        Dead = 0,
        Quiet = 1,
        Weak = 2,
        Slowed = 3,
        Sick = 4,
        Dazed = 5,
        Stunned = 6,
        Blind = 7,
        Hallucinating = 8,
        Drunk = 9,
        Confused = 10,
        StunImmune = 11,
        Invisible = 12,
        Paralyzed = 13,
        Speedy = 14,
        Bleeding = 15,
        ArmorBreakImmune = 16,
        Healing = 17,
        Damaging = 18,
        Berserk = 19,
        Paused = 20,
        Stasis = 21,
        StasisImmune = 22,
        Invincible = 23,
        Invulnerable = 24,
        Armored = 25,
        ArmorBroken = 26,
        Hexed = 27,
        NinjaSpeedy = 28,
        Unstable = 29,
        Darkness = 30,
        SlowedImmune = 31,
        DazedImmune = 32,
        ParalyzeImmune = 33,
        Petrified = 34,
        PetrifiedImmune = 35,
        PetDisable = 36,
        Curse = 37,
        CurseImmune = 38,
        HPBoost = 39,
        MPBoost = 40,
        AttBoost = 41,
        DefBoost = 42,
        SpdBoost = 43,
        VitBoost = 44,
        WisBoost = 45,
        DexBoost = 46,
        Silenced = 47,
        Exposed = 49
    }
    
    
    public class ConditionEffect
    {
        public const int CE_FIRST_BATCH = 0;
        public const int CE_SECOND_BATCH = 1;

        public ConditionEffect()
        {
        }

        public ConditionEffect(XElement elem)
        {
            Effect = (ConditionEffectIndex) Enum.Parse(typeof(ConditionEffectIndex), elem.Value.Replace(" ", ""));
            DurationMS = (int) (elem.AttrDefault("duration", "0").ParseFloat() * 1000);
            Range = elem.AttrDefault("range", "0").ParseFloat();
            Target = elem.AttrDefault("target", "0").ParseInt();
        }

        public ConditionEffectIndex Effect { get; set; }
        public int DurationMS { get; set; }
        public int Target { get; set; }
        public float Range { get; set; }
    }
}