var input=(await Helper.GetInput()).First().Split(",").Select(int.Parse).ToArray();

var max=input.Max();
var min=input.Min();

var minscore=int.MaxValue;
for(var i=min;i<=max;i++)
{
    var score=input.Sum(x=>{
        var d=Math.Abs(x-i);
        return d;
    });
    if (score<minscore) minscore=score;
}



Console.WriteLine($"res={minscore}");

minscore=int.MaxValue;
for(var i=min;i<=max;i++)
{
    var score=input.Sum(x=>{
        var d=Math.Abs(x-i);
        return d*(d+1)/2;
    });
    if (score<minscore) minscore=score;
}

Console.WriteLine($"res={minscore}");
