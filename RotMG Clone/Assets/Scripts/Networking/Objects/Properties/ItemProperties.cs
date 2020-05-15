using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Constants.Pets;
using Utils;

namespace Networking.Objects.Properties
{
    
    /**
     * Properties of an Item (inventory)
     */
    public class ItemProperties : ObjectProperties
    {
        private const bool DISABLE_SOULBOUND_UT = false;

        public ItemProperties(XElement item) : base(item)
        {
            SetType = item.AttrDefault("setType", "0x0").ParseHex();
            Tier = item.HasOwnProperty("Tier") ? item.Element("Tier").Value.ParseInt() : -1;
            SlotType = (byte) item.ElemDefault("SlotType", "0").ParseInt();
            Description = item.ElemDefault("Description", "");
            RateOfFire = item.ElemDefault("RateOfFire", "1").ParseFloat();
            Usable = item.HasOwnProperty("Usable");
            BagType = (byte) item.ElemDefault("BagType", "0").ParseInt();
            MpCost = (byte) item.ElemDefault("MpCost", "0").ParseInt();
            FeedPower = (ushort) item.ElemDefault("feedPower", "0").ParseInt();
            FameBonus = (byte) item.ElemDefault("FameBonus", "0").ParseInt();
            NumProjectiles = item.ElemDefault("NumProjectiles", "0").ParseInt();
            ArcGap = item.ElemDefault("ArcGap", "11.25").ParseFloat();
            Consumable = item.HasOwnProperty("Consumable");
            Potion = item.HasOwnProperty("Potion");
            DisplayId = item.ElemDefault("DisplayId", "");
            Doses = item.ElemDefault("Doses", "0").ParseInt();
            SuccessorId = item.ElemDefault("SuccessorId", "");
            if (item.HasOwnProperty("Soulbound"))
            {
                int s = item.ElemDefault("SlotType", "0").ParseInt();
                if (s == 10 || s == 26 || item.HasOwnProperty("ActivateOnEquip") || !DISABLE_SOULBOUND_UT)
                    Soulbound = true;
                else Soulbound = false;
            }

            Secret = item.HasOwnProperty("Secret");
            IsBackpack = item.HasOwnProperty("Backpack");
            Cooldown = item.ElemDefault("Cooldown", "0").ParseFloat();
            Resurrects = item.HasOwnProperty("Resurrects");
            Texture1 = item.ElemDefault("Tex1", "0x0").ParseHex();
            Texture2 = item.ElemDefault("Tex2", "0x0").ParseHex();
            Class = item.ElemDefault("Class", "");

            if (item.HasOwnProperty("PetFamily"))
                if (item.Element("PetFamily").Value == "? ? ? ?")
                    Family = (Family) Enum.Parse(typeof(Family), "Unknown", true);
                else
                    Family = (Family) Enum.Parse(typeof(Family), item.Element("PetFamily").Value, true);
            else
                Family = null;

            if (item.HasOwnProperty("Rarity"))
                Rarity = (Rarity) Enum.Parse(typeof(Rarity), item.Element("Rarity").Value, true);
            else
                Rarity = null;

            StatsBoost =
                item.Elements("ActivateOnEquip")
                    .Select(
                        i =>
                            new KeyValuePair<int, int>(int.Parse(i.Attribute("stat").Value),
                                int.Parse(i.Attribute("amount").Value)))
                    .ToArray();

            ActivateEffects = item.Elements("Activate").Select(i => new ActivateEffect(i)).ToArray();
            Projectiles = item.Elements("Projectile").Select(i => new ProjectileProperties(i)).ToArray();
            MpEndCost = item.ElemDefault("MpEndCost", "0").ParseInt();
            Timer = item.ElemDefault("Timer", "0").ParseFloat();
            XpBooster = item.HasOwnProperty("XpBoost");
            LootDropBooster = item.HasOwnProperty("LDBoosted");
            LootTierBooster = item.HasOwnProperty("LTBoosted");
        }

        public int SlotType { get; }
        public ushort FeedPower { get; }
        public int Tier { get; }
        public string Description { get; }
        public float RateOfFire { get; }
        public bool Usable { get; }
        public int BagType { get; }
        public int MpCost { get; }
        public int FameBonus { get; }
        public int NumProjectiles { get; }
        public float ArcGap { get; }
        public bool Consumable { get; }
        public bool Potion { get; }
        public string SuccessorId { get; }
        public bool Soulbound { get; }
        public float Cooldown { get; }
        public bool Resurrects { get; }
        public int Texture1 { get; }
        public int Texture2 { get; }
        public bool Secret { get; }
        public bool IsBackpack { get; }
        public Rarity? Rarity { get; }
        public Family? Family { get; }

        public int Doses { get; set; }

        public KeyValuePair<int, int>[] StatsBoost { get; }
        public ActivateEffect[] ActivateEffects { get; }

        public int? MpEndCost { get; }
        public float? Timer { get; }
        public bool XpBooster { get; }
        public bool LootDropBooster { get; }
        public bool LootTierBooster { get; }
        public int SetType { get; }

        public static Dictionary<ushort, ItemProperties> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, ItemProperties>();

            doc.Element("Objects")
                .Elements("Object")
                .Where(elem => elem.HasOwnProperty("Item"))
                .ForEach(item =>
                {
                    ItemProperties i = new ItemProperties(item);
                    map[i.ObjectType] = i;
                });

            return map;
        }

        public override string ToString()
        {
            return string.Format("Item: {0} (0x{1:X})", ObjectId, ObjectType);
        }
    }
}