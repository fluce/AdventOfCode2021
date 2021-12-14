var input=(await Helper.GetInput()).ToArray();

var formula=input.First().ToArray();
foreach(var p in formula) Console.WriteLine(p);

var rules=input.Skip(2).Select(x=>Regex.Match(x,@"(?<a>.)(?<b>.) -> (?<c>.)")).ToDictionary(x=>(a:x.Groups["a"].Value[0],b:x.Groups["b"].Value[0]),x=>x.Groups["c"].Value[0]);
foreach(var (k,v) in rules) Console.WriteLine($"{k}=>{v}");

Dictionary<char,int> CalcAfter(char[] input, int iters) {

    var list=new LinkedList<char>(input);

    for(int i=0;i<iters;i++) {

        var current=list.First;
        var next=current.Next;

        do {
        if (rules.TryGetValue((current.Value,next.Value),out var c))
            list.AddAfter(current,c);

        current=next;
        next=current.Next;
        }
        while (next!=null);
        //Console.WriteLine($"{string.Join("",list)}");

    }
    return list.GroupBy(c=>c).ToDictionary(x=>x.Key,x=>x.Count());

}

var dico=CalcAfter(formula,10).OrderBy(x=>x.Value).ToArray();
var res=dico.Last().Value-dico.First().Value;
Console.WriteLine($"Result = {res}");

var rules2=rules.ToDictionary(x=>x.Key,x=>new Dictionary<char,long>[40]);

void Increment(Dictionary<char,long> dico, char key, long value)
{
    if (dico.ContainsKey(key))
        dico[key]+=value;
    else
        dico[key]=value;
}

for(int i=0;i<40;i++)
{
    foreach(var r in rules) {
        var ri=rules2[r.Key][i]=new Dictionary<char,long>();
        if (i==0) {
            Increment(ri,r.Key.a,1);
            Increment(ri,r.Key.b,1);
            Increment(ri,r.Value,1);
        } else {
            foreach(var (r_,c_) in rules2[(r.Key.a,r.Value)][i-1])
                Increment(ri,r_,c_);
            foreach(var (r_,c_) in rules2[(r.Value,r.Key.b)][i-1])
                Increment(ri,r_,c_);
            Increment(ri,r.Value,-1);
        }
    }   
}
/*
foreach(var r in rules) {
    var d1=CalcAfter(new[] {r.Key.a,r.Key.b},1);
    var d2=CalcAfter(new[] {r.Key.a,r.Key.b},2);
    var d3=CalcAfter(new[] {r.Key.a,r.Key.b},3);
    var d4=CalcAfter(new[] {r.Key.a,r.Key.b},4);
    var d5=CalcAfter(new[] {r.Key.a,r.Key.b},5);
    var d6=CalcAfter(new[] {r.Key.a,r.Key.b},6);
    Console.WriteLine($"A {r} => {string.Join(", ",d1.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}   {string.Join(", ",d2.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}   {string.Join(", ",d3.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}   {string.Join(", ",d4.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}   {string.Join(", ",d5.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}   {string.Join(", ",d6.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))}");

    Console.WriteLine($"B {r} => {string.Join("   ",rules2[r.Key].Take(6).Select(y=>string.Join(", ",y.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))))}");
}*/

foreach(var r in rules) 
    Console.WriteLine($"B {r} => {string.Join("   ",rules2[r.Key].Skip(38).Select(y=>string.Join(", ",y.OrderBy(x=>x.Key).Select(x=>$"{x.Key}={x.Value}"))))}");

var list=new LinkedList<char>(formula);

var current=list.First;
var next=current.Next;
var res2=new Dictionary<char,long>();
do {
    foreach(var x in rules2[(current.Value,next.Value)][39]) {
        Increment(res2,x.Key,x.Value);
    }

    current=next;
    next=current.Next;
}
while (next!=null);

Console.WriteLine(string.Join(", ",res2.OrderBy(x=>x.Value).Select(x=>$"{x.Key}={x.Value}")));
var dico2=res2.OrderBy(x=>x.Value).ToArray();
var res3=dico2.Last().Value-dico2.First().Value;
Console.WriteLine($"Result = {res3}");
