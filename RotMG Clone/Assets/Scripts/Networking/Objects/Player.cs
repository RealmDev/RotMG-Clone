using System.Xml.Linq;
using RotMG_Net_Lib.Models;

namespace Networking.Objects
{
    public class Player : GameObject
    {
        public WorldPosData Position { get; set; }

        public Player(XElement xml) : base(xml)
        {
        }
    }
}