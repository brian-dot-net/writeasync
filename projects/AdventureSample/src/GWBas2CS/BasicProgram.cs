// <copyright file="BasicProgram.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Editing;

    internal sealed class BasicProgram
    {
        private readonly string name;
        private readonly SyntaxGenerator generator;
        private readonly List<SyntaxNode> intrinsics;
        private readonly Lines lines;

        public BasicProgram(string name)
        {
            this.name = name;
            this.generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            this.lines = new Lines();
            this.intrinsics = new List<SyntaxNode>();
        }

        public void AddComment(int line, string comment)
        {
            this.lines.AddComment(line, SyntaxFactory.Comment("// " + comment));
        }

        public void AddPrint(int line, string expression)
        {
            var callPrint = this.generator.InvocationExpression(SyntaxFactory.IdentifierName("PRINT"), this.generator.LiteralExpression(expression));
            this.lines.Add(line, callPrint);
            var callConsoleWriteLine = this.generator.MemberAccessExpression(this.generator.IdentifierName("Console"), "WriteLine");
            SyntaxNode[] printStatements = new SyntaxNode[] { this.generator.InvocationExpression(callConsoleWriteLine, this.generator.IdentifierName("expression")) };
            SyntaxNode[] parameters = new SyntaxNode[] { this.generator.ParameterDeclaration("expression", type: this.generator.TypeExpression(SpecialType.System_String)) };
            var printMethod = this.generator.MethodDeclaration("PRINT", accessibility: Accessibility.Private, modifiers: DeclarationModifiers.Static, parameters: parameters, statements: printStatements);
            this.intrinsics.Add(printMethod);
        }

        public void AddGoto(int line, int destination)
        {
            this.lines.AddGoto(line, destination);
        }

        public override string ToString()
        {
            var usingDirectives = this.generator.NamespaceImportDeclaration("System");

            List<SyntaxNode> classMembers = new List<SyntaxNode>();

            var runCoreMember = this.generator.MemberAccessExpression(this.generator.ThisExpression(), "Main");
            var callRunCore = this.generator.InvocationExpression(runCoreMember);
            List<SyntaxNode> runStatements = new List<SyntaxNode>();
            var whileLoop = this.generator.WhileStatement(callRunCore, null);
            runStatements.Add(whileLoop);
            var runMethod = this.generator.MethodDeclaration("Run", accessibility: Accessibility.Public, statements: runStatements);

            classMembers.Add(runMethod);

            var lastStatement = this.generator.ReturnStatement(this.generator.LiteralExpression(false));
            this.lines.Add(65535, lastStatement);

            var boolType = this.generator.TypeExpression(SpecialType.System_Boolean);
            var mainMethod = this.generator.MethodDeclaration("Main", accessibility: Accessibility.Private, returnType: boolType, statements: this.lines.Statements());

            classMembers.AddRange(this.intrinsics);
            classMembers.Add(mainMethod);

            var classDecl = this.generator.ClassDeclaration(this.name, accessibility: Accessibility.Internal, modifiers: DeclarationModifiers.Sealed, members: classMembers);
            return this.generator.CompilationUnit(usingDirectives, classDecl).NormalizeWhitespace().ToString();
        }

        private sealed class Lines
        {
            private readonly List<Line> statements;
            private readonly HashSet<int> references;

            public Lines()
            {
                this.statements = new List<Line>();
                this.references = new HashSet<int>();
            }

            public void Add(int line, SyntaxNode node)
            {
                this.statements.Add(new Line(line, node, null));
            }

            public void AddComment(int line, SyntaxTrivia comment)
            {
                this.statements.Add(new Line(line, null, comment));
            }

            public void AddGoto(int line, int destination)
            {
                var label = SyntaxFactory.IdentifierName(Label(destination));
                var gotoStatement = SyntaxFactory.GotoStatement(SyntaxKind.GotoStatement, label);
                this.Add(line, gotoStatement);
                this.references.Add(destination);
            }

            public IEnumerable<SyntaxNode> Statements()
            {
                SyntaxTrivia? previous = null;
                foreach (Line line in this.statements)
                {
                    SyntaxTrivia? next = line.Comment;
                    if (next == null)
                    {
                        foreach (SyntaxNode node in line.Nodes(this.references, previous))
                        {
                            yield return node;
                        }
                    }

                    previous = next;
                }
            }

            private static string Label(int number)
            {
                return "L" + number;
            }

            private sealed class Line
            {
                private readonly int number;
                private readonly SyntaxNode node;
                private readonly SyntaxTrivia? comment;

                public Line(int number, SyntaxNode node, SyntaxTrivia? comment)
                {
                    this.number = number;
                    this.node = node;
                    this.comment = comment;
                }

                public SyntaxTrivia? Comment => this.comment;

                public IEnumerable<SyntaxNode> Nodes(ISet<int> references, SyntaxTrivia? previous)
                {
                    SyntaxNode first = null;
                    SyntaxNode second = null;
                    if (references.Contains(this.number))
                    {
                        first = SyntaxFactory.LabeledStatement(Label(this.number), SyntaxFactory.EmptyStatement());
                        second = this.node;
                    }
                    else
                    {
                        first = this.node;
                    }

                    if (previous.HasValue)
                    {
                        first = first.WithLeadingTrivia(previous.Value);
                    }

                    yield return first;
                    if (second != null)
                    {
                        yield return second;
                    }
                }
            }
        }
    }
}
