using OpenTK.Mathematics;

namespace CG_Kuerteil
{
    public class Container : Base3DObject
    {
        public List<Base3DObject> Base3DObjects = new();

        public Container(Shader shader) : base(shader)
        {

        }

        public Matrix4 model = Matrix4.Identity;

        public override void Render()
        {
            foreach (var obj in Base3DObjects)
            {
                obj.Render();
            }
        }
    }
}
