using Microsoft.AspNetCore.ResponseCompression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace figma.CustomHandler
{
    public class CustomCompressionProvider : ICompressionProvider
    {
        public string EncodingName => "mycustomcompression";
        public bool SupportsFlush => true;

        public Stream CreateStream(Stream outputStream)
        {
            // Create a custom compression stream wrapper here
            return outputStream;
        }
    }
}
