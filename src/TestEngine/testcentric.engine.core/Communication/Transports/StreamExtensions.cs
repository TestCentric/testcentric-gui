// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace TestCentric.Engine.Communication.Transports
{
    /// <summary>
    /// Extension Methods for use with PipeStreams.
    /// </summary>
    public static class StreamExtensions
    {
        public static T ReadObject<T>(this Stream stream)
        {
            return (T)stream.ReadObject();
        }

        public static object ReadObject(this Stream stream)
        {
            return new BinaryFormatter().Deserialize(stream);
        }

        public static void WriteObject(this Stream stream, object graph)
        {
            new BinaryFormatter().Serialize(stream, graph);
        }
    }
}
#endif
