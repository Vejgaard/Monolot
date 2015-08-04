using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KKMono1
{
    public class DrawableModel
    {
        private GraphicsDevice _graphicsDevice;
        private Texture2D _texture;
        private VertexBuffer _buffer;

        public DrawableModel(KModel model, Texture2D texture, GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _texture = texture;

            // Create vertex buffer
            var vertexArray = ModelRendering.ToNonIndexed(model, texture);
            _buffer = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertexArray.Length, BufferUsage.WriteOnly);
            _buffer.SetData(vertexArray);
        }

        public void Draw(BasicEffect effect)
        {
            effect.TextureEnabled = true;
            effect.Texture = _texture;

            _graphicsDevice.SetVertexBuffer(_buffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _buffer.VertexCount / 3);
            }
        }
    }

    public static class ModelRendering
    {
        public static VertexPositionTexture[] ToNonIndexed(KModel model, Texture2D texture)
        {
            var result = new VertexPositionTexture[3 * model.Facets.Count];

            var arrayIndex = 0;
            for (int facetIndex = 0; facetIndex < model.Facets.Count; facetIndex++)
            {
                var facet = model.Facets[facetIndex];

                result[arrayIndex].Position = model.Vertices[facet.Ve0].Pt;
                result[arrayIndex].TextureCoordinate = facet.Tex0;
                arrayIndex++;

                result[arrayIndex].Position = model.Vertices[facet.Ve1].Pt;
                result[arrayIndex].TextureCoordinate = facet.Tex1;
                arrayIndex++;

                result[arrayIndex].Position = model.Vertices[facet.Ve2].Pt;
                result[arrayIndex].TextureCoordinate = facet.Tex2;
                arrayIndex++;
            }

            // If texture coordinates are in pixels, then scale them all to [0,1] x [0,1]
            if (model.TexCoordsAreInPixels)
            {
                var scaleX = 1f / texture.Bounds.Width;
                var scaleY = 1f / texture.Bounds.Height;
                var bounds = texture.Bounds;
                for (int i = 0; i < result.Length; i++)
                {
                    result[i].TextureCoordinate.X *= scaleX;
                    result[i].TextureCoordinate.Y *= scaleY;
                }
            }

            return result;
        }
    }
}
