# AssemblyLoader
Here, I'm listing various implementations of loading C# in memory in simple code for reference.

## Implementations/Techniques 
### Assembly.Load()
This has been used in two different ways

1. The **`ExecuteAssemblyLoad1`** is a very simple and vanilla use of `Assembly.Load()`
2. The **`ExecuteAssemblyLoad2`** it 
   1. enumerates namespaces, 
   2. enumerates the classes of each namespece, 
   3. enumerates the methods of each class, 
   4. enumerates the parameters of each method 
   5. then finally call the method to be executed.

### Assembly.LoadFile()
The **`ExecuteAssemblyLoadFile`** is a vanilla use of `Assembly.LoadFile()`

### AppDomain.ExecuteAssembly()
The **`ExecuteAssemblyLoadFileAppDomain`** is using AppDomain to load the assembly, execute it then unload the AppDomain.


## Usage 

### Executing an assembly without arguments
```
C:\AssemblyLoader\AssemblyLoaderX\bin\Release\AssemblyLoaderX.exe C:\Tools\JustACommand.exe
```

result
```
[*] Using Assembly.Load 1:
I've been executed successfully!


[*] Using Assembly.Load 2:
[*] Loaded Type 'JustACommand.Program'
  [*] Loading Method 'CallMeIfYouCan'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.String[] names'
  [*] Loading Method 'CallMeIfYouCan2'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.String name'
[!] Ops, wrong method name 'CallMeIfYouCan2'
  [*] Loading Method 'Equals'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.Object obj'
  [*] Loading Method 'GetHashCode'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetHashCode'
  [*] Loading Method 'GetType'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetType'
  [*] Loading Method 'ToString'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'ToString'


[*] Using Assembly.LoadFile:
I've been executed successfully!


[*] Using ExecuteAssemblyLoadFileAppDomain1:
[+] Executing 'JustACommand.exe' in 'King AppDomain' AppDomain.
I've been executed successfully!
```

### Executing an assembly with arguments
```
C:\AssemblyLoader\AssemblyLoaderX\bin\Release\AssemblyLoaderX.exe C:\Tools\JustACommandWithArgs.exe "111 222 333"
```
result
```
[*] Using Assembly.Load 1:
Hi 111!
Hi 222!
Hi 333!


[*] Using Assembly.Load 2:
[*] Loaded Type 'JustACommandWithArgs.Program'
  [*] Loading Method 'Equals'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.Object obj'
  [*] Loading Method 'GetHashCode'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetHashCode'
  [*] Loading Method 'GetType'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetType'
  [*] Loading Method 'ToString'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'ToString'


[*] Using Assembly.LoadFile:
Hi 111!
Hi 222!
Hi 333!


[*] Using ExecuteAssemblyLoadFileAppDomain1:
[+] Executing 'JustACommandWithArgs.exe' in 'King AppDomain' AppDomain.
Hi 111!
Hi 222!
Hi 333!
```

### Executing an assembly without arguments and enumerate all class methods (not the "Main" method) to be executed.

See `Assembly.Load 2` enumerating all the namesspace classes then enumerate all methods in that class and try to execute it.
It's going to find `CallMeIfYouCan` method, enumerate its parameters then call it for execution.

```
C:\AssemblyLoader\AssemblyLoaderX\bin\Release\AssemblyLoaderX.exe C:\Tools\JustACommand.exe "111 222 333"
```
result
```
[*] Using Assembly.Load 1:
I've been executed successfully!


[*] Using Assembly.Load 2:   <-----
[*] Loaded Type 'JustACommand.Program'
  [*] Loading Method 'CallMeIfYouCan'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.String[] names'
CallMeIfYouCan says: Hello, 111!.
CallMeIfYouCan says: Hello, 222!.
CallMeIfYouCan says: Hello, 333!.
  [*] Loading Method 'CallMeIfYouCan2'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.String name'
[!] Ops, wrong method name 'CallMeIfYouCan2'
  [*] Loading Method 'Equals'
  [*] Enumerating Method Parameters:
    - parameter type: 'System.Object obj'
  [*] Loading Method 'GetHashCode'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetHashCode'
  [*] Loading Method 'GetType'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'GetType'
  [*] Loading Method 'ToString'
  [*] Enumerating Method Parameters:
[!] Ops, wrong method name 'ToString'


[*] Using Assembly.LoadFile:
I've been executed successfully!


[*] Using ExecuteAssemblyLoadFileAppDomain1:
[+] Executing 'JustACommand.exe' in 'King AppDomain' AppDomain.
I've been executed successfully!
```


## Resources
- https://www.csharpcodi.com/csharp-examples/System.Reflection.Assembly.Load(byte[])/
- https://exord66.github.io/csharp-in-memory-assemblies
- https://blog.netspi.com/net-reflection-without-system-reflection-assembly/

