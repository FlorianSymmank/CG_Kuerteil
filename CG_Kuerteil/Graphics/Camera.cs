using OpenTK.Mathematics;
using static OpenTK.Mathematics.MathHelper;

namespace CG_Kuerteil.Graphics
{
    public class Camera
    {
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;
        private float _pitch;
        private float _yaw = -PiOver2; // Without this, you would be started rotated 90 degrees right.
        private float _fov = PiOver2;
        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }
        public Vector3 Position { get; set; }
        public float AspectRatio { private get; set; }
        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public Vector3 Right => _right;
        public float Pitch
        {
            get => RadiansToDegrees(_pitch);
            set
            {
                var angle = Clamp(value, -89f, 89f);
                _pitch = DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => RadiansToDegrees(_yaw);
            set
            {
                _yaw = DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Fov
        {
            get => RadiansToDegrees(_fov);
            set
            {
                var angle = Clamp(value, 1f, 90f);
                _fov = DegreesToRadians(angle);
            }
        }
        public Matrix4 GetViewMatrix() => Matrix4.LookAt(Position, Position + _front, _up);
        public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}