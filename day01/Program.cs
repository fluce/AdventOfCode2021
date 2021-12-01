var input=(await Helper.GetInput(1)).Select(x=>int.Parse(x)).ToArray();

var counter=0;
var p=0;
foreach(var (x,i) in input.Select((x,i)=>(x,i)))
{
    if (i>0 && x>p) counter++;
    p=x;
}

Console.WriteLine($"counter={counter}");


int prevsum=0;
counter=0;
var (remove,(a1,a2,a3,_))=(0,input);
var sum=prevsum=a1+a2+a3;
foreach(var i in input[3..])
{
    (remove,a1,a2,a3)=(a1,a2,a3,i);
    sum+=i-remove;
    Console.WriteLine($"last: {(a1,a2,a3)} => sum={sum}");
    if (sum>prevsum) counter++;
    prevsum=sum;
}

Console.WriteLine($"counter={counter}");

