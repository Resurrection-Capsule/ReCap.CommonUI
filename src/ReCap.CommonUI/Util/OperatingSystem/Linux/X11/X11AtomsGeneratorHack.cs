using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    static class X11AtomsGeneratorHack
    {
#if X11ATOMS_GENERATOR_HACK
        public static void Main(string[] args)
        {
            Generate();
        }
#endif




        static StringBuilder Pad(this StringBuilder sb, int count)
            => sb.Append(' ', count * 4);
        static StringBuilder AppendLines(this StringBuilder sb, params string[] array)
            => sb.AppendLines(lines: array);
        static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<string> lines)
            => sb.AppendLines(0, lines);
        static StringBuilder AppendLines(this StringBuilder sb, int count, params string[] array)
            => sb.AppendLines(count, lines: array);
        static StringBuilder AppendLines(this StringBuilder sb, int count, IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                sb.Pad(count).AppendLine(line);
            }
            return sb;
        }


        static readonly Type _X11ATOMS_TYPE = typeof(X11Atoms);
        static readonly Type _INTPTR_TYPE = typeof(IntPtr);
        static IEnumerable<FieldInfo> GetFields()
        {
            var fieldInfos = _X11ATOMS_TYPE.GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == _INTPTR_TYPE)
                    yield return fieldInfo;
            }
        }




        static readonly IEnumerable<string> _SUBPATH = "src/ReCap.CommonUI/Util/OperatingSystem/Linux/X11/X11Atoms`generated.cs".Split('/');
        static string AutoDetectPath()
            => AutoDetectPath(
            /*
                System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
                Path.Combine(Environment.CurrentDirectory, 
            */
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            );
        static string AutoDetectPath(string initPath)
        {
            string path = initPath;
            string nextPath = path;
            string slnPath;
            do
            {
                path = nextPath;
                slnPath = Path.Combine(path, "ReCap.CommonUI.sln");
                nextPath = Path.GetDirectoryName(path);
            }
            while ((path != null) && (slnPath != null) && !File.Exists(slnPath));

            return Path.Combine(_SUBPATH
                .Prepend(path)
                .ToArray()
            );
        }


        const string _Generate__PARAM_NAME_display = "display";
        const string _Generate__VAR_NAME_atoms = "atoms";
        const string _Generate__VAR_NAME_atomNames = "atomNames";
        public static void Generate()
        {
            Console.WriteLine($"{nameof(X11AtomsGeneratorHack)}.{nameof(Generate)}()");
            string path = AutoDetectPath();
            Console.WriteLine($"    {nameof(path)}:\n    '{path}'");
            Generate(path);
        }
        public static void Generate(string path)
        {
            List<FieldInfo> fields = new(GetFields());
            List<string> linesA = new();
            List<string> linesB = new();
            for (int i = 0; i < fields.Count; i++)
            {
                string fieldName = fields[i].Name;
                string fieldNameof = $"nameof({fieldName})";
                linesA.Add($"{fieldNameof},");
                linesB.Add($"{nameof(X11Atoms.InitAtom)}(ref {fieldName}, {fieldNameof}, {_Generate__VAR_NAME_atoms}[{i}]);");
            }

            
            var classBuilder = new StringBuilder()
                .AppendLines(new[]
                {
                    $"using {nameof(System)};",
                    string.Empty,
                    $"namespace {nameof(ReCap)}.{nameof(CommonUI)}.{nameof(Util)}.{nameof(OperatingSystem)}.{nameof(Linux)}",
                    "{",
                })
                .AppendLines(1, new[]
                {
                    $"partial class {nameof(X11Atoms)}",
                    "{",
                })
                .AppendLines(2, new[]
                {
                    $"public {nameof(X11Atoms)}({nameof(IntPtr)} {_Generate__PARAM_NAME_display})",
                    "{",
                })
                .AppendLines(3, new[]
                {
                    $"_{_Generate__PARAM_NAME_display} = {_Generate__PARAM_NAME_display};",
                    $"var {_Generate__VAR_NAME_atoms} = new {nameof(IntPtr)}[{fields.Count}];",
                    $"var {_Generate__VAR_NAME_atomNames} = new {nameof(String).ToLowerInvariant()}[]",
                    "{",
                })
                .AppendLines(4, linesA)
                .AppendLines(3, new[]
                {
                    "};",
                    $"{nameof(LibX11)}.{nameof(LibX11.XInternAtoms)}({_Generate__PARAM_NAME_display}, {_Generate__VAR_NAME_atomNames}, {_Generate__VAR_NAME_atomNames}.{nameof(Array.Length)}, true, {_Generate__VAR_NAME_atoms});",
                })
                .AppendLines(3, linesB)
                .AppendLines(2, "}")
                .AppendLines(1, "}")
                .AppendLines("}")
            ;

            File.WriteAllText(path, classBuilder.ToString());
        }
    }
}