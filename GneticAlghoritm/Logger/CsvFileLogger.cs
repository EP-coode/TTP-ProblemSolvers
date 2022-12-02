using ProblemSolvers.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Logger;

internal class CsvFileLogger<T> : ILogger<T> where T : ILogable
{
    private StreamWriter? DstStream { get; set; }
    private char separator;
    private object lockTarget = new object();
    private bool firstLine = true;

    public void SetFileDest(string fileDst, char separator = ';')
    {
        FileInfo file = new FileInfo(fileDst);
        file.Directory?.Create();
        DstStream = new StreamWriter(fileDst);
        this.separator = separator;
    }

    public void Flush()
    {
        DstStream?.Flush();
    }

    public void Close()
    {
        DstStream?.Close();
    }

    public void Log(T logable)
    {
        lock (lockTarget)
        {
            if (firstLine)
            {
                DstStream?.WriteLine(logable.GetHeader(separator));
                firstLine = false;
            }
            DstStream?.WriteLine(logable.ToString(separator));
        }
    }
}

