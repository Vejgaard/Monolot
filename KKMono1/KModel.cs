using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace KKMono1
{
    // Consider derived vertex / facet types with/without texture coordinates and normals

    public class KVertex
    {
        public Vector3 Pt;

    }

    public class KFacet
    { 
        // Vertex indices (in counter-clockwise order looking at front face)
        public int Ve0;
        public int Ve1;
        public int Ve2;

        // Texture coordinates (in image pixel coordinates)
        public Vector2 Tex0;
        public Vector2 Tex1;
        public Vector2 Tex2;
    }

    public class KModel
    {
        // Geometry
        public List<KVertex> Vertices = new List<KVertex>();
        public List<KFacet> Facets = new List<KFacet>();
        public bool TexCoordsAreInPixels = true;

        // Visualization
        // - smooth/flat shading?
        // - texture

        public int AddVertex(Vector3 pt)
        {
            Vertices.Add(new KVertex { Pt = pt });
            return Vertices.Count - 1;
        }

        public int AddFacet(int ve0, int ve1, int ve2)
        {
            Facets.Add(new KFacet { Ve0 = ve0, Ve1 = ve1, Ve2 = ve2 });
            return Facets.Count - 1;
        }

        //public void Transform(Matrix

        public void Append(KModel other)
        { 

        }
    }
}
