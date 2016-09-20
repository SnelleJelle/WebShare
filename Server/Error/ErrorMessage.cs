using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.Error
{
    class ErrorMessage : TemplateGenerator
    {
        public int ErrorCode { get; set; }
        private static string errortemplate = @"Server\Error\Error{0}.html";

        public ErrorMessage(int errorCode)
        {
            this.ErrorCode = errorCode;
            templateFile = string.Format(errortemplate, ErrorCode);
        }

        internal Stream getStream()
        {
            string html = File.ReadAllText(templateFile);
            return generateStreamFromString(html);
        }
    }
}
