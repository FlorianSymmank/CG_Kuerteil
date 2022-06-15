using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class Register
    {
        private static Register register = null;

        private IDictionary<string, VertexData> vertexArrayData = new Dictionary<string, VertexData>();

        public static Register GetRegister()
        {
            if (register == null)
                register = new Register();

            return register;
        }

        public bool TryGetVertexArray(string name, out VertexData vertexData)
        {
            return vertexArrayData.TryGetValue(name, out vertexData);
        }

        public void RegisterVertexArray(string name, VertexData vertexData)
        {
            vertexArrayData.Add(name, vertexData);
        }

        public void DestroyAllVertexArray()
        {
            foreach (var vertexData in vertexArrayData.Values)
            {
                GL.DeleteBuffer(vertexData.BufferID);
                GL.DeleteVertexArray(vertexData.ArrayID);
            }
        }

        public void UnRegister<T>()
        {
            registerDict.Remove(typeof(T));
        }

        public void RegisterObject(Object a)
        {
            registerDict.Add(a.GetType(), a);
        }

        public T Get<T>()
        {
            registerDict.TryGetValue(typeof(T), out object registered);
            return (T)registered;

        }

        private Dictionary<Type, object> registerDict = new Dictionary<Type, object>();
    }
}
