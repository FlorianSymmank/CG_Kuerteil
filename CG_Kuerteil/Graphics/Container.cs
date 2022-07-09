using OpenTK.Mathematics;

namespace CG_Kuerteil.Graphics
{
    public class Container : Base3DObject
    {
        public List<Base3DObject> Base3DObjects = new();
        public Container() { }
        public Matrix4 model = Matrix4.Identity;
        public override void Render()
        {
            foreach (var obj in Base3DObjects)
                obj.Render();
        }
        public override float RotationX
        {
            get => base.RotationX;
            set
            {
                base.RotationX = value;
                model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(value));
            }
        }
        public override float RotationY
        {
            get => base.RotationY;
            set
            {
                base.RotationY = value;
                model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(value));
            }
        }
        public override float RotationZ
        {
            get => base.RotationZ;
            set
            {
                base.RotationZ = value;
                model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(value));
            }
        }
        public enum Direction { X, Y, Z }
        public void AddRotation(Direction direction, float radian)
        {
            switch (direction)
            {
                case Direction.X:
                    model *= Matrix4.CreateRotationX(radian);
                    break;
                case Direction.Y:
                    model *= Matrix4.CreateRotationY(radian);
                    break;
                case Direction.Z:
                    model *= Matrix4.CreateRotationZ(radian);
                    break;
                default:
                    break;
            }
        }
    }
}