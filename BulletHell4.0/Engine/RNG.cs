using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHell4_0
{
    public static class RNG
    {
        static Random random;

        static RNG()
        {
            random = new Random();
        }

        public static int GetRNG(int a, int b)
        {
            return random.Next(a, b + 1);
        }

        
    }
}
