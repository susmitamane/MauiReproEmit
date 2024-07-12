using System.Reflection;
using System.Reflection.Emit;

namespace emitTest;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            // Define a dynamic assembly and module
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // Define a public class named "DynamicClass" in the assembly
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);

            // Define a public method named "HelloMethod" in the class
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("HelloMethod",
                MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes);

            // Get an ILGenerator and emit a body for the "HelloMethod" method
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            // Emit the IL instructions to print "Hello, Reflection.Emit!" to the console
            ilGenerator.Emit(OpCodes.Ldstr, "Hello, Reflection.Emit!");
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            ilGenerator.Emit(OpCodes.Ret);

            // Create the type
            Type dynamicType = typeBuilder.CreateType();

            // Invoke the "HelloMethod" method
            dynamicType.GetMethod("HelloMethod").Invoke(null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}