using System.Xml.Linq;
using Utils;

namespace Constants
{
    public class SpawnCount
    {
        public int Mean { get; }
        public int StdDev { get; }
        public int Min { get; }
        public int Max { get; }

        public SpawnCount(XElement elem)
        {
            //TODO is "mean" suposed to be empty?
            Mean = elem.ElemDefault("Mean", "0").ParseInt();
            StdDev = elem.ElemDefault("StdDev", "0").ParseInt();
            Min = elem.ElemDefault("Min", "0").ParseInt();
            Max = elem.ElemDefault("Max", "0").ParseInt();
        }
    }
}