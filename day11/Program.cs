var input=(await Helper.GetInput())
    .Select(
        x=>x.Select(x=>int.Parse(x.ToString())).ToArray()
    )
    .ToArray();

void Display(int[][] data, string title=null)
{
    Console.WriteLine(title);
    foreach(var l in data)
        Console.WriteLine($"{string.Join("",l.Select(x=>$"{(x==0?'.':x.ToString()),2}"))}");
    Console.WriteLine();
}

int GetVal(int[][] data,int x, int y)
{
    if (x<0 || x>= data.Length) return int.MinValue;
    if (y<0 || y>= data[x].Length) return int.MinValue;
    return data[x][y];
}

void SetVal(int[][] data,int x, int y, Transform transform)
{
    if (x<0 || x>= data.Length) return;
    if (y<0 || y>= data[x].Length) return;
    transform(x,y,ref data[x][y]);
}


(int dx,int dy)[] adj= new[] { (-1,0),(1,0),(0,-1),(0,1),(-1,-1),(1,-1),(-1,1),(1,1) };

int[][] ForEachDup(int[][] input, Func<int,int,int,int> transform)
{
    int[][] result=new int[input.Length][];
    for(int i=0;i<input.Length;i++) {
        result[i]=new int[input[i].Length];
        for(int j=0;j<input[i].Length;j++)
        {        
            result[i][j]=transform(i,j,input[i][j]);
        }
    }
    return result;
}

void ForEach(int[][] input, Transform transform)
{
    int[][] result=new int[input.Length][];
    for(int i=0;i<input.Length;i++) {
        result[i]=new int[input[i].Length];
        for(int j=0;j<input[i].Length;j++)
        {        
            transform(i,j,ref input[i][j]);
        }
    }
}

int[][] Step(int[][] input, out int total_count)
{
    ForEach(input,(int i,int j,ref int val)=>val++);
//    Display(input,$"Part 1");
    int count;
    total_count=0;
    int cc=0;
    var flashed=ForEachDup(input,(i,j,v)=>0);
    do {
//        Display(input,$"Part 2 : iter {cc++}");
//        Display(flashed,"flashed");
        count=0;
        var newinput=ForEachDup(input,(i,j,v)=>v);
        ForEach(input,(int i,int j,ref int val)=>{
            if (val>=10 && flashed[i][j]==0) {
                count++;
//                Display(newinput,$"Flashing {i} {j}");
                SetVal(flashed,i,j,(int i2,int j2,ref int val2)=>val2=1);
                foreach(var dd in adj) {
                    SetVal(newinput,i+dd.dx,j+dd.dy,(int i2,int j2, ref int val2)=>{
                        val2++;
//                        Display(newinput,$" Upping {i2} {j2} {val2}");                        
                    });
                }
//                Console.ReadKey();
            }
        });
        input=newinput;
//        Display(input,$"Part 2 : iter {cc} => count={count}");
//        Console.ReadKey();
        total_count+=count;
    } while (count!=0);
    ForEach(input,(int i,int j,ref int val)=>{if (val>9) val=0;});
    return input;
}

Display(input);
int count=0;
for(int i=1;i<100000;i++) {
    input=Step(input, out int step_count);
    count+=step_count;
    if (i==100)
        Display(input,$"After step {i} : count={count}");
    if (step_count==100)
    {
        Display(input,$"After step {i} : count={count}");
        break;
    }
}

delegate void Transform(int i,int j,ref int val);

