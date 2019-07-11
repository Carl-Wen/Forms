using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.DependencyServices
{
    public interface IPdfService
    {
        string ConvertHtmlToPDF(string html, string fileName);
    }
}
