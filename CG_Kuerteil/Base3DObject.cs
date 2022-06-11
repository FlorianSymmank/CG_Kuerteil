using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class Base3DObject
    {
        private readonly int vertexArrayID;
        private Vector3 scale;
        private Vector3 position;
        private Color4 color;
        private readonly Container _parent;

        public Base3DObject(string name, Container parent)
        {
            vertexArrayID = Register.GetRegister().GetArrayID(name);
            _parent = parent;
        }

        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Scale { get => scale; set => scale = value; }
        public Color4 Color { get => color; set => color = value; }

        public void render(Shader shader)
        {
            Matrix4 model = Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(position) * _parent.model;
            shader.SetVector4("instanceColor", (Vector4)color);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", Register.GetRegister().GetCamera().GetViewMatrix());
            shader.SetMatrix4("projection", Register.GetRegister().GetCamera().GetProjectionMatrix());

            GL.BindVertexArray(vertexArrayID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}