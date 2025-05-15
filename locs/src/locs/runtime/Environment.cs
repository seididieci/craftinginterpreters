namespace Lox.Runtime;

public class Environment
{
  private readonly Dictionary<string, object> values = new();

  public void Define(string name, object value)
  {
    // Allows variable redefinition
    if (values.ContainsKey(name))
      values[name] = value;
    else
      values.Add(name, value);
  }

  public object Get(Token name)
  {
    if (values.TryGetValue(name.Lexeme, out var value))
      return value;
    throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
  }

  public object Assign(Token name, object value)
  {
    if (values.ContainsKey(name.Lexeme))
      return values[name.Lexeme] = value;
    throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
  }
}
