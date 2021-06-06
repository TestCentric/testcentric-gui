using System.Collections.Generic;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace TestCentric.Engine.Services.Fakes
{
    public class FakeExtensionService : FakeService, IExtensionService
    {
        public IEnumerable<IExtensionPoint> ExtensionPoints
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public IEnumerable<IExtensionNode> Extensions
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public void EnableExtension(string typeName, bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IExtensionNode> GetExtensionNodes(string path)
        {
            throw new System.NotImplementedException();
        }

        public IExtensionPoint GetExtensionPoint(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
