
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis {

    public sealed class Evaluator {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root) {
           _root = root;

         }

  public int Evaluate() {

    return EvaluateExpression(_root) ;

    }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            if(node is NumberExpressionSyntax n) {
                return (int)n.NumberToken.Value;
            }

            if(node is UnaryExpressionSyntax u) {
               var operand = EvaluateExpression(u.Operand);

               if(u.OperatorToken.Kind == SyntaxKind.MinusToken) {
                return  -operand;
               } else if(u.OperatorToken.Kind == SyntaxKind.PlusToken) {
                return  operand;
               } else {

                 throw new Exception($"Unexpected unary operatot : ${u.OperatorToken.Kind} ");
               }
            }

            if(node is BinaryExpressionSyntax b) {

                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if(b.OperatorToken.Kind == SyntaxKind.PlusToken){
                    return left+right;
                }else if(b.OperatorToken.Kind == SyntaxKind.MinusToken){
                    return left-right;
                }else if(b.OperatorToken.Kind == SyntaxKind.StarToken){
                    return left*right;
                }else if(b.OperatorToken.Kind == SyntaxKind.SlashToken){
                    return left/right;
                }else {
                    throw new Exception($"Unexpected binary operatot : ${b.OperatorToken.Kind} ");
                }

            }

            if(node is ParenthesizedExpressionSyntax p ){
                return EvaluateExpression(p.ExpressionSyntax);
            }

            throw new Exception($"Unexpected Node {node.Kind} ");
        }
    }

}
