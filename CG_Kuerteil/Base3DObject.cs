using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_Kuerteil
{
    public abstract class Base3DObject
    {
        protected int vertexArrayID;
        private Color4 color;
        private Vector3 position;
        private Vector3 scale;
        private Vector3 rotation;
        protected Container? _parent;
        protected Shader _shader;
        protected int vertexCount = 0;

        public Base3DObject(Shader shader)
        {
            _shader = shader;
        }

        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Scale { get => scale; set => scale = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        public Color4 Color { get => color; set => color = value; }

        public virtual void Render()
        {
            Logger.Log("Render id: " + vertexArrayID);
            Logger.Log("vertexCount: " + vertexCount);


            Matrix4 parentMat;
            if (_parent != null)
                parentMat = _parent.model;
            else
                parentMat = Matrix4.Identity;

            Matrix4 model = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(position) * parentMat;
            _shader.SetVector4("objectColor", (Vector4)color);
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", Register.GetRegister().GetCamera().GetViewMatrix());
            _shader.SetMatrix4("projection", Register.GetRegister().GetCamera().GetProjectionMatrix());

            GL.BindVertexArray(vertexArrayID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
        }
    }
}