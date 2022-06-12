using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class Slice : Base3DObject
    {
        public Slice(Container parent, Shader shader, int vertexArrayID, int vertexCount) : base(shader)
        {
            this._parent = parent;
            this.vertexCount = vertexCount;
            this.vertexArrayID = vertexArrayID;

            Scale = OpenTK.Mathematics.Vector3.One;
            Position = OpenTK.Mathematics.Vector3.Zero;
            Rotation = OpenTK.Mathematics.Vector3.Zero;
            Color = Color4.White;
        }

        public static (int id, int count) createSlice(int angle, Shader shader)
        {
            //              //angle*frag*(vert+norm)
            int vertexCount = angle * 3 * (3 + 3); // one vert = 3 positions(x,y,z), one frag(3 verts) per angle
            Console.WriteLine(3 * 3 * angle);
            float[] sliceVertices = new float[vertexCount];
            int offset = 3*(3+3); // 3 verts pro frag, 3 coords und 3 normals pro vert
            float dist = 1;
            for (int i = 0; i < angle; i++)
            {
                // center
                sliceVertices[i * offset] = 0f; // x
                sliceVertices[i * offset + 1] = 0f; // y 
                sliceVertices[i * offset + 2] = 0f; // z

                // center normal
                sliceVertices[i * offset + 3] = 0f; // x
                sliceVertices[i * offset + 4] = 0f; // y 
                sliceVertices[i * offset + 5] = 1f; // z

                // angle
                sliceVertices[i * offset + 6] = X(i, dist); // x
                sliceVertices[i * offset + 7] = Y(i, dist); // y 
                sliceVertices[i * offset + 8] = 0f; // z

                // angle normal
                sliceVertices[i * offset + 9] = 0f; // x
                sliceVertices[i * offset + 10] = 0f; // y 
                sliceVertices[i * offset + 11] = 1f; // z

                // angle + 1
                sliceVertices[i * offset + 12] = X(i + 1, dist); // x
                sliceVertices[i * offset + 13] = Y(i + 1, dist); // y 
                sliceVertices[i * offset + 14] = 0f; // z

                // angle + 1 normal
                sliceVertices[i * offset + 15] = 0f; // x
                sliceVertices[i * offset + 16] = 0f; // y 
                sliceVertices[i * offset + 17] = 1f; // z
            }

            return (Register(sliceVertices, shader), vertexCount);
        }

        private static int Register(float[] vertices, Shader shader)
        {
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

            // We now need to define the layout of the normal so the shader can use it
            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            return vertexArrayID;
        }

        public static float X(double angle, float dist = 1)
        {
            return dist * (float)Math.Cos(AngleToRad(angle));
        }

        public static float Y(double angle, float dist = 1)
        {
            return dist * (float)Math.Sin(AngleToRad(angle));
        }

        public static double AngleToRad(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        /*
         _CubeVertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _CubeVertexBuffer);

                // set buffer data
                GL.BufferData(BufferTarget.ArrayBuffer, _CubeVertices.Length * sizeof(float), _CubeVertices, BufferUsageHint.StaticDraw);

                _CubeVertexArrayID = GL.GenVertexArray();
                GL.BindVertexArray(_CubeVertexArrayID);

                // describe data
                int vertexLocation = _shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

                // We now need to define the layout of the normal so the shader can use it
                var normalLocation = _shader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            }
            vertexCount = 36; // 3 per fragment; 2 frags per side

            vertexArrayID = _CubeVertexArrayID;
            _parent = parent;
         
         */
    }
}
