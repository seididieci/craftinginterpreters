using System.Text;

namespace Lox.Ast;

public class AstPrinter : Expr.IVisitor<string>
{
  public string Print(Expr expr)
  {
    return expr.Accept(this);
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
}
