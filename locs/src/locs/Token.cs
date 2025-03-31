namespace Lox;

public enum TokenType
{
  // Single-character tokens.
  LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
  COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

  // One or two character tokens.
  BANG, BANG_EQUAL,
  EQUAL, EQUAL_EQUAL,
  GREATER, GREATER_EQUAL,
  LESS, LESS_EQUAL,

  // Literals.
  IDENTIFIER, STRING, NUMBER,

  // Keywords.
  AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
  PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

  EOF
}

public class Token
{
  public TokenType Type { get; }
  public String Lexeme { get; }
  public Object Literal { get; }
  public int Line { get; }

  public Token(TokenType type, String lexeme, Object literal, int line)
  {
    this.Type = type;
    this.Lexeme = lexeme;
    this.Literal = literal;
    this.Line = line;
  }

  public override String ToString()
  {
    return Type + " " + Lexeme + " " + Literal;
  }
}

