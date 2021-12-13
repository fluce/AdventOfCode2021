var input=(await Helper.GetInput()).ToArray();

var points=input.TakeWhile(x=>!string.IsNullOrEmpty(x)).Select(x=>x.Split(',')).Select(x=>(x:int.Parse(x[0]),y:int.Parse(x[1]))).ToArray();
foreach(var p in points) Console.WriteLine(p);

var folds=input.SkipWhile(x=>!string.IsNullOrEmpty(x)).Skip(1).Select(x=>x.Split('=')).Select(x=>x[0]=="fold along x"?(x:int.Parse(x[1]),y:0):(x:0,y:int.Parse(x[1]))).ToArray();

foreach(var f in folds) Console.WriteLine($"folds : {f}");

var iter=0;
foreach(var fold in folds) {
    var newpoints=points.Select(p=>{
        if (fold.y>0 && p.y>fold.y) {
            return (p.x,2*fold.y-p.y);
        }
        if (fold.x>0 && p.x>fold.x) {
            return (2*fold.x-p.x,p.y);
        }
        return p;
    }).ToArray();
    if (iter==0) {
        var c=newpoints.Distinct().Count();
        Console.WriteLine($"count={c}");
    }
    points=newpoints;
}

foreach(var p in points) Console.WriteLine(p);
var max=(x:points.Max(x=>x.x),y:points.Max(x=>x.y));

var tab=new char[max.y+1][];
for(int i=0;i<=max.y;i++) {
    tab[i]=new char[max.x+1];
    for(int j=0;j<=max.x;j++) tab[i][j]=' ';
}
foreach(var p in points) tab[p.y][p.x]='#';
for(int i=0;i<=max.y;i++) Console.WriteLine(new string(tab[i]));

    


