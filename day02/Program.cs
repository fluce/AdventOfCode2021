var input=(await Helper.GetInput(2)).Select(x=>x.Split(' ')).Where(x=>x.Length==2).Select(x=>(cmd:x[0],val:int.Parse(x[1]))).ToArray();

long hor=0;
long depth=0;
foreach(var x in input)
{
    switch(x.cmd) {
        case "forward": hor+=x.val; break;
        case "down": depth+=x.val; break;
        case "up": depth-=x.val; break;
        default: break;
    }
}

Console.WriteLine($"hor={hor} depth={depth} res={hor*depth}");

hor=0;
depth=0;
long aim=0;
foreach(var x in input)
{
    switch(x.cmd) {
        case "forward": hor+=x.val; depth+=aim*x.val; break;
        case "down": aim+=x.val; break;
        case "up": aim-=x.val; break;
        default: break;
    }
}

Console.WriteLine($"hor={hor} depth={depth} res={hor*depth}");