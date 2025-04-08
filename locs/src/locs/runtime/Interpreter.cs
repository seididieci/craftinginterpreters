namespace Lox.Runtime;

public class Interpreter : Ast.Expr.IVisitor<object>
{
  public void Interpret(Ast.Expr expr)
  {
    try
    {
      var result = evaluate(expr);
      Console.WriteLine(stringify(result));
    }
    catch (RuntimeError error)
    {
      LoxLanguage.runtimeError(error);
    }
  }

  public object VisitBinaryExpr(Ast.Expr.Binary expr)
  {
    var left = evaluate(expr.Left);
    var right = evaluate(expr.Right);

    switch (expr.Operator.Type)
    {
      case TokenType.MINUS:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left - (double)right;
      case TokenType.STAR:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left * (double)right;
      case TokenType.SLASH:
        checkNumberOperands(expr.Operator, left, right);
        if ((double)right == 0)
          throw new RuntimeError(expr.Operator, "Division by zero.");
        return (double)left / (double)right;
      case TokenType.PLUS:
        if (left is double lnum && right is double rnum)
          return lnum + rnum;
        if (left is string lstring)
        {
          if (right is string || right is double)
            return $"{lstring}{stringify(right)}";
        }
        else if (right is string rstring)
        {
          if (left is string || left is double)
            return $"{stringify(left)}{rstring}";
        }
        throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
      case TokenType.GREATER:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left > (double)right;
      case TokenType.GREATER_EQUAL:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left >= (double)right;
      case TokenType.LESS:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left < (double)right;
      case TokenType.LESS_EQUAL:
        checkNumberOperands(expr.Operator, left, right);
        return (double)left <= (double)right;
      case TokenType.BANG_EQUAL:
        return !isEqual(left, right);
      case TokenType.EQUAL_EQUAL:
        return isEqual(left, right);
      default:
        return null;
    }
  }

  public object VisitGroupingExpr(Ast.Expr.Grouping expr)
  {
    return evaluate(expr.Expression);
  }

  public object VisitLiteralExpr(Ast.Expr.Literal expr)
  {
    return expr.Value;
  }

  public object VisitUnaryExpr(Ast.Expr.Unary expr)
  {
    var right = evaluate(expr.Right);

    switch (expr.Operator.Type)
    {
      case TokenType.MINUS:
        checkNumberOperands(expr.Operator, right);
        return -(double)right;
      case TokenType.BANG:
        return !isTruthy(right);
      default:
        return null;
    }
  }

  private object evaluate(Ast.Expr expr)
  {
    return expr.Accept(this);
  }

  private bool isTruthy(object obj)
  {
    if (obj is null)
      return false;
    if (obj is bool)
      return (bool)obj;

    return true;
  }

  private bool isEqual(object a, object b)
  {
    if (a is null && b is null)
      return true;
    if (a is null)
      return false;

    return a.Equals(b);
  }

  private bool checkNumberOperands(Token op, object right)
  {
    if (right is double)
      return true;
    throw new RuntimeError(op, "Operand must be numbers.");
  }

  private bool checkNumberOperands(Token op, object left, object right)
  {
    if (left is double && right is double)
      return true;
    throw new RuntimeError(op, "Operands must be numbers.");
  }

  private string stringify(object obj)
  {
    if (obj is null)
      return "nil";

    if (obj is double num)
    {
      var text = num.ToString();
      if (text.EndsWith(".0"))
        text = text.Substring(0, text.Length - 2);
      return text;
    }

    return obj.ToString();
  }
}
