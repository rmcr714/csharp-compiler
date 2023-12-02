  namespace Minsk.CodeAnalysis {

  public sealed class ParenthesizedExpressionSyntax: ExpressionSyntax {

        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken,ExpressionSyntax expressionSyntax,
        SyntaxToken closedParenthesisToken) {
           OpenParenthesisToken = openParenthesisToken;
           ExpressionSyntax = expressionSyntax;
           ClosedParenthesisToken = closedParenthesisToken;
           
        }

        public SyntaxToken OpenParenthesisToken {get;}
        public ExpressionSyntax ExpressionSyntax {get;}
        public SyntaxToken ClosedParenthesisToken {get;}

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpressionSyntax;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return ExpressionSyntax;
            yield return ClosedParenthesisToken;
        }
    }

  }