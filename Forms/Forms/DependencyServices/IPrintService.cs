using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.DependencyServices
{
    public interface IPrintService
    {
        void PrintDocument(string filePath);

        void PrintHtml(string html);

        void SetPrintServiceCallBack(IPrintServiceCallBack callBack);
    }

    public interface IPrintServiceCallBack
    {
        void OnFinish();

        void OnFailed(string errorMsg);
    }
}
