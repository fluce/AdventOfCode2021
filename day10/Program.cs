var input=(await Helper.GetInput()).ToArray();

long[] scores=new[] {1197L,57L,25137L,3L};

long Check(string data, out string neededToComplete)
{
    neededToComplete="";
    Stack<char> stack=new();

    foreach(var c in data)
    {
        if ("{[<(".Contains(c)) 
            stack.Push(c);
        else {
            var p=stack.Pop();
            var idx="}]>)".IndexOf(c);
            if ("{[<(".IndexOf(p)!=idx)
                return scores[idx];
        }
    }

    while (stack.Count>0) {
        var p=stack.Pop();
        neededToComplete+="}]>)"["{[<(".IndexOf(p)];
    }

    return 0;
}


List<long> scores2=new();
var res=input.Sum(data=>{
    var i=Check(data,out var remaining);
    Console.WriteLine($"{data} => {i} {remaining}");
    long score=i==0?remaining.Aggregate(0L,(s,c)=>s*5+c switch { ')'=>1, ']'=>2, '}'=>3, _=>4 }):0;
    Console.WriteLine($"{data} => {i} {remaining} {score}");
    if (i==0) scores2.Add(score);
    return i;
});
Console.WriteLine($"res={res}");

var res2=scores2.OrderBy(x=>x).Skip(scores2.Count()>>1).First();

Console.WriteLine($"res2={res2}");



