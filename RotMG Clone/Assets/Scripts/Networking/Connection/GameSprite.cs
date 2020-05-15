using Networking.GameMap;
using RotMG_Net_Lib.Networking.Packets.Incoming;

namespace Networking.Connection
{
    public class GameSprite
    {
        public bool IsGameStarted { get; set; }

        public GameServerConnectionConcrete GameServerConnectionConcrete;
        
        public void Connect()
        {
            if (!this.IsGameStarted)
            {
                IsGameStarted = true;
                GameServerConnectionConcrete.Connect();
            }
        }

        public void ApplyMapInfo(MapInfoPacket mapInfoPacket)
        {
            Map.SetProps(mapInfoPacket.Width, mapInfoPacket.Height, mapInfoPacket.Name, mapInfoPacket.Background, mapInfoPacket.AllowPlayerTeleport, mapInfoPacket.ShowDisplays);
        }
    }
}