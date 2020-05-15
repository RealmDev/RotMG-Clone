using System.Xml.Linq;
using Utils;

namespace Networking.Objects.Properties
{
    public class PortalProperties
    {
        public string DisplayId { get; }

        public string DungeonName { get; set; }
        public int TimeoutTime { get; }
        public bool NexusPortal { get; }
        public string ObjectId { get; }
        public ushort ObjectType { get; }

        public PortalProperties(XElement elem)
        {
            ObjectType = (ushort) elem.ElemDefault("type", "0x0").ParseHex();

            ObjectId = elem.AttrDefault("id", "");

            DisplayId = elem.ElemDefault("DisplayId", "");

            NexusPortal = elem.Element("NexusPortal") != null;

            if (elem.Element("DungeonName") != null) //<NexusPortal/>
                DungeonName = elem.Element("DungeonName").Value;

            TimeoutTime = ObjectId == "The Shatters" ? 70 : 30;
        }
    }
}