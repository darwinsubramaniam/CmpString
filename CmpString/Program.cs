using System.Diagnostics;
using Csv;

(string, string)[] stringToCompare =
{
    ("mA" , "m"),
    ("MV" , "M"),
    ("Gs", "G")
};


string[] columnName = { "runNum", "StrCmp (ms)", "SpanCmp (ms)", "Diff (ms)" };
List<string[]> datas = new List<string[]>();
var loopCounter = 0;

while(loopCounter < 1000)
{
    loopCounter++;
    (TimeSpan, TimeSpan) timing;
    foreach (var pair in stringToCompare)
    {
        Stopwatch stopwatch1 = Stopwatch.StartNew();
        var result1 = strncmp(pair.Item1, pair.Item2, 1);
        stopwatch1.Stop();
        timing.Item1 = stopwatch1.Elapsed;
        Debug.WriteLine(stopwatch1.Elapsed);

        Stopwatch stopwatch2 = Stopwatch.StartNew();
        var result2 = strnSpancmp(pair.Item1, pair.Item2, 1);
        stopwatch2.Stop();
        timing.Item2 = stopwatch2.Elapsed;
        Debug.WriteLine(stopwatch2.Elapsed);

        if(result1 != result2)
        {
            throw new Exception("Logic Out");
        }
        var diff = timing.Item2 - timing.Item1;
        Debug.WriteLine($"Difference SpanCmp to CmpStr = {diff}");

        var data = new Data { runNum = loopCounter, strCmp = timing.Item1, spanCmp = timing.Item2, diff = diff };

        datas.Add(data.toStringArray());
    }
}

var csv = CsvWriter.WriteToText(columnName, datas, ',');
File.WriteAllText("DATA.csv", csv);




int strncmp(string str1, string str2, int len)
{
    for (int i = 0; i < len; i++)
    {
        if (str1.Substring(i, 1) != str2.Substring(i, 1))
            return -1;
    }
    return 0;
}


int strnSpancmp(string str1, string str2, int len)
{
    ReadOnlySpan<char> spanStr1 = str1.AsSpan();
    ReadOnlySpan<char> spanStr2 = str2.AsSpan();

    for (int i = 0; i < len; i++)
    {
        if (spanStr1[i] != spanStr2[i])
        {
            return -1;
        }
    }
    return 0;
}

class Data
{
    public int runNum;
    public TimeSpan strCmp;
    public TimeSpan spanCmp;
    public TimeSpan diff;

    public string[] toStringArray()
    {
        string[] result = { runNum.ToString(), strCmp.TotalMilliseconds.ToString(), spanCmp.TotalMilliseconds.ToString(), diff.TotalMilliseconds.ToString() };
        return result;
    }
}