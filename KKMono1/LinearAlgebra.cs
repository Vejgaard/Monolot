using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace KKMono1
{
    public static class LinearAlgebra
    {
        public static Vector3 Normalized(this Vector3 v)
        {
            v.Normalize();
            return v;
        }
    }
}
