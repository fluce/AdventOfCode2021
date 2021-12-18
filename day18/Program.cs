var input = (await Helper.GetInput()).ToArray();

//AbstractNumber.Parse("[[[[[9,8],1],2],3],4]").Reduce();
//AbstractNumber.Parse("[7,[6,[5,[4,[3,2]]]]]").Reduce();
//AbstractNumber.Parse("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]").Reduce();
//AbstractNumber.Parse("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]").Reduce();

/*AbstractNumber.Add(
    AbstractNumber.Parse("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"),
    AbstractNumber.Parse("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]")
).Reduce();*/

/*AbstractNumber.Add(
    AbstractNumber.Parse("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"),
    AbstractNumber.Parse("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]")
).Reduce();*/


var inp=input.Select(AbstractNumber.Parse).ToArray();
var n=inp.First().Clone();
foreach(var a in inp.Skip(1))
    n=AbstractNumber.Add(n,a.Clone());

Console.WriteLine($"magnitude={n.Magnitude}");

var maxMagn=0;
foreach(var i in inp)
    foreach(var j in inp)
    {
        if (i!=j)
        {
            var res=AbstractNumber.Add(i.Clone(),j.Clone());
            var magnitude=res.Magnitude;
            Console.WriteLine($"{i} + {j} = {res} => {magnitude}");
            if (magnitude>maxMagn) maxMagn=magnitude;
        }
            
    }

Console.WriteLine($"magnitude={n.Magnitude}");
Console.WriteLine($"maxmagnitude={maxMagn}");


abstract class AbstractNumber
{
    public abstract AbstractNumber Clone(Pair owningPair=null);

    public Pair OwningPair { get; internal set; }

    public Pair Root => OwningPair?.Root??(this as Pair);
    
    public abstract int Magnitude { get; }

    public AbstractNumber Highlighted { get;set;}

    public bool IsHighlighted => CheckHightlight(this);

    public bool CheckHightlight(AbstractNumber number) {
        if (number==Highlighted) return true;
        return OwningPair?.CheckHightlight(number)??false;
    }

    public string HighlightIfNeeded(string val) => IsHighlighted?$"\u001b[31m{val}\u001b[0m":val;
    public int Depth => (OwningPair?.Depth??0) + 1;

    public void Reduce()
    {
        while(ReduceStep(1) || ReduceStep(2)) {};
        //Console.WriteLine($"Reduced : {this}");
    }

    internal abstract bool ReduceStep(int step);

    public static AbstractNumber Add(AbstractNumber a, AbstractNumber b) 
    {
        var p=new Pair(null);
        p.Left=a;
        p.Right=b;
        a.OwningPair=p;
        b.OwningPair=p;
        p.Reduce();
        return p;
    }

    public static AbstractNumber Parse(string input)
    {
        Pair rootNumber = new Pair(null);
        Stack<Pair> stack = new Stack<Pair>();
        stack.Push(rootNumber);
        int pos = 0;
        Console.WriteLine($"Parsing {input}");
        foreach (char c in input)
        {

            switch (c)
            {

                case '[':
                    {
                        var p = stack.Peek();
                        var val = new Pair(p);
                        if (p.Left == null) p.Left = val;
                        else if (p.Right == null) p.Right = val;
                        else throw new Exception("Syntax error : pair already full");
                        stack.Push(val);
                        break;
                    }

                case ',': break;
                case ']':
                    {
                        var p = stack.Pop();
                        if (p.Left == null || p.Right == null)
                            throw new Exception("Syntax error : pair not full");
                        break;
                    }
                default:
                    if (char.IsDigit(c))
                    {
                        var p = stack.Peek();
                        var val = new Number(p, (int)Char.GetNumericValue(c));
                        if (p.Left == null) p.Left = val;
                        else if (p.Right == null) p.Right = val;
                        else throw new Exception("Syntax error : pair already full");
                    }
                    break;

            }
            //Console.WriteLine($"Parse {pos++}:{c} => {rootNumber.Left}");
        }
        var remaining = stack.Pop();
        if (remaining != rootNumber || stack.Count != 0) throw new Exception("Syntax error : unmatched []");
        rootNumber.Left.OwningPair=null;
        return rootNumber.Left;

    }
}

class Pair : AbstractNumber
{

    public Pair(Pair owning) { OwningPair = owning; }

