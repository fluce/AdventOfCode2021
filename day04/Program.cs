var input=(await Helper.GetInput(4)).ToArray();

var (draws,rest)=input;
var data=rest.ToArray();
var n=data.Length/6;

var grids=new Grid[n];

foreach(var i in Enumerable.Range(0,n))
    grids[i]=new Grid(data[(i*6+1)..(i*6+6)]);

(byte? drawed, Grid? g) first=(null,null),last=(null,null);

foreach (var d in draws.Split(",").Select(x=>byte.Parse(x)))
{
    foreach(var g in grids) {
        if (g.Draw(d)) {
            last=(d,g);
            if (first.drawed==null) {
                first=(d,g);
                Console.WriteLine($"Bingo ! => {d} {g.Sum()} => {d*g.Sum()}");
                g.Print();
            }
        }
    }
}

Console.WriteLine($"Bingo ! => {last.drawed} {last.g.Sum()} => {last.drawed*last.g.Sum()}");
last.g.Print();


class Grid {
    byte?[][] data;

    private bool won=false;

    public Grid(string[] lines) 
    {
        data=new byte?[lines.Length][];
        foreach(var l in lines.Select((x,i)=>(x,i))) {
            data[l.i]=l.x.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(byte.Parse).Cast<byte?>().ToArray();
        }
    }

    public void Print()
    {
        Console.WriteLine();
        foreach(var l in data) {
            Console.WriteLine(string.Join(" ",l.Select(x=>$"{x,2}{(x==null?"*":" ")} ")));
        }
    }

    public bool Draw(byte b)
    {
        if (!won)
            foreach(var l in data)
                foreach(ref var c in l.AsSpan())
                    if (c==b) {
                        c=null;
                        won=CheckRow() || CheckColumn();
                        return won;                    
                    }                 
        return false;
    }

    public bool CheckRow() => data.Any(x=>x.All(x=>x==null));

    public bool CheckColumn() => Enumerable.Range(0,data[0].Length).Any(i=>data.All(x=>x[i]==null));

    public long Sum() => data.Sum(x=>x.Where(x=>x!=null).Sum(x=>x.Value));
}





