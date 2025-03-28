namespace Lox;

public abstract class Expr
{
  public abstract R accept<R>(Visitor<R> visitor);

  public interface Visitor<R>
  {
    R visitBinaryExpr(Binary expr);
    R visitGroupingExpr(Grouping expr);
    R visitLiteralExpr(Literal expr);
    R visitUnaryExpr(Unary expr);
  }

  public class Binary(Expr left, Token Operator, Expr right)
  {
    public Expr Left { get; } = left;
    public Token Operator { get; } = Operator;
    public Expr Right { get; } = right;
  }

  public class Grouping(Expr expression)
  {
    public Expr Expression { get; } = expression;
  }

  public class Literal(Object value)
  {
    public Object Value { get; } = value;
  }

  public class Unary(Token Operator, Expr right)
  {
    public Token Operator { get; } = Operator;
    public Expr Right { get; } = right;
  }
}
