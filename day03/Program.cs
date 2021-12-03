var input=(await Helper.GetInput(3)).Select(x=>x.Select(y=>y=='0'?0:1).ToArray()).ToArray();

var n=input.Length/2;
var m=input[0].Length;

(long[],long[]) GetCommon(IEnumerable<int[]> d) {
    var c=new long[m];
    var count=d.Count();
    var f=(count%2==0);
    var n=d.Count()>>1;
    foreach(var x in d)
    {
        foreach(var y in x.Select((a,i)=>(a,i)))
            c[y.i]+=y.a;
    }
    Console.WriteLine($"### {n} / {string.Join(" ",c.Select(x=>x.ToString()))}\n");
    return (c.Select((x,i)=>(x==n && f)?1:(x>n)?1L:0L).ToArray(),c.Select((x,i)=>(x==n && f)?0:(x<=n)?1L:0L).ToArray());
}
var (mostCommon,leastCommon)=GetCommon(input);

var r1=mostCommon.Select((x,i)=>x<<(m-i-1)).Sum();
var r2=leastCommon.Select((x,i)=>x<<(m-i-1)).Sum();

Console.WriteLine($"r1={r1} r2={r2} => {r1*r2}");


var dataMost=input;
var dataLeast=input;

(mostCommon,leastCommon)=GetCommon(input);

for (int j = 0;j<m;j++) {

    Console.WriteLine($"\n{j}  {string.Join(" ",mostCommon.Select(x=>x.ToString()))}\n");

    foreach(var d in dataMost) Console.WriteLine("   "+string.Join(" ",d.Select(x=>x.ToString())));

    if (dataMost.Length>1) {        
        dataMost=dataMost.Where(x=>x[j]==mostCommon[j]).ToArray();
    }
    (mostCommon,leastCommon)=GetCommon(dataMost);
}

var rep1=dataMost.Single().Select((x,i)=>x<<(m-i-1)).Sum();
Console.WriteLine($"dataMost = {rep1}");

(mostCommon,leastCommon)=GetCommon(input);

for (int j = 0;j<m;j++) {

    Console.WriteLine($"\n{j}  {string.Join(" ",leastCommon.Select(x=>x.ToString()))}\n");
    foreach(var d in dataLeast) Console.WriteLine("   "+string.Join(" ",d.Select(x=>x.ToString())));

    if (dataLeast.Length>1) {
        dataLeast=dataLeast.Where(x=>x[j]==leastCommon[j]).ToArray();
    }
    (mostCommon,leastCommon)=GetCommon(dataLeast);

}
var rep2=dataLeast.Single().Select((x,i)=>x<<(m-i-1)).Sum();
Console.WriteLine($"dataLeast = {rep2}");
Console.WriteLine($"{rep1*rep2}");
