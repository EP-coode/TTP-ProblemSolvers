using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.Logger;

internal class CsvFileLogger : ILogger
{
    private StreamWriter DstStream { get; set; }
    private char separator;
    private object lockTarget = new object();

    public void SetFileDest(string fileDst, string[] fileHeaders, char separator = ';')
    {
        FileInfo file = new FileInfo(fileDst);
        file.Directory.Create();
        DstStream = new StreamWriter(fileDst);
        DstStream.WriteLine(string.Join(separator, fileHeaders));
        this.separator = separator;
    }

    public void Flush()
    {
        DstStream.Flush();
    }

    public void Close()
    {
        DstStream.Close();
    }

    public void Log(string[] data)
    {
        lock (lockTarget)
        {
            DstStream.WriteLine(string.Join(separator, data));
        }
    }
}

