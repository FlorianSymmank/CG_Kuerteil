using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static CG_Kuerteil.MathHelper;

namespace CG_Kuerteil
{
    public class Slice : Base3DObject
    {
        public Slice(Container parent, Shader shader, int angle) : base(shader)
        {
            CreateSlice(angle, out int vertexArrayID, out int vertexCount);

            this._parent = parent;
            this.vertexCount = vertexCount;
            this.vertexArrayID = vertexArrayID;

            Scale = OpenTK.Mathematics.Vector3.One;
            Position = OpenTK.Mathematics.Vector3.Zero;
            Rotation = OpenTK.Mathematics.Vector3.Zero;
            Color = Color4.White;
        }

        private static Dictionary<int, (int id, int count)> SliceMap = new();

        private int offsetID = 0;
        private int OffsetID => offsetID++;
        private void CreateSlice(int angle, out int vertexArrayID, out int vertexCount)
        {
            if (SliceMap.ContainsKey(angle))
            {
                var value = SliceMap[angle];
                vertexArrayID = value.id;
                vertexCount = value.count;
                return;
            }

            int indexesF = 18; // Front points
            int indexesB = 18; // Back  points
            int indexesR = 2 * 18; // right side

            int offset = indexesF + indexesB + indexesR;
            int vertices = angle * offset;

            float[] sliceVertices = new float[vertices];
            float radius = 1;
            float height = .1f;

            Vector2 center = Vector2.Zero;
            Vector2 normal = Vector2.Zero;
            Vector2 v = Vector2.Zero;

            for (int i = 0; i < angle; i++)
            {
                // Front Face
                // center
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                // center normal            
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = 1f; // z

                // angle
                sliceVertices[i * offset + OffsetID] = CircleX(i, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i, radius); // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                // angle normal
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = 1f; // z

                // angle + 1
                sliceVertices[i * offset + OffsetID] = CircleX(i + 1, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 1, radius); // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                // angle + 1 normal
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = 1f; // z



                // Back Face
                // center
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // center normal            
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = -1f; // z

                // angle
                sliceVertices[i * offset + OffsetID] = CircleX(i, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i, radius); // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // angle normal
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = -1f; // z

                // angle + 1
                sliceVertices[i * offset + OffsetID] = CircleX(i + 1, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 1, radius); // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // angle + 1 normal
                sliceVertices[i * offset + OffsetID] = 0f; // X
                sliceVertices[i * offset + OffsetID] = 0f; // Y 
                sliceVertices[i * offset + OffsetID] = -1f; // z



                // Right Face 1
                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i, radius); // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                // normal
                v = new Vector2(CircleX(i, radius), CircleY(i, radius));
                normal = v - center;
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z

                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i, radius); // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // normal
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z

                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i + 1f, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 1f, radius); // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // normal
                v = new Vector2(CircleX(i + 1f, radius), CircleY(i + 1f, radius));
                normal = v - center;
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z


                // Right Face 2
                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i + 1f, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 1f, radius); // Y 
                sliceVertices[i * offset + OffsetID] = -height; // z

                // normal            
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z

                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i + 1, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 1, radius); // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                // normal
                v = new Vector2(CircleX(i + 1f, radius), CircleY(i + 1f, radius));
                normal = v - center;
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z

                // vertex
                sliceVertices[i * offset + OffsetID] = CircleX(i + 0f, radius); // X
                sliceVertices[i * offset + OffsetID] = CircleY(i + 0f, radius); // Y 
                sliceVertices[i * offset + OffsetID] = height; // z

                //normal
                v = new Vector2(CircleX(i, radius), CircleY(i, radius));
                normal = v - center;
                sliceVertices[i * offset + OffsetID] = normal.X; // X
                sliceVertices[i * offset + OffsetID] = normal.Y; // Y 
                sliceVertices[i * offset + OffsetID] = 0f; // z

                offsetID = 0;
            }

            vertexArrayID = Register(sliceVertices);
            vertexCount = vertices;

            SliceMap.Add(angle, (vertexArrayID, vertexCount));
        }

        private int Register(float[] vertices)
        {
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            // set buffer data
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int vertexArrayID = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayID);

            // describe data
            int vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            // We now need to define the laCircleYout of the normal so the shader can use it
            var normalLocation = _shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            return vertexArrayID;
        }
    }
}
