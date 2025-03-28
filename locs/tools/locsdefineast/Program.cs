// See https://aka.ms/new-console-template for more information
using System.Text;

if (args.Length != 1)
{
  Console.Error.WriteLine("Usage: generate_ast <output directory>");
  Environment.Exit(64);
}

String outputDir = args[0];


defineAst(outputDir, "Expr", new Dictionary<string, string>{
    { "Binary","Expr left, Token Operator, Expr right"},
    { "Grouping","Expr expression"},
    { "Literal","Object value"},
    { "Unary","Token Operator, Expr right"},
});

static void defineAst(String outputDir, String baseName, Dictionary<string, string> types)
{
  string path = outputDir + "/" + baseName + ".cs";
  using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

  writer.WriteLine("namespace Lox;");
  writer.WriteLine();
  writer.WriteLine("public abstract class " + baseName);
  writer.WriteLine("{");

  // The base accept() method.
  writer.WriteLine("  public abstract R accept<R>(Visitor<R> visitor);");
  writer.WriteLine();

  defineVisitor(writer, baseName, types);

  // The AST classes.
  foreach (var type in types)
  {
    string className = type.Key;
    string fields = type.Value.Trim(); ;
    defineType(writer, baseName, className, fields);
  }

  writer.WriteLine("}");
  writer.Close();
}

static void defineVisitor(StreamWriter writer, string baseName, Dictionary<string, string> types)
{
  writer.WriteLine("  public interface Visitor<R>");
  writer.WriteLine("  {");

  foreach (var type in types)
  {
    var typeName = type.Key;
    writer.WriteLine("    R visit" + typeName + baseName + "(" +
        typeName + " " + baseName.ToLower() + ");");
  }

  writer.WriteLine("  }");
}

static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
{
  // AutoConstructor
  writer.WriteLine();
  writer.Write("  public class " + className);
  writer.WriteLine("(" + fieldList + ")");
  writer.WriteLine("  {");

  // Store parameters in properties.
  var fields = fieldList.Split(", ");
  foreach (var field in fields)
  {
    var name = field.Split(" ")[1];
    var type = field.Split(" ")[0];
    writer.WriteLine($"    public {type} {name.Substring(0, 1).ToUpper() + name.Substring(1)} {{ get; }} = " + name + ";");
  }

  writer.WriteLine("  }");
}
