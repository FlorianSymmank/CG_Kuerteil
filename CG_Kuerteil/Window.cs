using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace CG_Kuerteil
{
    public class Window : GameWindow
    {
        private readonly float[] _CubeVertices = {
             // positions        
             -0.5f, -0.5f, -0.5f,
              0.5f, -0.5f, -0.5f,
              0.5f,  0.5f, -0.5f,
              0.5f,  0.5f, -0.5f,
             -0.5f,  0.5f, -0.5f,
             -0.5f, -0.5f, -0.5f,

             -0.5f, -0.5f,  0.5f,
              0.5f, -0.5f,  0.5f,
              0.5f,  0.5f,  0.5f,
              0.5f,  0.5f,  0.5f,
             -0.5f,  0.5f,  0.5f,
             -0.5f, -0.5f,  0.5f,

             -0.5f,  0.5f,  0.5f,
             -0.5f,  0.5f, -0.5f,
             -0.5f, -0.5f, -0.5f,
             -0.5f, -0.5f, -0.5f,
             -0.5f, -0.5f,  0.5f,
             -0.5f,  0.5f,  0.5f,

              0.5f,  0.5f,  0.5f,
              0.5f,  0.5f, -0.5f,
              0.5f, -0.5f, -0.5f,
              0.5f, -0.5f, -0.5f,
              0.5f, -0.5f,  0.5f,
              0.5f,  0.5f,  0.5f,

             -0.5f, -0.5f, -0.5f,
              0.5f, -0.5f, -0.5f,
              0.5f, -0.5f,  0.5f,
              0.5f, -0.5f,  0.5f,
             -0.5f, -0.5f,  0.5f,
             -0.5f, -0.5f, -0.5f,

             -0.5f,  0.5f, -0.5f,
              0.5f,  0.5f, -0.5f,
              0.5f,  0.5f,  0.5f,
              0.5f,  0.5f,  0.5f,
             -0.5f,  0.5f,  0.5f,
             -0.5f,  0.5f, -0.5f,
        };

        private int _CubeVertexBuffer;

        private int _CubeVertexArrayID;

        private Shader _shader;

        private Stopwatch _timer = new Stopwatch();

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        //private Matrix4 _model = Matrix4.Identity;

        private Container _container;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Logger.Log("Create Window");
            _timer.Start();
        }

        protected override void OnLoad()
        {
            Logger.Log("Load Window");

            base.OnLoad();

            // set BackgroundColor
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // create and bind a vertex buffer
            _CubeVertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _CubeVertexBuffer);

            // set buffer data
            GL.BufferData(BufferTarget.ArrayBuffer, _CubeVertices.Length * sizeof(float), _CubeVertices, BufferUsageHint.StaticDraw);

            _CubeVertexArrayID = GL.GenVertexArray();
            GL.BindVertexArray(_CubeVertexArrayID);
            Register.GetRegister().RegisterArrayID("cube", _CubeVertexArrayID);

            // load shader
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            // enable the shader.
            _shader.Use();

            // describe data
            int vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //int color = _shader.GetAttribLocation("aColor");
            //GL.EnableVertexAttribArray(color);
            //GL.VertexAttribPointer(color, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(new(0, 0, 2f), Size.X / (float)Size.Y);
            Register.GetRegister().RegisterCamera(_camera);

            _container = new Container();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Bind the shader
            _shader.Use();

            for (int i = 0; i < _container.length; i++)
                _container[i].render(_shader);
            

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(_CubeVertexBuffer);
            GL.DeleteVertexArray(_CubeVertexArrayID);

            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
                return;

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Logger.Log("Exit");
                System.Environment.Exit(1);
            }

            const float sensitivity = 0.005f;

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else if (!mouse.IsButtonPressed(MouseButton.Left) && mouse.IsButtonDown(MouseButton.Left))
            {
                var angleY = MathHelper.Clamp(mouse.Y - _lastPos.Y, -89f, 89f) * sensitivity;
                var angleX = MathHelper.Clamp(mouse.X - _lastPos.X, -89f, 89f) * sensitivity;

                _container.model *= Matrix4.CreateRotationX(angleY);
                _container.model *= Matrix4.CreateRotationY(angleX);

                Console.WriteLine(_container.model.ToString());
            }

            _lastPos = new Vector2(mouse.X, mouse.Y);
        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }
    }
}