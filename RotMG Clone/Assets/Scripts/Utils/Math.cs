using System;

namespace Utils
{
    public class Math
    {
        private static Random r = new Random();
        
        public static double Random()
        {
            return r.NextDouble();
        }
    }
}