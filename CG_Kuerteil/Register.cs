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
        private Camera camera = new Camera(new(), 640/400f);
        private IDictionary<string, int> vertexArrayIDs = new Dictionary<string, int>();

        public static Register GetRegister()
        {
            if (register == null)
                register = new Register();

            return register;
        }

        public Camera GetCamera()
        {
            return camera;
        }
        public void RegisterCamera(Camera camera)
        {
            this.camera = camera;
        }

        public int GetArrayID(string name)
        {
            return vertexArrayIDs[name];
        }

        public void RegisterArrayID(string name, int arrayID)
        {
            vertexArrayIDs.Add(name, arrayID);
        }
    }
}
