using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace KKMono1
{
    public static class GeometricalPrimitives
    {
        public static void AddQuad(this KModel model, int veLowerLeft, int veLowerRight, int veUpperLeft, int veUpperRight, Rectangle texRect)
        {
            //  UL   UR
            //   *---*
            //   |\  |
            //   | \ |
            //   |  \|
            //   *---*
            //  LL   LR

            // Create the LL facet
            var faId = model.AddFacet(veUpperLeft, veLowerLeft, veLowerRight);
            var fa = model.Facets[faId];
            fa.Tex0 = new Vector2(texRect.Left, texRect.Top);
            fa.Tex1 = new Vector2(texRect.Left, texRect.Bottom);
            fa.Tex2 = new Vector2(texRect.Right, texRect.Bottom);
            
            // Create the UR facet
            faId = model.AddFacet(veUpperLeft, veLowerRight, veUpperRight);
            fa = model.Facets[faId];
            fa.Tex0 = new Vector2(texRect.Left, texRect.Top);
            fa.Tex1 = new Vector2(texRect.Right, texRect.Bottom);
            fa.Tex2 = new Vector2(texRect.Right, texRect.Top);
        }

        /// <summary>
        /// Let the side with X to the right and Y up be the front.
        /// The texture is supposed to contain six (probably square) parts like this:
        ///     [ Left | Front | Right | Back | Top | Bottom ]
        /// Where the first four transition into each other
        /// and front and bottom transition into front.
        /// </summary>
        /// <param name="texRect">The texture coordinates for the first side only</param>
        public static void AddBox(this KModel model, BoundingBox bb, Rectangle texRect)
        {
            //    6*----*7
            //    /    /|
            //  2*----*3|     Y   
            //   |    | *5    |  
            //   |    |/      |  
            //  0*----*1      *---X
            //               /
            //              /
            //             Z
            int ve0 = model.AddVertex(new Vector3(bb.Min.X, bb.Min.Y, bb.Max.Z));
            int ve1 = model.AddVertex(new Vector3(bb.Max.X, bb.Min.Y, bb.Max.Z));
            int ve2 = model.AddVertex(new Vector3(bb.Min.X, bb.Max.Y, bb.Max.Z));
            int ve3 = model.AddVertex(new Vector3(bb.Max.X, bb.Max.Y, bb.Max.Z));
            int ve4 = model.AddVertex(new Vector3(bb.Min.X, bb.Min.Y, bb.Min.Z));
            int ve5 = model.AddVertex(new Vector3(bb.Max.X, bb.Min.Y, bb.Min.Z));
            int ve6 = model.AddVertex(new Vector3(bb.Min.X, bb.Max.Y, bb.Min.Z));
            int ve7 = model.AddVertex(new Vector3(bb.Max.X, bb.Max.Y, bb.Min.Z));

            // Left
            model.AddQuad(ve4, ve0, ve6, ve2, texRect);

            // Front
            texRect.X += texRect.Width;
            model.AddQuad(ve0, ve1, ve2, ve3, texRect);

            // Right
            texRect.X += texRect.Width;
            model.AddQuad(ve1, ve5, ve3, ve7, texRect);

            // Back
            texRect.X += texRect.Width;
            model.AddQuad(ve5, ve4, ve7, ve6, texRect);

            // Top
            texRect.X += texRect.Width;
            model.AddQuad(ve2, ve3, ve6, ve7, texRect);

            // Bottom
            texRect.X += texRect.Width;
            model.AddQuad(ve4, ve5, ve0, ve1, texRect);
        }

        /// <summary>
        /// Creates an icosahedron (regular polyhedron with 20 triangle sides) with vertices on the unit sphere.
        /// </summary>
        public static KModel CreateIcosahedron(Rectangle texRect)
        {
            // aka golden ratio
            float tao = (float)(1 + Math.Sqrt(5)) / 2;

            var model = new KModel();
            int v0 = model.AddVertex(new Vector3(1, tao, 0).Normalized());
            int v1 = model.AddVertex(new Vector3(-1, tao, 0).Normalized());
            int v2 = model.AddVertex(new Vector3(1, -tao, 0).Normalized());
            int v3 = model.AddVertex(new Vector3(-1, -tao, 0).Normalized());
            int v4 = model.AddVertex(new Vector3(0, 1, tao).Normalized());
            int v5 = model.AddVertex(new Vector3(0, -1, tao).Normalized());
            int v6 = model.AddVertex(new Vector3(0, 1, -tao).Normalized());
            int v7 = model.AddVertex(new Vector3(0, -1, -tao).Normalized());
            int v8 = model.AddVertex(new Vector3(tao, 0, 1).Normalized());
            int v9 = model.AddVertex(new Vector3(-tao, 0, 1).Normalized());
            int vA = model.AddVertex(new Vector3(tao, 0, -1).Normalized());
            int vB = model.AddVertex(new Vector3(-tao, 0, -1).Normalized());

            model.AddFacet(v0, v6, v1);
            model.AddFacet(v0, vA, v6);
            model.AddFacet(v0, v8, vA);
            model.AddFacet(v0, v4, v8);
            model.AddFacet(v0, v1, v4);

            model.AddFacet(v3, v5, v9);
            model.AddFacet(v3, v2, v5);
            model.AddFacet(v3, v7, v2);
            model.AddFacet(v3, vB, v7);
            model.AddFacet(v3, v9, vB);

            model.AddFacet(v1, v6, vB);
            model.AddFacet(v1, v9, v4);
            model.AddFacet(v1, vB, v9);
            model.AddFacet(v2, v7, vA);
            model.AddFacet(v2, v8, v5);
            model.AddFacet(v2, vA, v8);
            model.AddFacet(v4, v5, v8);
            model.AddFacet(v4, v9, v5);
            model.AddFacet(v6, v7, vB);
            model.AddFacet(v6, vA, v7);

            // Set texture coordinates on all facets
            foreach (var facet in model.Facets)
            {
                facet.Tex0 = new Vector2(texRect.Left, texRect.Top);
                facet.Tex1 = new Vector2((texRect.Left + texRect.Right) / 2f, texRect.Bottom);
                facet.Tex2 = new Vector2(texRect.Right, texRect.Top);
            }

            return model;
        }
    
        /// <summary>
        /// Creates a unit ball.
        /// Creates an icosahedron and subdivides it a number of times given by the resolution argument.
        /// (number of facets becomes 20 * 4 ^ resolution)
        /// </summary>
        public static KModel CreateBall(int resolution, Rectangle texRect)
        {
            var model = CreateIcosahedron(texRect);

            for (int i = 0; i < resolution; i++)
            {
                SplitEachFacetInFour(model);
                foreach (var vertex in model.Vertices)
                    vertex.Pt.Normalize();
            }

            return model;
        }

        private static void SplitEachFacetInFour(KModel model)
        {
            // Create a new vertex at the midpoint of every edge
            var edge2Vertex = new Dictionary<Tuple<int, int>, int>();

            foreach (var facet in model.Facets)
            {
                foreach (var t in new Tuple<int, int>[] {
                    Tuple.Create(facet.Ve0, facet.Ve1),
                    Tuple.Create(facet.Ve1, facet.Ve2),
                    Tuple.Create(facet.Ve2, facet.Ve0)})
                {
                    if (!edge2Vertex.ContainsKey(t))
                    {
                        Vector3 midPoint = (model.Vertices[t.Item1].Pt + model.Vertices[t.Item2].Pt) / 2;
                        int veMidpoint = model.AddVertex(midPoint);
                        edge2Vertex.Add(t, veMidpoint);
                        edge2Vertex.Add(Tuple.Create(t.Item2, t.Item1), veMidpoint);
                    }
                }
            }

            // Split each facet in four
            var originalFacetCount = model.Facets.Count;
            for (int faId = 0; faId < originalFacetCount; faId++)
            {
                var f = model.Facets[faId];

                // Corners of original facet
                int ve0 = f.Ve0;
                int ve1 = f.Ve1;
                int ve2 = f.Ve2;

                // Edge midpoints
                int ve01 = edge2Vertex[Tuple.Create(f.Ve0, f.Ve1)];
                int ve12 = edge2Vertex[Tuple.Create(f.Ve1, f.Ve2)];
                int ve20 = edge2Vertex[Tuple.Create(f.Ve2, f.Ve0)];

                // Texture coordinates of corners
                var tex0 = f.Tex0;
                var tex1 = f.Tex1;
                var tex2 = f.Tex2;

                // Texture coordinates of midpoints
                var tex01 = 0.5f * (tex0 + tex1);
                var tex12 = 0.5f * (tex1 + tex2);
                var tex20 = 0.5f * (tex2 + tex0);

                //
                //      2
                //     / \
                //   20---12
                //   / \ / \
                //  0---01--1

                // First facet: 0-01-20 (change the original)
                f.Ve1 = ve01;
                f.Ve2 = ve20;
                f.Tex1 = tex01;
                f.Tex2 = tex20;

                // 01-1-12
                f = model.Facets[model.AddFacet(ve01, ve1, ve12)];
                f.Tex0 = tex01;
                f.Tex1 = tex1;
                f.Tex2 = tex12;

                // 01-12-20
                f = model.Facets[model.AddFacet(ve01, ve12, ve20)];
                f.Tex0 = tex01;
                f.Tex1 = tex12;
                f.Tex2 = tex20;

                // 20-12-2
                f = model.Facets[model.AddFacet(ve20, ve12, ve2)];
                f.Tex0 = tex20;
                f.Tex1 = tex12;
                f.Tex2 = tex2;
            }
        }

    }
}
