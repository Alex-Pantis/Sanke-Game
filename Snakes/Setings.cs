using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes
{
    class Setings
    {

        public static int Width { get; set; }
        public static int Height { get; set; }

        public static string Derection;

        public Setings()
        {
            Width = 16;
            Height = 16;
            Derection = "Left";
        }
    }
}
