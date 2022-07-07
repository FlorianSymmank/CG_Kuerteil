using CG_Kuerteil.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_Kuerteil.Graphics
{
    public abstract class Base3DObject
    {
        protected int vertexArrayID;
        private Color4 color;
        private Vector3 position;
        private Vector3 scale;
        protected Container? _parent;
        protected int vertexCount = 0;

        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Scale { get => scale; set => scale = value; }

        private float rotationX = 0f;

        public virtual float RotationX
        {
            get => OpenTK.Mathematics.MathHelper.DegreesToRadians(rotationX);
            set
            {
                rotationX = value % 360;
            }
        }

        private float rotationY = 0f;

        public virtual float RotationY
        {
            get => OpenTK.Mathematics.MathHelper.DegreesToRadians(rotationY);
            set
            {
                rotationY = value % 360;
            }
        }

        private float rotationZ = 0f;

        public virtual float RotationZ
        {
            get => OpenTK.Mathematics.MathHelper.DegreesToRadians(rotationZ);
            set
            {
                rotationZ = value % 360;
            }
        }

        public Color4 Color { get => color; set => color = value; }

        public virtual void Render()
        {
            Shader shader = Register.GetRegister().Get<Shader>();

            Matrix4 parentMat;
            if (_parent != null)
                parentMat = _parent.model;
            else
                parentMat = Matrix4.Identity;

            Matrix4 model = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(position) * Matrix4.CreateRotationX(RotationX) * Matrix4.CreateRotationY(RotationY) * Matrix4.CreateRotationZ(RotationZ) * parentMat;
            shader.SetVector4("objectColor", (Vector4)color);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", Register.GetRegister().Get<Camera>().GetViewMatrix());
            shader.SetMatrix4("projection", Register.GetRegister().Get<Camera>().GetProjectionMatrix());
            shader.SetInt("mode", 1);

            GL.BindVertexArray(vertexArrayID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
        }

        protected void RegisterArray(float[] vertices, int vertexCount)
        {
            Shader shader = Register.GetRegister().Get<Shader>();

            if (Register.GetRegister().TryGetVertexArray($"{GetType().Name}{vertexCount}", out var data))
            {
                this.vertexCount = data.Count;
                this.vertexArrayID = data.ArrayID;
                return;
            }

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            // set buffer data
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int vertexArrayID = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayID);

            // describe data
            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            // We now need to define the laout of the normal so the shader can use it
            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            this.vertexCount = vertexCount;
            this.vertexArrayID = vertexArrayID;

            Register.GetRegister().RegisterVertexArray($"{GetType().Name}{vertexCount}", new(vertexArrayID, vertexBuffer, vertexCount));
        }
    }
}