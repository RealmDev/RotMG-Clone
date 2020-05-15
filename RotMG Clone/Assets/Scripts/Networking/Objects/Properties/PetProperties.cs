using System;
using System.Xml.Linq;
using Constants.Pets;
using Utils;
using Ability = RotMG_Net_Lib.Ability;

namespace Networking.Objects.Properties
{
    public class PetProperties
    {
        public PetProperties(XElement elem)
        {
            ObjectId = elem.AttrDefault("id", "0");
            ObjectType = (ushort) elem.AttrDefault("type", "0x0").ParseHex();
            if (elem.ElemDefault("Family", "0") == "? ? ? ?")
                PetFamily = Family.Unknown;
            else
                PetFamily = (Family) Enum.Parse(typeof(Family), elem.ElemDefault("Family", "0"));
            PetRarity = (Rarity) Enum.Parse(typeof(Rarity), elem.ElemDefault("Rarity", "0"));
            if (elem.Element("FirstAbility") != null)
                FirstAbility = (Ability) Enum.Parse(typeof(Ability),
                    elem.ElemDefault("FirstAbility", "0").Replace(" ", string.Empty));
            DefaultSkin = elem.ElemDefault("DefaultSkin", "0");
            Size = int.Parse(elem.ElemDefault("Size", "0"));
            DisplayId = elem.ElemDefault("DisplayId", "0");
        }

        public string DisplayId { get; }
        public Family PetFamily { get; }
        public Rarity PetRarity { get; }
        public Ability FirstAbility { get; }
        public string DefaultSkin { get; }
        public int Size { get; }
        public string ObjectId { get; }
        public ushort ObjectType { get; }
    }

    public class PetSkin
    {
        public PetSkin(XElement elem)
        {
            ObjectId = elem.AttrDefault("id", "0");
            ObjectType = (ushort) elem.AttrDefault("type", "0x0").ParseHex();
            DisplayId = elem.ElemDefault("DisplayId", "0");
        }

        public string DisplayId { get; }
        public string ObjectId { get; }
        public ushort ObjectType { get; }
    }
}