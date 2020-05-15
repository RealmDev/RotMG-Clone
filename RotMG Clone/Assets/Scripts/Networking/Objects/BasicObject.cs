using Networking.GameMap;
using UnityEngine;

namespace Networking.Objects
{
    public class BasicObject : MonoBehaviour
    {
        public Map Map { get; set; }

        public int ObjectId { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        public bool HasShadow { get; set; }
        public bool Drawn { get; set; }

        /// <summary>
        /// Updates the object based on the tick time.
        /// </summary>
        /// <param name="time">Tick time</param>
        /// <param name="dt">TODO</param>
        /// <returns>False if the object is to be destroyed.</returns>
        public virtual bool _Update(int time, int dt)
        {
            return true;
        }
        
        public bool AddTo(Map map, float x, float y)
        {
            this.Map = map;
            this.X = x;
            this.Y = y;

            return true;
        }
    }
}