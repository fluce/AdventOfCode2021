namespace aoclib;
public static class Helper
{
    static HttpClient httpClient=GetHttpClient();

    static HttpClient GetHttpClient() {
        var cli=new HttpClient();
        cli.BaseAddress=new Uri("https://adventofcode.com/");
        cli.DefaultRequestHeaders.Add("Cookie",$"session={Environment.GetEnvironmentVariable("AOCSESSION")}");
        return cli;
    }

    public static async Task<IEnumerable<string>> GetInput(int day=0) 
    {
        if (day==0) day=int.Parse(Regex.Match(Environment.GetCommandLineArgs()[0],@"day(?<day>\d+)").Groups["day"].Value);
        Stream stream;
        var filename=$".input-{day}";
        var cached=false;
        var a=Environment.GetCommandLineArgs();
        if (a.Length>1) {
            filename=a[1];
            cached=true;
        } else {
            filename=$".input-{day}";
            cached=File.Exists(filename);
        }
        if (cached) 
            stream=File.OpenRead(filename);
        else {
            var res=await httpClient.GetAsync($"2021/day/{day}/input");
            stream=await res.Content.ReadAsStreamAsync();
        }
        using var reader=new StreamReader(stream);
        var list=new List<string>();
        while (!reader.EndOfStream)
            list.Add((await reader.ReadLineAsync())!);
        
        if (!cached)
            await File.WriteAllLinesAsync(filename,list);
        return list;
    }

    public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out IEnumerable<T> rest)
    {
        first = seq.First();
        rest = seq.Skip(1);
    }

    public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out IEnumerable<T> rest)
        => (first, (second, rest)) = seq;

    public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out IEnumerable<T> rest)
        => (first, second, (third, rest)) = seq;

    public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out IEnumerable<T> rest)
        => (first, second, third, (fourth, rest)) = seq;

    public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out T fifth, out IEnumerable<T> rest)
        => (first, second, third, fourth, (fifth, rest)) = seq;

    public static void Deconstruct<T>(this T[] seq, out T first, out T second)
        => (first, second) = (seq[0],seq[1]);

    public static void Deconstruct<T>(this T[] seq, out T first, out T second, out T third)
        => (first, second, third) = (seq[0],seq[1],seq[2]);


    public static IEnumerable<(T,int)> Indexed<T>(this IEnumerable<T> seq) => seq.Select((x,i)=>(x,i));

    public static string HighlightIf<T>(this T str, Func<T,bool>? predicate=null) => (predicate?.Invoke(str)??true)?$"\u001b[31m{str}\u001b[0m":$"{str}";
}
