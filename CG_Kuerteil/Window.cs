using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Mathematics;


namespace CG_Kuerteil
{
    // Tutorial https://opentk.net/learn/index.html
    public class Window : GameWindow
    {
        private Shader _shader;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private Container container;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Logger.Log("Create Window");
        }

        protected override void OnLoad()
        {
            Logger.Log("Load Window");

            base.OnLoad();

            // set BackgroundColor
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // load shader
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            // enable the shader.
            _shader.Use();

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(new(0, 0, 2f), Size.X / (float)Size.Y);

            Light light = new(new Vector3(0, 3, 15f), _shader, _camera, Color4.White);

            Register.GetRegister().RegisterObject(_camera);
            Register.GetRegister().RegisterObject(_shader);

            container = new PieDiagram();
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Bind the shader
            _shader.Use();

            container.Render();

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

            Register.GetRegister().DestroyAllVertexArray();

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
                var angleY = OpenTK.Mathematics.MathHelper.Clamp(mouse.Y - _lastPos.Y, -89f, 89f) * sensitivity;
                var angleX = OpenTK.Mathematics.MathHelper.Clamp(mouse.X - _lastPos.X, -89f, 89f) * sensitivity;

                // ja ist so gewollt das x und y vertauscht sind
                container.AddRotation(Container.Direction.X, angleY);
                container.AddRotation(Container.Direction.Y, angleX);
            }

            _lastPos = new Vector2(mouse.X, mouse.Y);

            if (KeyboardState.IsKeyDown(Keys.Up))
                container.AddRotation(Container.Direction.X, -sensitivity);
            if (KeyboardState.IsKeyDown(Keys.Down))
                container.AddRotation(Container.Direction.X, sensitivity);

            if (KeyboardState.IsKeyDown(Keys.Left))
                container.AddRotation(Container.Direction.Y, -sensitivity);

            if (KeyboardState.IsKeyDown(Keys.Right))
                container.AddRotation(Container.Direction.Y, sensitivity);
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