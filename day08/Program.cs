var input=(await Helper.GetInput()).Select(x=>Regex.Match(x,@"^((?<A>[a-g]+) )+\|( (?<B>[a-g]+))+"))
    .Select(x=>(x.Groups["A"].Captures.Select(y=>y.Value).ToArray(),x.Groups["B"].Captures.Select(y=>y.Value).ToArray())).ToArray();

var c=0;
foreach(var (a,b) in input) {
    Console.WriteLine($"{string.Join("-",a)} # {string.Join("-",b)}");
    c+=b.Count(x=>x.Length==2 || x.Length==4 || x.Length==3 || x.Length==7 );
}

Console.WriteLine(c);

var t="abcdefg";
c=0;
foreach(var (a,b) in input) {
   
    var decode=new Dictionary<string,byte>();

    decode[new string(a.Single(x=>x.Length==2).OrderBy(x=>x).ToArray())]=1;
    decode[new string(a.Single(x=>x.Length==4).OrderBy(x=>x).ToArray())]=4;
    decode[new string(a.Single(x=>x.Length==3).OrderBy(x=>x).ToArray())]=7;
    decode[new string(a.Single(x=>x.Length==7).OrderBy(x=>x).ToArray())]=8;

    Console.WriteLine($"{string.Join("-",a)} # {string.Join("-",b)}");

    var a5=a.Where(x=>x.Length==5);
    var cc5=new int[7];
    foreach(var x in a5) {
        for(int i=0;i<7;i++)
            if (x.Contains(t[i])) cc5[i]++;
        Console.WriteLine($"{x} {new string(t.Select(y=>x.Contains(y)?y:'.').ToArray())}");        
    }
    Console.WriteLine(string.Join(" ",cc5.Select(x=>x.ToString())));
    var horz=new string(cc5.Select((x,i)=>x==3?t[i]:' ').Where(x=>x!=' ').ToArray());
    Console.WriteLine($"Horz bar : {horz}");

    var a6=a.Where(x=>x.Length==6);
    var cc6=new int[7];
    var zero="";
    foreach(var x in a6) {
        for(int i=0;i<7;i++)
            if (x.Contains(t[i])) cc6[i]++;        
        Console.WriteLine($"{x} {new string(t.Select(y=>x.Contains(y)?y:'.').ToArray())}");        
        if (x.Intersect(horz).Count()==2) {
            zero=new string(x.OrderBy(x=>x).ToArray());
            decode[zero]=0;
        } 
    }
    foreach(var x in a6) {
        var d=new string(x.OrderBy(x=>x).ToArray());
        if (d!=zero) {
            var c4=0;
            var c5=0;
            foreach(var y in a5) {
                var ic=x.Intersect(y).Count();
                if (ic==5) c5++;
                if (ic==4) c4++;
                Console.WriteLine($"{x} {y} => {ic}");
            }
            if (c4==2) decode[d]=6;
            if (c4==1) decode[d]=9;
        }
    }

    foreach(var x in a5) {
        var d=new string(x.OrderBy(x=>x).ToArray());
        if (d!=zero) {
            var c4=0;
            var c5=0;
            foreach(var y in a6) {
                var ic=x.Intersect(y).Count();
                if (ic==5) c5++;
                if (ic==4) c4++;
                Console.WriteLine($"{x} {y} => {ic}");
            }
            if (c5==2) decode[d]=5;
            if (c5==0) decode[d]=2;
            if (c5==1) decode[d]=3;
        }
    }

    foreach(var (k,v) in decode) Console.WriteLine($"{k} = {v}");

    var n=int.Parse(string.Join("",b.Select(x=>new string(x.OrderBy(x=>x).ToArray())).Select(x=>decode[x].ToString())));
    c+=n;
    Console.WriteLine(n);
}

Console.WriteLine($"Grand total : {c}");
