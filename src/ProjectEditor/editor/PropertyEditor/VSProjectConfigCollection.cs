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
using System.Collections;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// A simple collection to hold VSProjectConfigs. Originally,
	/// we used the (NUnit) ProjectConfigCollection, but the
	/// classes have since diverged.
	/// </summary>
	public class VSProjectConfigCollection : CollectionBase
	{
		public VSProjectConfig this[int index]
		{
			get { return List[index] as VSProjectConfig; }
		}

		public VSProjectConfig this[string name]
		{
			get
			{
				foreach ( VSProjectConfig config in InnerList )
					if ( config.Name == name ) return config;

				return null;
			}
		}

		public void Add( VSProjectConfig config )
		{
			List.Add( config );
		}

		public bool Contains( string name )
		{
			foreach( VSProjectConfig config in InnerList )
				if ( config.Name == name ) return true;

			return false;
		}
	}
}
