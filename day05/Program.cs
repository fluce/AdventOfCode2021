var input=(await Helper.GetInput(5))
    .Select(x=>System.Text.RegularExpressions.Regex.Match(x,@"^(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)"))
    .Select(m=>(p1:(x:int.Parse(m.Groups["x1"].Value),y:int.Parse(m.Groups["y1"].Value)),p2:(x:int.Parse(m.Groups["x2"].Value),y:int.Parse(m.Groups["y2"].Value))))
    .ToArray();

var dico=new ConcurrentDictionary<(int x,int y),int>();

foreach (var l in input.Where(p=>p.p1.x==p.p2.x)) {
    Console.WriteLine(l);
    for(int y=Math.Min(l.p1.y,l.p2.y);y<=Math.Max(l.p1.y,l.p2.y);y++)
        dico.AddOrUpdate((l.p1.x,y),(e)=>1,(e,v)=>v+1);
}

foreach (var l in input.Where(p=>p.p1.x!=p.p2.x && p.p1.y==p.p2.y)) {
    Console.WriteLine(l);
    for(int x=Math.Min(l.p1.x,l.p2.x);x<=Math.Max(l.p1.x,l.p2.x);x++)
        dico.AddOrUpdate((x,l.p1.y),(e)=>1,(e,v)=>v+1);
}

foreach(var d in dico.Where(x=>x.Value>=2))
    Console.WriteLine($"{d.Key} : {d.Value}");


Console.WriteLine($"res = {dico.Count(x=>x.Value>=2)}");


foreach (var l in input.Where(p=>p.p1.x!=p.p2.x && p.p1.y!=p.p2.y)) {
    Console.WriteLine(l);
    var len=Math.Max(l.p1.x,l.p2.x)-Math.Min(l.p1.x,l.p2.x);
    var start=(x:0,y:0);
    int pente=0;
    if (l.p1.x<l.p2.x) {
        start=l.p1;
        pente=l.p1.y>l.p2.y?-1:1;
    } else {
        start=l.p2;
        pente=l.p2.y>l.p1.y?-1:1;
    }
    for(int i=0;i<=len;i++) {
        dico.AddOrUpdate((start.x+i,start.y+i*pente),(e)=>1,(e,v)=>v+1);
    }
}

Console.WriteLine($"res = {dico.Count(x=>x.Value>=2)}");

