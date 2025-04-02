namespace Lox;

public static class LoxInterpreter
{
  static bool hadError = false;

  public static void Main(string[] args)
  {
    if (args.Length > 1)
    {
      Console.WriteLine("Usage: lox_cs [script]");
      Environment.Exit(64);
    }
    else if (args.Length == 1)
    {
      runFile(args[0]);
    }
    else
    {
      runRepl();
    }

  }

  static void runFile(string path)
  {
    var file = File.ReadAllText(path);
    run(file);

    if (hadError)
      Environment.Exit(65);
  }

  static void runRepl()
  {
    Console.Write("lox_cs repl> ");
    var line = Console.ReadLine();
    while (line != null)
    {
      run(line);
      hadError = false;

      Console.Write("lox_cs repl> ");
      line = Console.ReadLine();
    }
  }

  static void run(string source)
  {
    var scanner = new Lox.Scanner(source);
    var tokens = scanner.ScanTokens();

    Parser parser = new Parser(tokens);
    var expr = parser.Parse();

    if (hadError)
      return;

    if (expr is not null)
      Console.WriteLine(new Ast.AstPrinter().Print(expr));
  }

  public static void error(int line, string message)
  {
    report(line, "", message);
  }

  public static void error(Token token, String message)
  {
    if (token.Type == TokenType.EOF)
      report(token.Line, " at end", message);
    else
      report(token.Line, " at '" + token.Lexeme + "'", message);
  }

  static void report(int line, string where, string message)
  {
    Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
    hadError = true;
  }
}


