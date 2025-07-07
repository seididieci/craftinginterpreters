namespace Lox.Runtime;

public class Environment(Environment enclosing = null)
{
  public Environment Enclosing { get; } = enclosing;
  private readonly Dictionary<string, object> values = new();
  private readonly HashSet<string> assigned = new();


  public void Define(string name, object value, bool assigned = false)
  {
    // Allows variable redefinition
    if (values.ContainsKey(name))
      values[name] = value;
    else
      values.Add(name, value);

    if (assigned)
      this.assigned.Add(name);
  }

  public object Get(Token name)
  {
    if (values.TryGetValue(name.Lexeme, out var value))
    {
      if (!assigned.Contains(name.Lexeme))
      {
        Console.WriteLine($"Tried to get unassigned variable '{name.Lexeme}': {value}.");
      }

      //   throw new RuntimeError(name, $"Tried to get unassigned variable '{name.Lexeme}'.");
      return value;
    }

    if (Enclosing != null)
      return Enclosing.Get(name);

    throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
  }

  public object Assign(Token name, object value)
  {
    if (values.ContainsKey(name.Lexeme))
    {
      assigned.Add(name.Lexeme);
      return values[name.Lexeme] = value;
    }

    if (Enclosing != null)
      return Enclosing.Assign(name, value);

    throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
  }
}
