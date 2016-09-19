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
            templateFile = @"Server\Error\ErrorBase.html";
        }

        internal Stream getStream()
        {
            string errorHtml;
            try
            {
                errorHtml = File.ReadAllText(string.Format(errortemplate, ErrorCode));
            }
            catch (Exception e)
            {
                Debug.WriteLine("no template for error: " + ErrorCode);
            }
            errorHtml = "error " + ErrorCode;
            string TemplateHtml = encapsulateInTemplate(errorHtml);
            return GenerateStreamFromString(TemplateHtml);
        }
    }
}
