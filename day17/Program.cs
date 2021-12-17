var input=(await Helper.GetInput()).First();

var m=Regex.Match(input,@"x=(?<x1>-?\d+)\.\.(?<x2>-?\d+), y=(?<y1>-?\d+)\.\.(?<y2>-?\d+)");

var X=(min:int.Parse(m.Groups["x1"].Value),max:int.Parse(m.Groups["x2"].Value));
var Y=(min:int.Parse(m.Groups["y1"].Value),max:int.Parse(m.Groups["y2"].Value));

Console.WriteLine($"{X} - {Y}");

var velocity=(dx:6,dy:9);
var theMaxAlt=0;
var count=0;

for (int i=-1000;i<1000;i++)
    for (int j=-1000;j<1000;j++) {
        velocity=(i,j);
        var res=CalcHeight(velocity,out var maxAlt);
        if (res==0) {
            count++;
            Console.WriteLine($"{velocity} => {maxAlt}");
            if (maxAlt>theMaxAlt) theMaxAlt=maxAlt;
        }
    }
Console.WriteLine($"{theMaxAlt} {count}");


int? CalcHeight((int dx,int dy) velocity, out int maxAlt) {

    var pos=(x:0,y:0);

    maxAlt=0;

    while (pos.x<=X.max && pos.y>=Y.min) {
        if (pos.x>=X.min && pos.y<=Y.max) {
            return 0;
        }
        (pos,velocity)=Next(pos,velocity);
        //Console.WriteLine($"pos: {pos}  velocity: {velocity}");
        if (pos.y>maxAlt) maxAlt=pos.y;
    }
    return pos.x<X.min?-1:1;
}

((int x,int y),(int dx,int dy)) Next((int x,int y) pos,(int dx,int dy) velocity) 
    => (
        (pos.x+velocity.dx,pos.y+velocity.dy),
        (velocity.dx switch { >0 => velocity.dx-1, <0=>velocity.dx+1, _ => 0 }, velocity.dy-1 )
    );
