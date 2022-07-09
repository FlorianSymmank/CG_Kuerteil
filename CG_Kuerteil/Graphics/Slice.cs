using CG_Kuerteil.Util;
using OpenTK.Mathematics;
using static CG_Kuerteil.Util.MathHelper;

namespace CG_Kuerteil.Graphics
{
    public class Slice : Base3DObject
    {
        public readonly int Angle;
        public Slice(Container parent, int angle)
        {
            CreateSlice(angle);

            Angle = angle;

            _parent = parent;
            Scale = Vector3.One;
        }
        private int offsetID = 0;
        private int OffsetID => offsetID++;
        private void CreateSlice(int angle)
        {
            int indexesF = 18; // Front points
            int indexesB = 18; // Back  points
            int indexesR = 2 * 18; // right side

            int offset = indexesF + indexesB + indexesR;
            int vertices = angle * offset;

            if (Register.GetRegister().TryGetVertexArray($"{GetType().Name}{vertices}", out var data))
            {
                vertexArrayID = data.ArrayID;
                vertexCount = data.Count;
                return;
            }

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

            RegisterArray(sliceVertices, vertices);
        }
    }
}