using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Assets;
using Constants;
using Utils;
using Math = Utils.Math;

namespace Networking.Objects.Properties
{
    /**
    * The property of an object.
    */
    public class ObjectProperties
    {
        public string DisplayId { get; set; }

        public string Group { get; }
        public string Class { get; set; }

        public int ShadowSize { get; }

        public bool IsPlayer { get; }
        public bool IsEnemy { get; }
        public bool OccupySquare { get; }
        public bool FullOccupy { get; }
        public bool EnemyOccupySquare { get; }
        public bool Static { get; }
        public bool NoMiniMap { get; }
        public bool ProtectFromGroundDamage { get; }
        public bool ProtectFromSink { get; }
        public bool Flying { get; }
        public bool ShowName { get; }
        public bool DontFaceAttacks { get; }

        public float Z { get; set; }

        public bool BlocksSight { get; }
        public int MinSize { get; }
        public int MaxSize { get; }
        public int SizeStep { get; }
        public TagList Tags { get; }
        public ProjectileProperties[] Projectiles { get; set; }

        public int UnlockCost { get; }
        public int MaxHitPoints { get; }
        public int MaxMagicPoints { get; private set; }
        public int MaxAttack { get; }
        public int MaxDefense { get; }
        public int MaxSpeed { get; }
        public int MaxDexterity { get; }
        public int MaxHpRegen { get; }
        public int MaxMpRegen { get; }

        public double MaxHP { get; }
        public int Defense { get; }
        public string Terrain { get; }
        public float SpawnProbability { get; }
        public SpawnCount Spawn { get; }
        public bool Cube { get; }
        public bool God { get; }
        public bool Quest { get; }
        public int? Level { get; }
        public bool StasisImmune { get; }
        public bool StunImmune { get; }
        public bool ParalyzedImmune { get; }
        public bool DazedImmune { get; }
        public bool Oryx { get; }
        public bool Hero { get; }
        public int? PerRealmMax { get; }
        public float? ExpMultiplier { get; } //Exp gained = level total / 10 * multi

        public string OldSound { get; set; }

        public float AngleCorrection { get; set; }

        public float Rotation { get; set; }

        public float BloodProb { get; }

        public int BloodColor { get; }
        public int ShadowColor { get; }

        public Dictionary<int, string> Sounds = new Dictionary<int, string>();

        public ushort ObjectType { get; }
        public string ObjectId { get; }

