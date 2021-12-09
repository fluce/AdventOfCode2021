var input=(await Helper.GetInput()).First().Split(',').Select(x=>int.Parse(x)).GroupBy(x=>x).ToDictionary(x=>x.Key,x=>(long)x.Count());

foreach(var d in input) Console.WriteLine($" {d.Key} = {d.Value}");

List<(int,long)> toAdd=new List<(int,long)>();

for(int i=0;i<256;i++) {
    input=input.ToDictionary(x=>x.Key-1,x=>x.Value);
    //Console.WriteLine($"{i}  {string.Join(" ", input.Select(d=>$"{d.Key}={d.Value}"))}");
    foreach(var c in toAdd) {
        if (input.ContainsKey(c.Item1))
            input[c.Item1]+=c.Item2;
        else
            input[c.Item1]=c.Item2;
    }
    toAdd=input.Where(x=>x.Key==0).Select(x=>(8,x.Value)).ToList();
    if (i==80) Console.WriteLine($"{i}  {string.Join(" ", input.Select(d=>$"{d.Key}={d.Value}"))} => {input.Sum(x=>x.Value)}");
    input=input.Select(x=>(x.Key==-1?6:x.Key,x.Value)).GroupBy(x=>x.Item1).ToDictionary(x=>x.Key,x=>x.Sum(y=>y.Value));
}

Console.WriteLine($"res={input.Sum(x=>x.Value)}");
