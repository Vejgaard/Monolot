using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace KKMono1
{
    public class Ball
    {
        public Vector3 Center;
        public float Radius;
        public Matrix Orientation;

        public Matrix WorldTransform()
        {
            return Matrix.CreateScale(Radius) * Orientation * Matrix.CreateTranslation(Center);
        }
    }
}