        public ObjectProperties(XElement obj)
        {
            ObjectType = (ushort) obj.AttrDefault("type", "0x0").ParseHex();
            ObjectId = obj.AttrDefault("id", "");
            DisplayId = obj.ElemDefault("DisplayId", null);

            Class = obj.ElemDefault("Class", "GameObject");
            Group = obj.ElemDefault("Group", null);

            ShadowSize = obj.ElemDefault("ShadowSize", "0").ParseInt();

            IsPlayer = obj.HasOwnProperty("Player");
            IsEnemy = obj.HasOwnProperty("Enemy");

            MaxHP = (ushort) obj.ElemDefault("MaxHitPoints", "0").ParseHex();
            ExpMultiplier = obj.ElemDefault("XpMult", "0").ParseFloat();

            OccupySquare = obj.HasOwnProperty("OccupySquare");
            FullOccupy = obj.HasOwnProperty("FullOccupy");
            EnemyOccupySquare = obj.HasOwnProperty("EnemyOccupySquare");
            Static = obj.HasOwnProperty("Static");

            NoMiniMap = obj.Element("NoMiniMap") != null;
            ProtectFromGroundDamage = obj.Element("ProtectFromGroundDamage") != null;
            ProtectFromSink = obj.Element("ProtectFromSink") != null;
            Flying = obj.HasOwnProperty("Flying");
            ShowName = obj.Element("ShowName") != null;
            DontFaceAttacks = obj.Element("DontFaceAttacks") != null;

            Z = obj.ElemDefault("Z", "0").ParseFloat();

            BlocksSight = obj.HasOwnProperty("BlocksSight");

            if (obj.HasOwnProperty("Size"))
            {
                MinSize = MaxSize = obj.Element("Size").Value.ParseInt();
                SizeStep = 0;
            }
            else
            {
                MinSize = obj.ElemDefault("MinSize", "100").ParseInt();
                MaxSize = obj.ElemDefault("MaxSize", "100").ParseInt();
                SizeStep = obj.ElemDefault("SizeStep", "0").ParseInt();
            }

            Projectiles = obj.Elements("Projectile").Select(i => new ProjectileProperties(i)).ToArray();

            UnlockCost = obj.ElemDefault("UnlockCost", "0").ParseInt();

            if (obj.HasOwnProperty("MaxHitPoints"))
            {
                MaxHitPoints = obj.AttrDefault("max", "-1").ParseInt();
                MaxHP = obj.ElemDefault("MaxHitPoints", "0").ParseInt();
            }

            if (obj.HasOwnProperty("MaxMagicPoints")) MaxAttack = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("Attack")) MaxAttack = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("Dexterity")) MaxDexterity = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("Speed")) MaxSpeed = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("HpRegen")) MaxHpRegen = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("MpRegen")) MaxMpRegen = obj.AttrDefault("max", "-1").ParseInt();

            if (obj.HasOwnProperty("Defense"))
            {
                Defense = obj.ElemDefault("Defense", "0").ParseInt();
                MaxDefense = obj.AttrDefault("max", "-1").ParseInt();
            }

            Terrain = obj.ElemDefault("Terrain", "");
            SpawnProbability = obj.ElemDefault("SpawnProbability", "0").ParseFloat();
            SpawnProbability = obj.ElemDefault("SpawnProbability", "0").ParseFloat();

            if (obj.HasOwnProperty("Spawn")) Spawn = new SpawnCount(obj.Element("Spawn"));

            God = obj.Element("God") != null;
            Cube = obj.Element("Cube") != null;
            Quest = obj.Element("Quest") != null;

            Level = obj.ElemDefault("Level", "0").ParseInt();

            Tags = new TagList();

            if (obj.Elements("Tag").Any())
                foreach (XElement i
                    in obj.Elements("Tag"))
                    Tags.Add(new Tag(i));

            StasisImmune = obj.HasOwnProperty("StasisImmune");
            StunImmune = obj.HasOwnProperty("StunImmune");
            ParalyzedImmune = obj.HasOwnProperty("ParalyzedImmune");
            DazedImmune = obj.HasOwnProperty("DazedImmune");
            Oryx = obj.Element("Oryx") != null;
            Hero = obj.Element("Hero") != null;

            PerRealmMax = obj.ElemDefault("PerRealmMax", "0").ParseInt();

            ExpMultiplier = obj.ElemDefault("ExpMultiplier", "0").ParseFloat();

            OldSound = obj.ElemDefault("OldSound", "");

            AngleCorrection = obj.ElemDefault("AngleCorrection", "0").ParseFloat();

            Rotation = obj.ElemDefault("Rotation", "0").ParseFloat();

            BloodProb = obj.ElemDefault("BloodProb", "0").ParseFloat();

            BloodColor = obj.ElemDefault("BloodColor", "0").ParseHex();

            ShadowColor = obj.ElemDefault("ShadowColor", "0").ParseHex();

            if (obj.Elements("Sound").Any())
                foreach (XElement i in obj.Elements("Sound"))
                    Sounds[i.AttrDefault("id", "0").ParseInt()] = i.Value;
        }

        public static Dictionary<ushort, ObjectProperties> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, ObjectProperties>();

            doc.Element("Objects")
                .Elements("Object")
                .ForEach(obj =>
                {
                    ObjectProperties o = new ObjectProperties(obj);
                    map[o.ObjectType] = o;
                });

            return map;
        }

        public void LoadSounds()
        {
            if (this.Sounds == null)
            {
                return;
            }

            foreach (var sound in Sounds.Values)
            {
                SoundEffectLibrary.Load(sound);
            }
        }

        public int GetSize()
        {
            if (this.MinSize == this.MaxSize)
            {
                return this.MinSize;
            }

            int maxSteps = (this.MaxSize - this.MinSize) / this.SizeStep;
            return this.MinSize + (int) (Math.Random() * maxSteps) * this.SizeStep;
        }
    }

    public class TagList : List<Tag>
    {
        public bool ContainsTag(string name)
        {
            return this.Any(i => i.Name == name);
        }

        public string TagValue(string name, string value)
        {
            return
                (from i in this where i.Name == name where i.Values.ContainsKey(value) select i.Values[value])
                .FirstOrDefault();
        }
    }

    // Used by ObjectDesc TagList
    public class Tag
    {
        public Tag(XElement elem)
        {
            Name = elem.Attribute("name").Value;
            Values = new Dictionary<string, string>();
            foreach (XElement i in elem.Elements())
            {
                if (Values.ContainsKey(i.Name.ToString()))
                    Values.Remove(i.Name.ToString());
                Values.Add(i.Name.ToString(), i.Value);
            }
        }

        public string Name { get; }
        public Dictionary<string, string> Values { get; }
    }
}