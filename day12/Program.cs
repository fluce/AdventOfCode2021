var input=(await Helper.GetInput()).Select(x=>{ (String a, String b, IEnumerable<String> _)=x.Split('-'); return (a,b);}).ToArray();

Dictionary<String,(bool isBig,List<String> list)> adjList=new();

foreach(var (a,b) in input) {
    List<string> list;
    if (adjList.TryGetValue(a,out var item)) 
        list=item.list;
    else
    {
        list=new List<string>();
        adjList[a]=(char.IsUpper(a[0]),list);
    }
    if (!list.Contains(b)) list.Add(b);

    if (adjList.TryGetValue(b,out var item2)) 
        list=item2.list;
    else
    {
        list=new List<string>();
        adjList[b]=(char.IsUpper(b[0]),list);
    }
    if (!list.Contains(a)) list.Add(a);

}

foreach(var (k,(isBig,l)) in adjList) Console.WriteLine($"{k}=>{isBig} {string.Join(",",l)}");

string current="start";

void FindPath(string start, string end, Dictionary<string,int> visited, Stack<string> path, List<List<string>> allPaths, bool part2=false, string smallCave=null)
{
    visited[start]++;
    if (part2) {
//        Console.WriteLine($"{start}=>{end} {smallCave} {string.Join(",",visited.Where(x=>x.Value>0).Select(x=>x.Key).OrderBy(x=>x))} {string.Join(" > ",path.Reverse())}");
//        Console.ReadKey();    
    }
    path.Push(start);
    if (start==end) {
        allPaths.Add(path.Reverse().ToList());
        Console.WriteLine($"path={string.Join(" > ",path.Reverse())}");
    } else {
        var n=adjList[start];
        foreach(var l in n.list)
        {
            //if (part2) Console.WriteLine($" Candidate {l}");
            if ((adjList[l].isBig || visited[l]==0)) {
                FindPath(l, end, visited, path, allPaths, part2, smallCave);
            }
            else {
                if (part2 && l!="start" && l!="end" && !adjList[l].isBig && smallCave==null && visited[l]<=1)
                    FindPath(l, end, visited, path, allPaths, part2, l);
            }
        }
    }
    path.Pop();
    visited[start]--;
}

List<List<string>> allPaths=new();
FindPath("start","end",adjList.ToDictionary(x=>x.Key,x=>0), new Stack<string>(),allPaths);
Console.WriteLine($"total={allPaths.Count}");
FindPath("start","end",adjList.ToDictionary(x=>x.Key,x=>0), new Stack<string>(),allPaths,true);

foreach(var a in allPaths.Select(path=>string.Join(" > ",path)).OrderBy(x=>x))
    Console.WriteLine(a);
Console.WriteLine($"total={allPaths.Select(path=>string.Join(" > ",path)).Distinct().Count()}");