using OpenTK.Mathematics;

namespace CG_Kuerteil.Graphics
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
