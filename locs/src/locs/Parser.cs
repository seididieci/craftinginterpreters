namespace Lox;

public class Parser(List<Token> tokens)
{
  private int current = 0;

  private class ParseError : Exception { }

  public List<Ast.Stmt> Parse()
  {
    List<Ast.Stmt> statements = new List<Ast.Stmt>();
    try
    {
      while (!IsAtEnd())
        statements.Add(Declaration());
    }
    catch (ParseError)
    {
      return new List<Ast.Stmt>();
    }

    return statements;
  }

  private Ast.Stmt Declaration()
  {
    try
    {
      if (Match(TokenType.VAR))
        return VarDeclaration();
      return Statement();
    }
    catch (ParseError)
    {
      Synchronize();
      return null;
    }
  }

  private Ast.Stmt VarDeclaration()
  {
    Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

    Ast.Expr initializer = null;
    if (Match(TokenType.EQUAL))
      initializer = Expression();

    Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
    return new Ast.Stmt.Var(name, initializer);
  }

  private Ast.Stmt Statement()
  {
    if (Match(TokenType.PRINT))
      return PrintStatement();

    return ExpressionStatement();
  }

  private Ast.Stmt ExpressionStatement()
  {
    Ast.Expr expr = Expression();
    Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
    return new Ast.Stmt.Exprssn(expr);
  }

  private Ast.Stmt PrintStatement()
  {
    Ast.Expr expr = Expression();
    Consume(TokenType.SEMICOLON, "Expect ';' after value.");
    return new Ast.Stmt.Print(expr);
  }

  private Ast.Expr Expression()
  {
    return Assignment();
  }

  private Ast.Expr Assignment()
  {
    Ast.Expr expr = Equalty();

    if (Match(TokenType.EQUAL))
    {
      Token equals = Previous();
      Ast.Expr value = Assignment();

      if (expr is Ast.Expr.Variable variable)
        return new Ast.Expr.Assign(variable.Name, value);

      Error(equals, "Invalid assignment target.");
    }

    return expr;
  }

  private Ast.Expr Equalty()
  {
    Ast.Expr expr = Comparison();

    while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
    {
      Token Operator = Previous();

      Ast.Expr right = Comparison();

      expr = new Ast.Expr.Binary(expr, Operator, right);
    }

    return expr;
  }

  private Ast.Expr Comparison()
  {
    Ast.Expr expr = Term();

    while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
    {
      Token _operator = Previous();
      Ast.Expr right = Term();
      expr = new Ast.Expr.Binary(expr, _operator, right);
    }

    return expr;
  }

  private Ast.Expr Term()
  {
    Ast.Expr expr = Factor();

    while (Match(TokenType.PLUS, TokenType.MINUS))
    {
      Token _operator = Previous();
      Ast.Expr right = Factor();
      expr = new Ast.Expr.Binary(expr, _operator, right);
    }

    return expr;
  }

  private Ast.Expr Factor()
  {
    Ast.Expr expr = Unary();

    while (Match(TokenType.SLASH, TokenType.STAR))
    {
      Token _operator = Previous();
      Ast.Expr right = Unary();
      expr = new Ast.Expr.Binary(expr, _operator, right);
    }

    return expr;
  }

  private Ast.Expr Unary()
  {
    if (Match(TokenType.MINUS, TokenType.BANG))
    {
      Token _operator = Previous();
      Ast.Expr right = Unary();
      return new Ast.Expr.Unary(_operator, right);
    }
    return Primary();
  }

  private Ast.Expr Primary()
  {
    if (Match(TokenType.FALSE))
      return new Ast.Expr.Literal(false);
    if (Match(TokenType.TRUE))
      return new Ast.Expr.Literal(true);
    if (Match(TokenType.NIL))
      return new Ast.Expr.Literal(null);

    if (Match(TokenType.NUMBER, TokenType.STRING))
      return new Ast.Expr.Literal(Previous().Literal);

    if (Match(TokenType.IDENTIFIER))
      return new Ast.Expr.Variable(Previous());

    if (Match(TokenType.LEFT_PAREN))
    {
      Ast.Expr expr = Expression();
      Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
      return new Ast.Expr.Grouping(expr);
    }

    throw Error(Peek(), "Expect expression.");
  }

  private Token Consume(TokenType type, String message)
  {
    if (Check(type))
      return Advance();

    throw Error(Peek(), message);
  }

  private void Synchronize()
  {
    Advance();

    while (!IsAtEnd())
    {
      if (Previous().Type == TokenType.SEMICOLON)
        return;

      switch (Peek().Type)
      {
        case TokenType.CLASS:
        case TokenType.FUN:
        case TokenType.VAR:
        case TokenType.FOR:
        case TokenType.IF:
        case TokenType.WHILE:
        case TokenType.PRINT:
        case TokenType.RETURN:
          return;
      }

      Advance();
    }
  }

  private ParseError Error(Token token, String message)
  {
    LoxLanguage.error(token, message);
    return new ParseError();
  }

  private Boolean Match(params TokenType[] types)
  {
    foreach (TokenType type in types)
    {
      if (Check(type))
      {
        Advance();
        return true;
      }
    }
    return false;
  }

  private Boolean Check(TokenType type)
  {
    if (IsAtEnd())
      return false;

    return Peek().Type == type;
  }

  private Token Advance()
  {
    if (!IsAtEnd())
      current++;

    return Previous();
  }

  private Boolean IsAtEnd()
  {
    return Peek().Type == TokenType.EOF;
  }

  private Token Peek()
  {
    return tokens[current];
  }

  private Token Previous()
  {
    return tokens[current - 1];
  }
}
