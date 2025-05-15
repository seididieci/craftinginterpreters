using System.Text;

namespace Lox.Ast;

public class AstPrinter :
  Expr.IVisitor<string>,
  Stmt.IVisitor<string>
{
  public string Print(List<Stmt> statements)
  {
    StringBuilder builder = new StringBuilder();
    foreach (Stmt statement in statements)
      builder.AppendLine(statement.Accept(this));

    return builder.ToString();
  }

  public string VisitBinaryExpr(Expr.Binary expr)
  {
    return parenthesize(expr.Operator.Lexeme,
                            expr.Left, expr.Right);
  }

  public string VisitGroupingExpr(Expr.Grouping expr)
  {
    return parenthesize("group", expr.Expression);
  }

  public string VisitVariableExpr(Expr.Variable expr)
  {
    return expr.Name.Lexeme;
  }

  public string VisitLiteralExpr(Expr.Literal expr)
  {
    if (expr.Value is null)
      return "nil";

    return expr.Value.ToString();
  }

  public string VisitUnaryExpr(Expr.Unary expr)
  {
    return parenthesize(expr.Operator.Lexeme, expr.Right);
  }

  public string VisitAssignExpr(Expr.Assign expr)
  {
    return parenthesize($"{expr.Name.Lexeme} =", expr.Value);
  }

  private String parenthesize(String name, params Expr[] exprs)
  {
    StringBuilder builder = new StringBuilder();

    builder.Append("(").Append(name);
    foreach (Expr expr in exprs)
    {
      builder.Append(" ");
      builder.Append(expr.Accept(this));
    }
    builder.Append(")");

    return builder.ToString();
  }

  public string VisitExprssnStmt(Stmt.Exprssn stmt)
  {
    return parenthesize("", stmt.Expression);
  }

  public string VisitPrintStmt(Stmt.Print stmt)
  {
    return parenthesize("print", stmt.Expression);
  }

  public string VisitVarStmt(Stmt.Var stmt)
  {
    return parenthesize($"var {stmt.Name.Lexeme} =", stmt.Initializer ?? new Expr.Literal(null));
  }
}
