using System.Collections;

var input=(await Helper.GetInput()).First();


/*Decode("D2FE28");
Decode("38006F45291200");
Decode("EE00D40C823060");*/
/*Console.WriteLine($"res={Decode("CE00C43D881120", out var vsum)}");*/


Console.WriteLine($"res={Decode(input, out var vsum)} vsum={vsum}");

long Decode(string input, out int vsum) {

    var arr=new BitArray(Convert.FromHexString(input).Select(x=>x.Swap()).ToArray());
    Console.WriteLine(string.Join("",arr.Cast<Boolean>().Select(x=>x?"1":"0")));
    int pos=0;
    vsum=0;
    return InnerDecode(arr,ref pos, ref vsum);
}    

long InnerDecode(BitArray data, ref int pos, ref int vsum, int depth=0) 
{
    string prefix=new string('>',depth);

    var version=data.ToBytes(ref pos,3).Single();
    Console.WriteLine($"{prefix}V={version}");
    vsum+=version;

    var type=data.ToBytes(ref pos,3).Single();
    Console.WriteLine($"{prefix}T={type}");

    switch (type)
    {
        case 4: {
            long value=0;
            byte res;
            do {
                res=data.ToBytes(ref pos,5).Single();
//                Console.WriteLine($"{res} {res&16}");
                value<<=4;
                value|=res&15;
//                Console.WriteLine($"Remaining out: {value} {Convert.ToString(value, 2)}");
            } while ((res&16)==16);
            Console.WriteLine($"{prefix}val={value} pos={pos}");
            return value;
            break;
        }
        default: {
            List<long> Operands=new List<long>();
            var lengthtype=data.ToBytes(ref pos,1).Single();
            if (lengthtype==0) {
                var subpacketLength=data.ToBytes(ref pos,15).ToInt16();
                var targetpos=pos+subpacketLength-1;
                while (pos<targetpos) {
                    Operands.Add(InnerDecode(data,ref pos, ref vsum, depth+1));
                }
            } else
            {
                var subpacketCount=data.ToBytes(ref pos,11).ToInt16();
                while (subpacketCount>0) {
                    Operands.Add(InnerDecode(data,ref pos, ref vsum, depth+1));
                    subpacketCount--;
                }
            }

            switch (type)
            {
                case 0: return Operands.Sum(); break;
                case 1: return Operands.Aggregate(1L,(x,s)=>x*s); break;
                case 2: return Operands.Min(); break;
                case 3: return Operands.Max(); break;
                case 5: return Operands[0]>Operands[1]?1:0; break;
                case 6: return Operands[0]<Operands[1]?1:0; break;
                case 7: return Operands[0]==Operands[1]?1:0; break;
                default: return 0; break;
            }
            break;            

        }
    }

}


static class Extension 
{
    static uint bitSwap1(uint x) {
        uint m = 0x55555555;
        return ((x & m) << 1) | ((x & (~m)) >> 1);
    }
    static uint bitSwap2(uint x) {
        uint m = 0x33333333;
        return ((x & m) << 2) | ((x & (~m)) >> 2);
    }
    static uint bitSwap4(uint x) {
        uint m = 0x0f0f0f0f;
        return ((x & m) << 4) | ((x & (~m)) >> 4);
    }

    public static byte Swap(this byte value) {
        return (byte)(bitSwap4(bitSwap2(bitSwap1(value))));
    }

    public static Int16 ToInt16(this IEnumerable<byte> bytes) 
    {
        Int16 ret=0;
        foreach(var b in bytes) {
            ret<<=8;
            ret|=b;
        }
        return ret;
    }


    public static IEnumerable<byte> ToBytes(this BitArray bits, ref int offset, int count, bool MSB = true)
    {
        int bitCount = 7;
        int outByte = 0;
        var ret=new List<byte>();

        var remainder=(8-count%8)%8;

        for(int i=0;i<count+remainder;i++)        
        {
            bool bitValue;
            if (i<remainder) bitValue=false;
            else bitValue=bits[offset++];
            if (bitValue)
                outByte |= MSB ? 1 << bitCount : 1 << (7 - bitCount);
            //Console.WriteLine($"{offset} {remainder} bit: {bitValue} count: {bitCount} out: {outByte} {Convert.ToString(outByte, 2)}");
            if (bitCount == 0)
            {
                ret.Add((byte) outByte);
                bitCount = 8;
                outByte = 0;
            }
            bitCount--;
        }
        // Last partially decoded byte
/*        if (bitCount < 7) {
            outByte>>=(bitCount+1);
            Console.WriteLine($"Remaining out: {outByte} {Convert.ToString(outByte, 2)}");
            ret.Add((byte)(outByte));
        }*/
        return ret;
    }

}