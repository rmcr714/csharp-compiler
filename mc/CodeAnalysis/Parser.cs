using System.Diagnostics.Tracing;

namespace Minsk.CodeAnalysis {

internal sealed class Parser {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        

        public Parser(string text) {

         var tokens = new List<SyntaxToken>();

         var lexer = new Lexer(text);

          SyntaxToken token;

          do {
            token = lexer.NextToken();
            if(token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken)
               tokens.Add(token);


            } while(token.Kind != SyntaxKind.EndOfFileToken);

           _tokens = tokens.ToArray();
           _diagnostics.AddRange(lexer.Diagnostics);
    }

    public IEnumerable<string> Diagnostics => _diagnostics;

    private SyntaxToken Peek(int offset) {
        var index = _position + offset;
        if(index >= _tokens.Length) 
           return _tokens[_tokens.Length - 1];

           return _tokens[index];

    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken() {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken Match(SyntaxKind kind) {
        if( Current.Kind == kind ) 
            return NextToken();

        _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> expected <{kind}>");

       return new SyntaxToken(kind,Current.Position, null,null);
    }

    public SyntaxTree Parse() {

        var expression = ParseExpression();
       var endOfFileToken =  Match(SyntaxKind.EndOfFileToken);

       return new SyntaxTree(_diagnostics,expression,endOfFileToken);
        

    }

    private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {

        ExpressionSyntax left;
        
        var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

        if(unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence){
            var operatorToken = NextToken();
            var operand = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken,operand);

        }else{
             left = ParsePrimaryExpression();

        }

        while(true) {
            var precedence = Current.Kind.GetBinaryOperatorPrecedence();
            if(precedence == 0 || precedence <= parentPrecedence ) {
                break;
            }
            
            var operatorToken = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpressionSyntax(left,operatorToken,right);
        }

        return left ; 

    }

    private static int BinaryOperatorPrecedence(SyntaxKind kind) {

        switch(kind) {
            case SyntaxKind.PlusToken :
            case SyntaxKind.MinusToken :
                return 1;
            case SyntaxKind.StarToken :
            case SyntaxKind.SlashToken :
                return 2;
            default:
                return 0;
        }

    }

    

    

    

    public ExpressionSyntax ParsePrimaryExpression() {

        if(Current.Kind == SyntaxKind.OpenParenthesisToke) {
            var left = NextToken();
            var expression = ParseExpression();
            var right = Match(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left,expression,right);
        }

        var numberToken = Match(SyntaxKind.NumberToken);
        return new NumberExpressionSyntax(numberToken);

    }


}
}
