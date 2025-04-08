namespace Lox;

public class Scanner(string source)
{
  private List<Token> tokens = new List<Token>();
  private int start = 0;
  private int current = 0;
  private int line = 1;

  private static Dictionary<String, TokenType> keywords = new Dictionary<String, TokenType> {
    {"and",    TokenType.AND},
    {"class",  TokenType.CLASS},
    {"else",   TokenType.ELSE},
    {"false",  TokenType.FALSE},
    {"for",    TokenType.FOR},
    {"fun",    TokenType.FUN},
    {"if",     TokenType.IF},
    {"nil",    TokenType.NIL},
    {"or",     TokenType.OR},
    {"print",  TokenType.PRINT},
    {"return", TokenType.RETURN},
    {"super",  TokenType.SUPER},
    {"this",   TokenType.THIS},
    {"true",   TokenType.TRUE},
    {"var",    TokenType.VAR},
    {"while",  TokenType.WHILE}
  };

  public List<Token> ScanTokens()
  {
    while (!isAtEnd())
    {
      start = current;
      scanToken();
    }

    tokens.Add(new Token(TokenType.EOF, "", null, line));
    return tokens;
  }

  private Boolean isAtEnd()
  {
    return current >= source.Length;
  }

  private void scanToken()
  {
    char c = advance();
    switch (c)
    {
      case '(': addToken(TokenType.LEFT_PAREN); break;
      case ')': addToken(TokenType.RIGHT_PAREN); break;
      case '{': addToken(TokenType.LEFT_BRACE); break;
      case '}': addToken(TokenType.RIGHT_BRACE); break;
      case ',': addToken(TokenType.COMMA); break;
      case '.': addToken(TokenType.DOT); break;
      case '-': addToken(TokenType.MINUS); break;
      case '+': addToken(TokenType.PLUS); break;
      case ';': addToken(TokenType.SEMICOLON); break;
      case '*': addToken(TokenType.STAR); break;
      case '!':
        addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
        break;
      case '=':
        addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
        break;
      case '<':
        addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
        break;
      case '>':
        addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
        break;
      case '/':
        if (match('/'))
        {
          // A comment goes until the end of the line.
          while (peek() != '\n' && !isAtEnd())
            advance();
        }
        else if (match('*'))
        {
          advance();
          while (!isAtEnd())
          {
            var cc = advance();
            if (cc == '*' && peek() == '/')
            {
              advance();
              break;
            }
            else if (cc == '\n')
              line++;
          }
        }
        else
          addToken(TokenType.SLASH);
        break;
      case ' ':
      case '\r':
      case '\t':
        // Ignore whitespace.
        break;

      case '\n':
        line++;
        break;

      case '"': scan_string(); break;

      default:
        if (isDigit(c))
          number();
        else if (isAlpha(c))
          identifier();
        else
          LoxLanguage.error(line, "Unexpected character.");
        break;
    }
  }

  private Boolean isDigit(char c)
  {
    return c >= '0' && c <= '9';
  }

  private char advance()
  {
    return source.ElementAt(current++);
  }

  private void addToken(TokenType type)
  {
    addToken(type, null);
  }

  private void addToken(TokenType type, Object literal)
  {
    String text = source.Substring(start, current - start);
    tokens.Add(new Token(type, text, literal, line));
  }

  private Boolean match(char expected)
  {
    if (isAtEnd())
      return false;
    if (source.ElementAt(current) != expected) return false;

    current++;
    return true;
  }

  private char peek()
  {
    if (isAtEnd())
      return '\0';
    return source.ElementAt(current);
  }

  private char peekNext()
  {
    if (current + 1 >= source.Length) return '\0';
    return source.ElementAt(current + 1);
  }

  private void scan_string()
  {
    while (peek() != '"' && !isAtEnd())
    {
      if (peek() == '\n')
        line++;
      advance();
    }

    if (isAtEnd())
    {
      LoxLanguage.error(line, "Unterminated string.");
      return;
    }

    // The closing ".
    advance();

    // Trim the surrounding quotes.
    String value = source.Substring(start + 1, current - start - 2);
    addToken(TokenType.STRING, value);
  }

  private void number()
  {
    while (isDigit(peek())) advance();

    // Look for a fractional part.
    if (peek() == '.' && isDigit(peekNext()))
    {
      // Consume the "."
      advance();

      while (isDigit(peek()))
        advance();
    }

    addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
  }

  private void identifier()
  {
    while (isAlphaNumeric(peek()))
      advance();
    try
    {
      String text = source.Substring(start, current - start);
      if (!keywords.TryGetValue(text, out TokenType type))
        type = TokenType.IDENTIFIER;

      addToken(type);
    }
    catch (Exception)
    {
      Console.WriteLine($"current: {current}, start: {start}");
      throw;
    }
  }

  private Boolean isAlpha(char c)
  {
    return (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
            c == '_';
  }

  private Boolean isAlphaNumeric(char c)
  {
    return isAlpha(c) || isDigit(c);
  }


}
