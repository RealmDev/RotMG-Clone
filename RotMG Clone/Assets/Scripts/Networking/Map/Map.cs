namespace Networking.Map
{
    
    /**
     * Note : This class will be a Unity Tilemap.
     * It handles the tiles.
     */
    public class Map : AbstractMap
    {
        public const string CLOTH_BAZAAR = "Cloth Bazaar";
        public const string NEXUS = "Nexus";
        public const string DAILY_QUEST_ROOM = "Daily Quest Room";
        public const string DAILY_LOGIN_ROOM = "Daily Login Room";
        public const string PET_YARD_1 = "Pet Yard";
        public const string PET_YARD_2 = "Pet Yard 2";
        public const string PET_YARD_3 = "Pet Yard 3";
        public const string PET_YARD_4 = "Pet Yard 4";
        public const string PET_YARD_5 = "Pet Yard 5";
        public const string REALM = "Realm of the Mad God";
        public const string ORYX_CHAMBER = "Oryx\'s Chamber";
        public const string GUILD_HALL = "Guild Hall";
        public const string GUILD_HALL_2 = "Guild Hall 2";
        public const string GUILD_HALL_3 = "Guild Hall 3";
        public const string GUILD_HALL_4 = "Guild Hall 4";
        public const string GUILD_HALL_5 = "Guild Hall 5";
        public const string NEXUS_EXPLANATION = "Nexus_Explanation";
        public const string VAULT = "Vault";

        
        public static void SetProps(int width, int height, string name, int background, bool allowPlayerTeleport, bool showDisplays)
        {
            throw new System.NotImplementedException();
        }
        
        public static void RemoveObject(int drop)
        {
        }

        public static void SetGroundTile(short x, short y, ushort type)
        {
            
        }


    }
}