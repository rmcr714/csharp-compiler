// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk {


    internal static class Program {

       private static void Main() {

        
        var showTree = false;
        while(true) {

            Console.Write("-> ");
            var line = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(line)) 
            return;

            if(line == "#showtree") {
                showTree = !showTree;
                Console.WriteLine(showTree?"Showing parse Trees " : "Not showing parse trees");
                continue;
            }else if(line == "#clear" || line == "#cls") {
                Console.Clear();
                continue;
            }else if (line == "q"|| line == "quit") {
                break;
            }
            
           
            var syntaxTree = SyntaxTree.Parse(line);

            if(showTree) {

                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrettyPrint(syntaxTree.Root);
                Console.ResetColor(); 
            }

           if(!syntaxTree.Diagnostics.Any()) {
            
                var e = new Evaluator(syntaxTree.Root);
                var result = e.Evaluate();
                Console.WriteLine(result);

           }else {

                Console.ForegroundColor = ConsoleColor.DarkRed;
                
                foreach(var diagnostics in syntaxTree.Diagnostics) {
                Console.WriteLine(diagnostics);
            }
             
             Console.ResetColor(); 

           }
        }


       }

       static void PrettyPrint( SyntaxNode node, string indent = "", bool isLast = true) {
 
        var marker  = isLast ? "└──" : "├──";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if(node is SyntaxToken t && t.Value != null ) {
         Console.Write(" ");
         Console.Write(t.Value);

        }

        Console.WriteLine(" ");

        indent += isLast? "    ":"|   ";

        var lastChild = node.GetChildren().LastOrDefault();
        
        foreach(var child in node.GetChildren()) {
            PrettyPrint(child,indent, child == lastChild);

        } 

       }



    }

  
    }

