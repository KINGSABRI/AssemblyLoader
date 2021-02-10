using System;
using System.IO;
using System.Reflection;

// https://www.csharpcodi.com/csharp-examples/System.Reflection.Assembly.Load(byte[])/
// https://exord66.github.io/csharp-in-memory-assemblies
// https://blog.netspi.com/net-reflection-without-system-reflection-assembly/
namespace AssemblyLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("A simple C# binary in memory loader");
                Console.WriteLine("Usage:");
                Console.WriteLine("AssemblyLoader.exe <FILE.EXE> \"<param1 param2 paramX>\"");
                return;
            }
            Byte[] fileBytes = File.ReadAllBytes(args[0]);

            string[] fileArgs = { };
            if (args.Length > 1)
            {
                fileArgs = args[1].Split(' ');
            }

            ExecuteAssembly(fileBytes, fileArgs);
        }

        public static void ExecuteAssembly(Byte[] assemblyBytes, string[] param)
        {
            // Load the assembly
            Assembly assembly = Assembly.Load(assemblyBytes);
            // Find the Entrypoint or "Main" method
            MethodInfo method = assembly.EntryPoint;
            // Get the parameters
            object[] parameters = new[] { param };
            // Invoke the method with its parameters
            object execute = method.Invoke(null, parameters);
        }
    }
}
