namespace Lox.Ast;

public abstract class Expr
{
  public abstract R Accept<R>(IVisitor<R> visitor);

  public interface IVisitor<R>
  {
    R VisitAssignExpr(Assign expr);
    R VisitBinaryExpr(Binary expr);
    R VisitGroupingExpr(Grouping expr);
    R VisitLiteralExpr(Literal expr);
    R VisitUnaryExpr(Unary expr);
    R VisitVariableExpr(Variable expr);
  }

  public class Assign(Token name, Expr value) : Expr
  {
    public Token Name { get; } = name;
    public Expr Value { get; } = value;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitAssignExpr(this);
    }
  }

  public class Binary(Expr left, Token Operator, Expr right) : Expr
  {
    public Expr Left { get; } = left;
    public Token Operator { get; } = Operator;
    public Expr Right { get; } = right;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitBinaryExpr(this);
    }
  }

  public class Grouping(Expr expression) : Expr
  {
    public Expr Expression { get; } = expression;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitGroupingExpr(this);
    }
  }

  public class Literal(Object value) : Expr
  {
    public Object Value { get; } = value;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitLiteralExpr(this);
    }
  }

  public class Unary(Token Operator, Expr right) : Expr
  {
    public Token Operator { get; } = Operator;
    public Expr Right { get; } = right;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitUnaryExpr(this);
    }
  }

  public class Variable(Token name) : Expr
  {
    public Token Name { get; } = name;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitVariableExpr(this);
    }
  }
}
