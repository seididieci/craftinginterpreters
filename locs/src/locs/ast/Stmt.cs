namespace Lox.Ast;

public abstract class Stmt
{
  public abstract R Accept<R>(IVisitor<R> visitor);

  public interface IVisitor<R>
  {
    R VisitExprssnStmt(Exprssn stmt);
    R VisitPrintStmt(Print stmt);
    R VisitVarStmt(Var stmt);
  }

  public class Exprssn(Expr expression) : Stmt
  {
    public Expr Expression { get; } = expression;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitExprssnStmt(this);
    }
  }

  public class Print(Expr expression) : Stmt
  {
    public Expr Expression { get; } = expression;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitPrintStmt(this);
    }
  }

  public class Var(Token name, Expr initializer) : Stmt
  {
    public Token Name { get; } = name;
    public Expr Initializer { get; } = initializer;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitVarStmt(this);
    }
  }
}
