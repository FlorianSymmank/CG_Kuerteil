namespace CG_Kuerteil.Graphics
{
    public class VertexData
    {
        public readonly int ArrayID;
        public readonly int BufferID;
        public readonly int Count;

        public VertexData(int arrayID, int bufferID, int count)
        {
            ArrayID = arrayID;
            BufferID = bufferID;
            Count = count;
        }
    }
}