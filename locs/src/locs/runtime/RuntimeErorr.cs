namespace Lox.Runtime;

public class RuntimeError : Exception
{
  public Token Token { get; }

  public RuntimeError(Token token, string message) : base(message)
  {
    Token = token;
  }

}
