
namespace Minsk.CodeAnalysis {
//This is just a wrapper to hold the expressionSyntax. ExpressionSyntax contains the tree and everything we need
    public sealed class SyntaxTree {

    public SyntaxTree(IEnumerable<string> diagnostics,ExpressionSyntax root, SyntaxToken endOfFileToken) {
        Diagnostics = diagnostics.ToArray();
        Root =root;
        EndOfFileToken = endOfFileToken;

    }

    public ExpressionSyntax Root { get;}
    public SyntaxToken EndOfFileToken {get;}
    public IReadOnlyList<string> Diagnostics {get;}
    public static SyntaxTree Parse(string text) {
        var parser = new Parser(text);
        return parser.Parse();
    }

}

}