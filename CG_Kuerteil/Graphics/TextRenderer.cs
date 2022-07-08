using CG_Kuerteil.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace CG_Kuerteil.Graphics
{
    public class TextRenderer
    {

        private static readonly float[] _vertices =
       {
            // Position     Texture coordinates
             0f,  0f, 0.0f, 0.0f, 1.0f,
             0f,  1f, 0.0f, 0.0f, 0.0f,
             1f,  1f, 0.0f, 1.0f, 0.0f,
             1f,  0f, 0.0f, 1.0f, 1.0f
        };

        private static readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private IDictionary<char, CharUnit> chars = new Dictionary<char, CharUnit>();

        public TextRenderer()
        {
            chars = initializeChars();
        }

        public int Columns = 80;
        public int Rows = 80;

        public void RenderText(string text, int column = 0, int row = 0)
        {
            RenderText(text, column, row, Color4.White);
        }

        public void RenderText(string text, int column, int row, Color4 color)
        {
            row += 1;

            Shader shader = Register.GetRegister().Get<Shader>();
            float offsetC = 1f / (Columns / 2);
            float offsetR = 1f / (Rows / 2);

            float x = -1 + (offsetC * column);
            float y = 1 - (offsetR * row);

            Matrix4 model = Matrix4.CreateScale(offsetC, offsetR, 0) * Matrix4.CreateTranslation(new(x, y, 0));

            foreach (char c in text)
            {
                CharUnit charUnit = chars[c];
                charUnit.Use();

                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                shader.SetInt("mode", 2);
                shader.SetVector4("objectColor", (Vector4)color);
                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", Matrix4.Identity);
                shader.SetMatrix4("projection", Matrix4.Identity);

                GL.BindVertexArray(charUnit.vertexArrayObject);
                GL.DrawElements(PrimitiveType.Triangles, charUnit.Count, DrawElementsType.UnsignedInt, 0);
                model *= Matrix4.CreateTranslation(new(offsetC, 0, 0));

                GL.Disable(EnableCap.Texture2D);
                GL.Disable(EnableCap.Blend);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "<Ausstehend>")]
        private IDictionary<char, CharUnit> initializeChars()
        {
            Shader _shader = Register.GetRegister().Get<Shader>();

            IDictionary<char, CharUnit> chars = new Dictionary<char, CharUnit>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Available Chars:");
            // from ' ' to 'þ' 
            // https://www.fileformat.info/info/charset/UTF-8/list.htm
            for (int i = 32; i < 255; i++)
            {
                char c = (char)i;
                int handle = GL.GenTexture();

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, handle);

                Bitmap bitmap = createCharBMP(c);
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                // up/downscaling
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                // wrapping
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                // mipmaps 
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                int _vertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(_vertexArrayObject);

                int _vertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

                int _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

                var vertexLocation = _shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                CharUnit charUnit = new CharUnit()
                {
                    Char = c,
                    vertexArrayObject = _vertexArrayObject,
                    Count = _vertices.Length,
                    Handle = handle
                };

                chars.Add(c, charUnit);
                sb.AppendLine($"{c} {i}");
            }

            //Logger.Log(sb.ToString());
            return chars;
        }

        private int FontSize = 250;
        private string FontName = "Consolas";
        private Color TextColor = Color.White;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "<Ausstehend>")]
        private Bitmap createCharBMP(char c)
        {
            int width = FontSize - (int)(FontSize * 0.05f);
            int height = FontSize + (int)(FontSize * 0.5f);

            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);

            Font myFont = new Font(FontName, FontSize); // fontsize = pixels
            PointF rect = new PointF(0, 0);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawString(c.ToString(), myFont, new SolidBrush(Color.White), rect);

            return bmp;
        }
    }
}
