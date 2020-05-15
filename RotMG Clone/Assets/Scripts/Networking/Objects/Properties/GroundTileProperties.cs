using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using NLog;
using Utils;

namespace Networking.Objects.Properties
{
    // Formely known as a "Biome" or "Tile"
    public class GroundTileProperties
    {

        public int MaxDamage;

        public int MinDamage;

        public bool NoWalk = true;

        public bool Sink = false;
        
        private float Speed;

        public XElement Element { get; }

        public Color Color { get; }
        public bool Damaging { get; }

        public ushort ObjectType { get; }
        public string ObjectId { get; }

        public GroundTileProperties(XElement tile)
        {
            Element = tile;
            
            ObjectType = (ushort) tile.AttrDefault("type", "0x0").ParseHex();
            ObjectId = tile.AttrDefault("id", "");

            NoWalk = tile.HasOwnProperty("NoWalk");

            MinDamage = tile.ElemDefault("MinDamage", "0").ParseInt();
            MaxDamage = tile.ElemDefault("MaxDamage", "0").ParseInt();

            if (MinDamage > 0 || MaxDamage > 0) Damaging = true;

            Speed = tile.ElemDefault("Speed", "1").ParseFloat();

            Color = tile.ElemDefault("Color", "0x00000").ParseHexColor();

            //log.Info("Loaded tile " + ObjectId + " with color " + Color);
            
            // Edge
        }

        public static Dictionary<ushort, GroundTileProperties> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, GroundTileProperties>();

            doc.Element("GroundTypes")
                .Elements("Ground")
                .ForEach(tile =>
                {
                    GroundTileProperties t = new GroundTileProperties(tile);
                    map[t.ObjectType] = t;
                });

            return map;
        }

        public override string ToString()
        {
            return string.Format("Tile: {0} (0x{1:X})", ObjectId, ObjectType);
        }
    }
}