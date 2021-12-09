var input=(await Helper.GetInput())
    .Select(
        x=>x.Select(x=>int.Parse(x.ToString())).ToArray()
    )
    .ToArray();

void Display(int[][] data)
{
    foreach(var l in data)
        Console.WriteLine($"{string.Join("",l.Select(x=>$"{x,2}"))}");
}

void Display2(int[][] data,int[][] bassin, bool val=false)
{
    for(int i=0;i<input.Length;i++) 
        Console.WriteLine($"{string.Join("",data[i].Select((x,j)=>$"{(bassin[i][j]==0?(val?x:"."):"#")}"))}");
}

int GetVal(int[][] data,int x, int y)
{
    if (x<0 || x>= data.Length) return int.MaxValue;
    if (y<0 || y>= data[x].Length) return int.MaxValue;
    return data[x][y];
}

(int dx,int dy)[] adj= new[] { (-1,0),(1,0),(0,-1),(0,1) };

Display(input);

int res=0;
int c=1;

int[][] bassins=new int[input.Length][];
Dictionary<int,int> dico=new Dictionary<int, int>();

for(int i=0;i<input.Length;i++) {
    bassins[i]=new int[input[i].Length];
    for(int j=0;j<input[i].Length;j++)
    {        
        if (input[i][j]<adj.Min(x=>GetVal(input,i+x.dx,j+x.dy))) {
            res+=1+input[i][j];
            dico[c]=1;
            bassins[i][j]=c++;

        }
    }
}

Console.WriteLine($"risk={res}");
Display2(input,bassins);
Console.ReadKey();

bool GetValExceptBassin(int[][] data,int[][] bassin,int x, int y, ref int bassin_id)
{
    if (x<0 || x>= data.Length) return false;
    if (y<0 || y>= data[x].Length) return false;
    if (data[x][y]==9) return false;
    if (bassin[x][y]>0) {
        bassin_id=bassin[x][y];
        return true;
    }
    return false;
}

var iter=0;
int modified=0;

do {
    modified=0;
    var newbassins=bassins.Select(x=>x.ToArray()).ToArray();
    for(int i=0;i<input.Length;i++) {
        for(int j=0;j<input[i].Length;j++)
        {        
            int bassin_id=0;
            if (bassins[i][j]==0 && input[i][j]!=9 && adj.Any(x=>GetValExceptBassin(input,bassins,i+x.dx,j+x.dy, ref bassin_id))) {
                newbassins[i][j]=bassin_id;
                dico[bassin_id]++;
                modified++;
            } else
                newbassins[i][j]=bassins[i][j];
        }
    }
    bassins=newbassins;
    Display2(input,bassins);
    iter++;
} while(iter<100);
//Console.WriteLine(iter);

Display2(input,bassins,true);

foreach(var p in dico.OrderBy(x=>x.Value)) {Console.WriteLine($"{p.Key}={p.Value}");}

var res2=dico.OrderByDescending(x=>x.Value).Take(3).Aggregate(1,(y,x)=>y*x.Value);
Console.WriteLine($"res={res2}");