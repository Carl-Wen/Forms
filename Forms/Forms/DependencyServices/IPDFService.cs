using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms.DependencyServices
{
    public interface IPdfService
    {
        Task<string> ConvertHtmlToPDF(string html, string fileName);
    }
}
