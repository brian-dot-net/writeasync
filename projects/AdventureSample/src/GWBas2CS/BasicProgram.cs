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
        private readonly List<SyntaxNode> mainStatements;
        private readonly List<SyntaxNode> intrinsics;

        private SyntaxTrivia commentNode;

        public BasicProgram(string name)
        {
            this.name = name;
            this.generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            this.mainStatements = new List<SyntaxNode>();
            this.intrinsics = new List<SyntaxNode>();
        }

        public void AddComment(string comment)
        {
            this.commentNode = SyntaxFactory.Comment("// " + comment);
        }

        public void AddPrint(string expression)
        {
            var callPrint = this.generator.InvocationExpression(SyntaxFactory.IdentifierName("PRINT"), this.generator.LiteralExpression(expression));
            this.mainStatements.Add(callPrint);
            var callConsoleWriteLine = this.generator.MemberAccessExpression(this.generator.IdentifierName("Console"), "WriteLine");
            SyntaxNode[] printStatements = new SyntaxNode[] { this.generator.InvocationExpression(callConsoleWriteLine, this.generator.IdentifierName("expression")) };
            SyntaxNode[] parameters = new SyntaxNode[] { this.generator.ParameterDeclaration("expression", type: this.generator.TypeExpression(SpecialType.System_String)) };
            var printMethod = this.generator.MethodDeclaration("PRINT", accessibility: Accessibility.Private, modifiers: DeclarationModifiers.Static, parameters: parameters, statements: printStatements);
            this.intrinsics.Add(printMethod);
        }

        public void AddGoto(int destination)
        {
            string label = "L" + destination;
            var gotoStatement = SyntaxFactory.GotoStatement(SyntaxKind.GotoStatement, SyntaxFactory.IdentifierName(label));
            this.mainStatements.Add(SyntaxFactory.LabeledStatement(label, gotoStatement));
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
            if (this.commentNode != null)
            {
                lastStatement = lastStatement.WithLeadingTrivia(this.commentNode);
            }

            this.mainStatements.Add(lastStatement);

            var boolType = this.generator.TypeExpression(SpecialType.System_Boolean);
            var mainMethod = this.generator.MethodDeclaration("Main", accessibility: Accessibility.Private, returnType: boolType, statements: this.mainStatements);

            classMembers.AddRange(this.intrinsics);
            classMembers.Add(mainMethod);

            var classDecl = this.generator.ClassDeclaration(this.name, accessibility: Accessibility.Internal, modifiers: DeclarationModifiers.Sealed, members: classMembers);
            return this.generator.CompilationUnit(usingDirectives, classDecl).NormalizeWhitespace().ToString();
        }
    }
}
