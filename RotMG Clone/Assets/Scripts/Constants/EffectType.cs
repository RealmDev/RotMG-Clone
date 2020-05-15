namespace Constants
{
    public enum EffectType
    {
        Unknown = 0,
        Heal = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        Nova = 5, //radius=pos1.x
        Poison = 6,
        Line = 7,
        Burst = 8, //radius=dist(pos1,pos2)
        Flow = 9,
        Ring = 10, //radius=pos1.x
        Lightning = 11, //particleSize=pos2.x
        Collapse = 12, //radius=dist(pos1,pos2)
        ConeBlast = 13, //origin=pos1, radius = pos2.x
        Earthquake = 14,
        Flash = 15, //period=pos1.x, numCycles=pos1.y
        BeachBall = 16,
        ElectricBolts = 17, //If a pet paralyzes a monster
        ElectricFlashing = 18, //If a monster got paralyzed from a electric pet
        RisingFury = 19 //If a pet is standing still (this white particles)
    }
}