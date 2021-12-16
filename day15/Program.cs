var input=(await Helper.GetInput()).Select(x=>x.Select(y=>int.Parse(y.ToString())).ToArray()).ToArray();

var adj=new(int x, int y)[] { (-1,0),(1,0),(0,-1),(0,1) };

IEnumerable<((int,int),int)> GetAdh(int x,int y, Func<int,int,int?> getVal) {
    
    foreach(var a in adj) {
        var p=getVal(x+a.x,y+a.y);
        if (p!=null) {
//            Console.WriteLine($"adj to {(x,y)} : {((x+a.x,y+a.y),int.Parse(p.ToString()))}");
            yield return ((x+a.x,y+a.y),p.Value);
        }
    }
}

var c=0;

int? dijkstra((int,int) source, (int,int) target, Func<int,int,int?> getVal) 
{

    Dictionary<(int x,int y),bool> visited=new();
    PriorityQueue<(int x,int y),int> toVisit=new();

    toVisit.Enqueue(source,0);
    visited[source]=true;

    while (toVisit.TryDequeue(out var node, out var d)) {

        if ((c++%100000)==0)
            Console.WriteLine($"node at {node} d={d}");

        if (node==target) return d;

        visited[node]=true;

        foreach(var (n,w) in GetAdh(node.x,node.y, getVal)) {
            if (!visited.ContainsKey(n))
                toVisit.Enqueue(n,d+w);
        }

    }
    return null;
}

int? astar((int,int) source, (int,int) target, Func<int,int,int?> getVal, Func<(int x, int y), int> h=null) 
{
    if (h==null) h=(x)=>0;

    PriorityQueue<(int x,int y),int> toVisit=new();
    Dictionary<(int x,int y),bool> toVisitDico=new();

    Dictionary<(int x,int y),int> gScore=new();

    toVisit.Enqueue(source,h(source));
    toVisitDico[source]=true;
    gScore[source]=0;

    while (toVisit.TryDequeue(out var node, out var d)) {

        toVisitDico.Remove(node);

        if ((c++%100000)==0)
            Console.WriteLine($"node at {node} d={d}");

        if (node==target) return gScore[node];

        foreach(var (n,w) in GetAdh(node.x,node.y, getVal)) {
            int t_gscore=gScore[node]+w;
            if (!gScore.TryGetValue(n,out var sn)) sn=int.MaxValue;
            if (t_gscore<sn) {
                gScore[n]=t_gscore;
                if (!toVisitDico.ContainsKey(n))
                {
                    toVisit.Enqueue(n,t_gscore+h(n));
                    toVisitDico[n]=true;
                }
            }
        }

    }
    return null;
}

int? GetVal(int x, int y)
{
    if (x<0 || x>= input.Length) return null;
    if (y<0 || y>= input[x].Length) return null;
    return input[x][y];
}

var source=(0,0);
var target=(input[0].Length-1,input.Length-1);
var size=(x:input[0].Length,y:input.Length);
var mss=size.x+size.y;

int? shortest=dijkstra(source,target, GetVal);
Console.WriteLine(shortest);

int? GetVal2(int x, int y)
{
    if (x<0 || x>= size.x*5) return null;
    if (y<0 || y>= size.y*5) return null;

    int i=x/size.x; int ir=x%size.x;
    int j=y/size.y; int jr=y%size.y;

    return 1+(input[ir][jr]+i+j-1)%9;
}

source=(0,0);
target=(input[0].Length*5-1,input.Length*5-1);
mss=size.x*5+size.y*5;

shortest=astar(source,target, GetVal2,((int x, int y) c)=>mss-c.x-c.y);
Console.WriteLine(shortest);
