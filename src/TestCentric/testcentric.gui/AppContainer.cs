// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    /// <summary>
    /// AppContainer acts as the container for our components, providing 
    /// them with a Site from which services may be acquired.
    /// </summary>
    public class AppContainer : Container
    {
        public AppContainer()
        {
            // We use a single root-level service container for the app
            _services = new ServiceContainer();
            // Add the service containter to itself as a service!
            // This allows components to get access to the ServiceContainer
            // service in order to add services themselves.
            _services.AddService( typeof( IServiceContainer ), _services );
        }
 
        ServiceContainer _services;
        public IServiceContainer Services 
        {
            get { return _services; }
        }

        // Override GetService so that any components in this
        // container get access to services in the associated
        // ServiceContainer.
        protected override object GetService(Type service)
        {
            object s = _services.GetService(service);
            if (s == null) s = base.GetService(service);
            return s;
        }

        public static ISite GetSite( Control control )
        {
            while( control != null && control.Site == null )
                control = control.Parent;
            return control == null ? null : control.Site;
        }

        public static IContainer GetContainer( Control control )
        {
            ISite site = GetSite( control );
            return site == null ? null : site.Container;
        }

        public static object GetService( Control control, Type service )
        {
            ISite site = GetSite( control );
            return site == null ? null : site.GetService( service );
        }

        public static AppContainer GetAppContainer( Control control )
        {
            return GetContainer( control ) as AppContainer;
        }
    } 
}
