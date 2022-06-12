using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class Light
    {
        private Color4 color;
        public Color4 Color
        {
            get => color;
            set
            {
                color = value;
                shader.SetVector4("lightColor", (Vector4)color);
            }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                shader.SetVector3("lightPos", position);
            }
        }

        private Shader shader;
        public Light(Vector3 pos, Shader shader, Camera camera, Color4 color)
        {
            this.shader = shader;
            Position = pos;
            Color = color;

            shader.SetVector3("viewPos", camera.Position);
        }
    }
}
