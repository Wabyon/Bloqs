using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Bloqs.Core")]
[assembly: AssemblyVersion("0.1.0.0")]

#if DEBUG
[assembly: InternalsVisibleTo("Bloqs.Data.Test")]
#endif