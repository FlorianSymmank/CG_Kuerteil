using CG_Kuerteil.Data;
using CG_Kuerteil.Graphics;
using CG_Kuerteil.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_Kuerteil
{
    // Tutorial https://opentk.net/learn/index.html
    public class Window : GameWindow
    {
        private Shader _shader;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private Diagramm _diagramm;
        private TextRenderer _textRenderer;

        public delegate void Notify(Window window);
        public event Notify OnLoaded;

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

            _textRenderer = new TextRenderer();
            Register.GetRegister().RegisterObject(_textRenderer);

            OnLoaded?.Invoke(this);
        }

        public void SetDiagramm(Diagramm diagramm)
        {
            _diagramm = diagramm;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Bind the shader
            _shader.Use();

            _diagramm?.Render();

            _textRenderer.Rows = 20;
            _textRenderer.Columns = 20;
            _textRenderer.RenderText(_diagramm.Title, 10 - (_diagramm.Title.Length / 2), 1, Color4.Bisque);

            _textRenderer.Rows = 80;
            _textRenderer.Columns = 80;
            string fps = string.Format("fps: {0:0.00}", 1 / e.Time);
            _textRenderer.RenderText(fps, 0, 0);

            RenderTextLegend();

            SwapBuffers();
        }

        private void RenderTextLegend()
        {
            int seriesCount = _diagramm.DataSeries.Count;

            if (seriesCount <= 0) return;

            int datapointsPerSeries = _diagramm.DataSeries[0].dataPoints.Count;

            int rowsToBeRendered = seriesCount * datapointsPerSeries + seriesCount * 2 + 1;

            _textRenderer.Rows = rowsToBeRendered * 3;
            _textRenderer.Columns = rowsToBeRendered * 4;

            int row = _textRenderer.Rows - rowsToBeRendered;

            _textRenderer.RenderText("Legende:", 0, row);
            row++;

            foreach (Series s in _diagramm.DataSeries)
            {
                _textRenderer.RenderText($"Series: {s.Description}", 0, row, s.Color);
                row++;

                foreach (DataPoint dp in s.dataPoints)
                {
                    _textRenderer.RenderText($"Title: {dp.Title} Value: {dp.Value}", 0, row, dp.Color);
                    row++;
                }

                _textRenderer.RenderText("", 0, row, s.Color);
                row++;
            }
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
                _diagramm?.AddRotation(Container.Direction.X, angleY);
                _diagramm?.AddRotation(Container.Direction.Y, angleX);
            }

            _lastPos = new Vector2(mouse.X, mouse.Y);

            if (KeyboardState.IsKeyDown(Keys.Up))
                _diagramm?.AddRotation(Container.Direction.X, -sensitivity);
            if (KeyboardState.IsKeyDown(Keys.Down))
                _diagramm?.AddRotation(Container.Direction.X, sensitivity);

            if (KeyboardState.IsKeyDown(Keys.Left))
                _diagramm?.AddRotation(Container.Direction.Y, -sensitivity);

            if (KeyboardState.IsKeyDown(Keys.Right))
                _diagramm?.AddRotation(Container.Direction.Y, sensitivity);
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