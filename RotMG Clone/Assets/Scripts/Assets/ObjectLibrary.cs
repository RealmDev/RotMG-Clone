using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Networking.Objects;
using RotMG_Net_Lib;
using RotMG_Net_Lib.Models;
using UnityEngine;
using Utils;
using GameObject = Networking.Objects.GameObject;

namespace Assets
{
    public class ObjectLibrary
    {
        public static Dictionary<int, XElement> XmlLibrary = new Dictionary<int, XElement>();

        public static Dictionary<int, XElement> PetXMLDataLibrary = new Dictionary<int, XElement>();

        public static Dictionary<string, Type> TYPE_MAP = new Dictionary<string, Type>()
        {
            // {"ArenaGuard", typeof(ArenaGuard)},
            // {"ArenaPortal", typeof(ArenaPortal)},
            // {"CaveWall", typeof(CaveWall)},
            // {"Character", typeof(Character)},
            // {"CharacterChanger", typeof(CharacterChanger)},
            // {"ClosedGiftChest", typeof(ClosedGiftChest)},
            // {"ClosedVaultChest", typeof(ClosedVaultChest)},
            // {"ConnectedWall", typeof(ConnectedWall)},
            {"Container", typeof(Container)},
            // {"DoubleWall", typeof(DoubleWall)},
            // {"FortuneGround", typeof(FortuneGround)},
            // {"FortuneTeller", typeof(FortuneTeller)},
            {"GameObject", typeof(GameObject)},
            // {"GuildBoard", typeof(GuildBoard)},
            // {"GuildChronicle", typeof(GuildChronicle)},
            // {"GuildHallPortal", typeof(GuildHallPortal)},
            // {"GuildMerchant", typeof(GuildMerchant)},
            // {"GuildRegister", typeof(GuildRegister)},
            // {"Merchant", typeof(Merchant)},
            // {"MoneyChanger", typeof(MoneyChanger)},
            // {"MysteryBoxGround", typeof(MysteryBoxGround)},
            // {"NameChanger", typeof(NameChanger)},
            // {"ReskinVendor", typeof(ReskinVendor)},
            // {"OneWayContainer", typeof(OneWayContainer)},
            {"Player", typeof(Player)},
            // {"Portal",typeof(Portal)},
            {"Projectile", typeof(Projectile)},
            // {"QuestRewards",typeof(QuestRewards)},
            // {"DailyLoginRewards", typeof(DailyLoginRewards)},
            // {"Sign", typeof(Sign)},
            // {"SpiderWeb", typeof(SpiderWeb)},
            // {"Stalagmite", typeof(Stalagmite)},
            // {"Wall", typeof(Wall)},
            {"Pet", typeof(Pet)},
            // {"PetUpgrader", typeof(PetUpgrader)},
            // {"YardUpgrader", typeof(YardUpgrader)}
        };

        private static List<XElement> HexTransforms;

        public static string GetIdFromType(int type)
        {
            //TODO
            return "";
        }

        public ObjectProperties GetPropsFromId(string id)
        {
            //TODO
            return null;
        }

        public static void ParseFromXML(XElement xml, Action callback)
        {
            foreach (var objectXML in xml.Elements("Object"))
            {
                string id = objectXML.AttrDefault("id", "");

                string displayId = id;

                if (objectXML.HasOwnProperty("DisplayId"))
                {
                    displayId = objectXML.ElemDefault("DisplayId", "");
                }

                if (objectXML.HasOwnProperty("Group"))
                {
                    if (objectXML.ElemDefault("Group", "") == "Hexable")
                    {
                        HexTransforms.Add(objectXML);
                    }
                }

                int objectType = objectXML.AttrDefault("type", "0").ParseInt();

                if (objectXML.HasOwnProperty("PetBehavior") || objectXML.HasOwnProperty("PetAbility"))
                {
                    PetXMLDataLibrary[objectType] = objectXML;
                }

                // TODO continue this method
                // See com.company.assembleegameclient.objects.ObjectLibrary
            }
        }

        public static GameObject GetObjectFromType(int objectType)
        {
            XElement objectXML = XmlLibrary[objectType];
            string typeReference = objectXML.ElemDefault("Class", "");

            if (typeReference == "")
            {
                Debug.LogError("No Class element on object type " + objectType + ".");
            }

            Type t = TYPE_MAP[typeReference];

            UnityEngine.GameObject go = new UnityEngine.GameObject();
            go.AddComponent(t);

            return go.GetComponent<GameObject>();
        }
    }
}