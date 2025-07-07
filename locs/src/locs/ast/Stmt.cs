namespace Lox.Ast;

public abstract class Stmt
{
  public abstract R Accept<R>(IVisitor<R> visitor);

  public interface IVisitor<R>
  {
    R VisitBlockStmt(Block stmt);
    R VisitExprssnStmt(Exprssn stmt);
    R VisitIfStmt(If stmt);
    R VisitPrintStmt(Print stmt);
    R VisitVarStmt(Var stmt);
    R VisitWhileStmt(While stmt);
  }

  public class Block(List<Stmt> statements) : Stmt
  {
    public List<Stmt> Statements { get; } = statements;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitBlockStmt(this);
    }
  }

  public class Exprssn(Expr expression) : Stmt
  {
    public Expr Expression { get; } = expression;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitExprssnStmt(this);
    }
  }

  public class If(Expr condition, Stmt thenBranch, Stmt elseBranch) : Stmt
  {
    public Expr Condition { get; } = condition;
    public Stmt ThenBranch { get; } = thenBranch;
    public Stmt ElseBranch { get; } = elseBranch;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitIfStmt(this);
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

  public class While(Expr condition, Stmt body) : Stmt
  {
    public Expr Condition { get; } = condition;
    public Stmt Body { get; } = body;

    public override R Accept<R>(IVisitor<R> visitor)
    {
      return visitor.VisitWhileStmt(this);
    }
  }
}
