var scanners=new LinkedList<(int idx, List<(int x, int y, int z)> beacons)>();

int pi=-1;
List<(int x, int y, int z)>? current=null;
foreach(var line in (await Helper.GetInput()))
{
    if (line.StartsWith("---")) {
        pi++;
        scanners.AddLast((pi,current=new List<(int x, int y, int z)>()));
        continue;
    }
    else if (string.IsNullOrWhiteSpace(line)) continue;

    var (x,y,z)=line.Split(",").Select(int.Parse).ToArray();
    current!.Add((x,y,z));

}

Dump();
void Dump() {
    foreach(var (i,s) in scanners)
    {
        foreach(var beacon in s)
        {
            Console.WriteLine($"{i} : {beacon}");
        }
    }
}

var vects=new(int x,int y, int z)[] {
    ( 1, 0, 0 ),
    ( 0, 1, 0 ),
    ( 0, 0, 1 ),
    ( -1, 0, 0 ),
    ( 0, -1, 0 ),
    ( 0, 0, -1 )
};

var matx=vects.SelectMany(x=>vects,(u,v)=>(u,v,w:(
            x:(u.y*v.z-u.z*v.y),
            y:(u.z*v.x-u.x*v.z),
            z:(u.x*v.y-u.y*v.x)
            )))
            .Where(m=>m.w.x!=0 || m.w.y!=0 || m.w.z!=0)
            .ToArray();



var alreadyScanned=new LinkedList<(int idx, List<(int x, int y, int z)> beacons)>();
var firstScanner=scanners.First.Value;
scanners.RemoveFirst();
alreadyScanned.AddLast(firstScanner);

var scannerPos=new List<(int x, int y, int z)>();
scannerPos.Add((0,0,0));

var allbeacons=firstScanner.beacons.OrderBy(p=>p.z).OrderBy(p=>p.y).OrderBy(p=>p.x).ToList();

do {

(int idx,List<(int x, int y, int z)> beacons)? found=null;
List<(int x, int y, int z)> foundRot=null;
foreach (var s1 in alreadyScanned)
{
    foreach (var s in scanners)
    {
        foreach(var mat in matx) {

            var rotated=s.beacons.Select(p=>(
                                    x:mat.u.x*p.x+mat.u.y*p.y+mat.u.z*p.z,
                                    y:mat.v.x*p.x+mat.v.y*p.y+mat.v.z*p.z,
                                    z:mat.w.x*p.x+mat.w.y*p.y+mat.w.z*p.z
                                )).OrderBy(p=>p.z).OrderBy(p=>p.y).OrderBy(p=>p.x).ToList();

            var commonsx=s1.beacons.SelectMany(a=>rotated,(a,b)=>a.x-b.x).GroupBy(x=>x).Select(x=>(n:x.Key,c:x.Count())).OrderByDescending(x=>x.c).Where(x=>x.c>=12).ToDictionary(x=>x.n);
            var commonsy=s1.beacons.SelectMany(a=>rotated,(a,b)=>a.y-b.y).GroupBy(x=>x).Select(x=>(n:x.Key,c:x.Count())).OrderByDescending(x=>x.c).Where(x=>x.c>=12).ToDictionary(x=>x.n);
            var commonsz=s1.beacons.SelectMany(a=>rotated,(a,b)=>a.z-b.z).GroupBy(x=>x).Select(x=>(n:x.Key,c:x.Count())).OrderByDescending(x=>x.c).Where(x=>x.c>=12).ToDictionary(x=>x.n);

            var c=s1.beacons.SelectMany(a=>rotated,(a,b)=>commonsx.TryGetValue(a.x-b.x,out var _) && commonsy.TryGetValue(a.y-b.y,out var _) && commonsz.TryGetValue(a.z-b.z,out var _)).Where(x=>x).Count();            
            if (c>=12) {
                var coord=(x:commonsx.Single().Key,y:commonsy.Single().Key,z:commonsz.Single().Key);
                scannerPos.Add(coord);
                Console.WriteLine($"\nMatrix : {mat}  {coord}");

                foreach (var ps in s1.beacons) {
                    Console.Write($"{ps.x,5} : ");
                    foreach (var pr in rotated) {
                        Console.Write($"{ps.x-pr.x,6}".HighlightIf(x=>commonsx.TryGetValue(ps.x-pr.x,out var _) && commonsy.TryGetValue(ps.y-pr.y,out var _) && commonsz.TryGetValue(ps.z-pr.z,out var _)));
                    }
                    Console.WriteLine();
                }
                var translated=rotated.Select(u=>(x:u.x+coord.x,y:u.y+coord.y,z:u.z+coord.z)).ToList();
                //foreach(var b in translated) Console.WriteLine(b);
                allbeacons=allbeacons.Concat(translated).Distinct().ToList();
                Console.WriteLine($"{s.idx} overlaps with {s1.idx}");
                Console.WriteLine($"All beacons count = {allbeacons.Count}");
                foundRot=translated;
                found=s;
                break;
            }      
        }
        if (found!=null) break;
    }
    if (found!=null) break;
}
scanners.Remove(found.Value);
alreadyScanned.AddLast((found.Value.idx,foundRot));
} while (scanners.Count>0);

var maxdist=scannerPos.SelectMany(x=>scannerPos,(a,b)=>Math.Abs(a.x-b.x)+Math.Abs(a.y-b.y)+Math.Abs(a.z-b.z)).Max();
Console.WriteLine($"Max dist={maxdist}");