    public override AbstractNumber Clone(Pair owning=null) {
        var r=new Pair(owning);
        r.Left=Left.Clone(r);
        r.Right=Right.Clone(r);
        return r;
    }

    public AbstractNumber? Left { get; set; }
    public AbstractNumber? Right { get; set; }

    public override int Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

    public override string ToString() => (Depth>4?"*":"")+HighlightIfNeeded($"[{Left},{Right}]");

    internal override bool ReduceStep(int step)
    {
        //Console.WriteLine($"Reducing {this} {Depth} {OwningPair}");
        if (Depth > 4 && step==1)
        {
            Explode();
            return true;
        }
        return Left.ReduceStep(step) || Right.ReduceStep(step);
    }

    public void Replace(AbstractNumber toReplace, AbstractNumber number)
    {
        if (toReplace == Left) Left = number;
        else if (toReplace == Right) Right = number;
        else throw new Exception($"Invalid replace {this} {toReplace} {number}");
    }

    public void Explode()
    {
        Root.Highlighted=this;
        //Console.WriteLine($"Exploding     : {Root}");
        Number nl, nr, nn;
        if (Left is Number l && Right is Number r)
        {
            nl=OwningPair?.AddToFirstNumberOnLeft(this, l.Value);
            nr=OwningPair?.AddToFirstNumberOnRight(this, r.Value);
            OwningPair.Replace(this, nn=new Number(OwningPair, 0));
        }
        else throw new Exception($"Invalid explode {this}");
        if (nl!=null) nl.Highlighted=nl;
        if (nr!=null) nr.Highlighted=nr;
        if (nn!=null) nn.Highlighted=nn;
        //Console.WriteLine($"Exploded      : {Root}");
        if (nl!=null) nl.Highlighted=null;
        if (nr!=null) nr.Highlighted=null;
        if (nn!=null) nn.Highlighted=null;
        Root.Highlighted=null;

    }

    public Number AddToFirstNumberOnLeft(Pair source, int val)
    {
        //Console.WriteLine($"AddToFirstNumberOnLeft {this} source={source} val={val}");
        if (source == Left) return OwningPair?.AddToFirstNumberOnLeft(this, val);

        if (Right is Number r) { r.Value += val; return r; }

        if (Right is Pair pr && Right!=source) return pr.AddToFirstNumberOnLeft(source, val);

        if (Left is Number l) { l.Value += val; return l; }

        if (Left is Pair pl) return pl.AddToFirstNumberOnLeft(source, val);

        return OwningPair?.AddToFirstNumberOnLeft(source, val);
        //Console.WriteLine($"AddToFirstNumberOnLeft {this} => {OwningPair}");
    }

    public Number AddToFirstNumberOnRight(Pair source, int val)
    {
        //Console.WriteLine($"AddToFirstNumberOnRight {this} source={source} val={val}");
        if (source == Right) return OwningPair?.AddToFirstNumberOnRight(this, val);

        if (Left is Number l) { l.Value += val; return l; }

        if (Left is Pair pl && Left!=source) return pl.AddToFirstNumberOnRight(source, val);

        if (Right is Number r) { r.Value += val; return r; }

        if (Right is Pair pr) return pr.AddToFirstNumberOnRight(source, val);

        return OwningPair?.AddToFirstNumberOnRight(source, val);
        //Console.WriteLine($"AddToFirstNumberOnRight {this} => {OwningPair}");
    }

}

class Number : AbstractNumber
{
    public Number(Pair owning, int val) { Value = val; OwningPair = owning; }

    public override AbstractNumber Clone(Pair owning=null) => new Number(owning, Value);

    public int Value { get; set; }

    public override int Magnitude => Value;

    public override string ToString() => HighlightIfNeeded($"{Value}");

    internal override bool ReduceStep(int step)
    {
        if (Value >= 10 && step==2)
        {
            Split();
            return true;
        }
        return false;
    }

    private void Split() 
    {
        OwningPair.Highlighted=this;
        //Console.WriteLine($"Splitting    : {Root}");
        var p=new Pair(OwningPair);
        p.Left=new Number(p,(int)Math.Truncate((float)Value/2f));
        p.Right=new Number(p,(int)Math.Truncate(0.5f+(float)Value/2f));
        OwningPair.Highlighted=p;
        OwningPair.Replace(this,p);
        //Console.WriteLine($"Split result : {Root}");
        OwningPair.Highlighted=null;
    }

}
