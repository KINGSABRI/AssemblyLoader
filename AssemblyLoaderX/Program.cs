using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace AssemblyLoaderX
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("A various implementations for C# binary in memory loaders");
                Console.WriteLine("Usage:");
                Console.WriteLine("AssemblyLoader.exe <FILE.EXE> \"<param1 param2 paramX>\"");
                return;
            }
            var file = args[0];
            Byte[] fileBytes = File.ReadAllBytes(file);

            string[] fileArgs = { };
            if (args.Length > 1)
            {
                fileArgs = args[1].Split(' ');
            }

            ExecuteAssemblyLoad1(fileBytes, fileArgs);
            Console.WriteLine("\n");
            ExecuteAssemblyLoad2(fileBytes, fileArgs);
            Console.WriteLine("\n");
            ExecuteAssemblyLoadFile(file, fileArgs);
            Console.WriteLine("\n");
            ExecuteAssemblyLoadFileAppDomain(file, fileArgs);
            //Console.WriteLine("\n");
        }

        // Load and execute assembly from assembly raw
        //   - Accept local file path
        //   - Accept smb file path
        public static void ExecuteAssemblyLoad1(Byte[] assemblyBytes, string[] param)
        {
            Console.WriteLine("[*] Using Assembly.Load 1:");

            // Load the assembly
            Assembly assembly = Assembly.Load(assemblyBytes);
            // Find the Entrypoint or "Main" method
            MethodInfo method = assembly.EntryPoint;
            // Get the parameters
            object[] parameters = new[] { param };
            // Invoke the method with its parameters
            object execute = method.Invoke(null, parameters);
        }

        // Load and execute assembly from assembly raw
        //   - Accept local file path
        //   - Accept smb file path
        public static void ExecuteAssemblyLoad2(Byte[] assemblyBytes, string[] param)
        {
            Console.WriteLine("[*] Using Assembly.Load 2:");

            // Load the assembly
            Assembly assembly = Assembly.Load(assemblyBytes);
            // Find all the types (Namespaces and classes) containing the methods 
            foreach (var type in assembly.GetTypes())
            {
                Console.WriteLine($"[*] Loaded Type '{type}'");
                // Get the parameters
                object[] parameters = new object[] { param };
                // Console.WriteLine(parameters);

                // Enumerate (and try to load) all methods in this type (doesn't include 'Main' method)
                foreach (MethodInfo method in type.GetMethods())
                {
                    Console.WriteLine($"  [*] Loading Method '{method.Name}'");
                    Console.WriteLine($"  [*] Enumerating Method Parameters:");
                    // Enumerate the method's parameters
                    foreach (var mp in method.GetParameters())
                    {
                        Console.WriteLine($"    - parameter type: '{mp}'");
                    }
                    try
                    {
                        // Create an instance of the type using a constructor
                        object instance = Activator.CreateInstance(type);
                        // Invoke the method with its parameters
                        method.Invoke(instance, parameters);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[!] Ops, wrong method name '{method.Name}'");
                        //Console.WriteLine(e);
                    }
                }
            }
        }

        // Load and execute assembly from path
        //   - Accept only local file path
        public static void ExecuteAssemblyLoadFile(string assemblyPath, string[] param)
        {
            Console.WriteLine("[*] Using Assembly.LoadFile:");

            try
            {
                // Load the assembly
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                // Find the Entrypoint or "Main" method
                MethodInfo method = assembly.EntryPoint;
                // Get the parameters
                object[] parameters = new[] { param };
                // Invoke the method with its parameters
                object execute = method.Invoke(null, parameters);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ExecuteAssemblyLoadFileAppDomain(string file, string[] param)
        {
            Console.WriteLine("[*] Using ExecuteAssemblyLoadFileAppDomain1:");
            PermissionSet permission = new PermissionSet(PermissionState.Unrestricted);
            AppDomainSetup setup = new AppDomainSetup();

            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            AppDomain domain = AppDomain.CreateDomain("King AppDomain", null, setup, permission);

            try
            {
                Console.WriteLine($"[+] Executing '{Path.GetFileName(file)}' in '{domain.FriendlyName}' AppDomain.");
                domain.ExecuteAssembly(file, param);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }
    }
}
