using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Visualize.Shared;

[Generator]
public class GenIcons : ISourceGenerator
{

    public void Initialize(GeneratorInitializationContext context)
    {
        Debug.WriteLine("Initialize code generator");
    }

    public void Execute(GeneratorExecutionContext context)
    {
        Debug.WriteLine("Execute code generator");

        var deserializer=new YamlDotNet.Serialization.DeserializerBuilder().IgnoreUnmatchedProperties().WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance).Build();
        
        foreach(var file in context.AdditionalFiles.Where(f=>Path.GetExtension(f.Path).Equals(".yml",StringComparison.OrdinalIgnoreCase)))
        {
            if (!context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.additionalfiles.namespace", out var mynamespace))
               if (!context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_property.rootnamespace", out mynamespace))
                  mynamespace = null;

            var dico =deserializer.Deserialize<Dictionary<string,IconItem>>(file.GetText()!.ToString());

            var source = $@"
    
    {(mynamespace!=null?$"namespace {mynamespace};":"")}

    public static class Icons {{
        {string.Join("\n", dico.Select(kv => $"public const string {ConvertToPascalCase(kv.Key)}=\"\\u{kv.Value.Unicode}\"; // {kv.Value.Label}"))}                
    }}
";
            context.AddSource($"Icon_{Path.GetFileNameWithoutExtension(file.Path)}", source);

        }
    }

    private string ConvertToPascalCase(string key)
    {
        var r=Regex.Replace(key, @"(^|-)(?<first>\w)", (m) => m.Groups["first"].Value.ToUpper());
        if (char.IsDigit(r[0]) || r=="Equals" || r=="Icons") return $"_{r}";
        return r;
    }

    private class IconItem
    {
        public string? Label { get; set; }
        public string? Unicode { get; set; }
    }

}
