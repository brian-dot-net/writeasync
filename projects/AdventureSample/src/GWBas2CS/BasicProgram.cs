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

        private SyntaxTrivia commentNode;

        public BasicProgram(string name)
        {
            this.name = name;
        }

        public void AddComment(string comment)
        {
            this.commentNode = SyntaxFactory.Comment("// " + comment);
        }

        public override string ToString()
        {
            var workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var usingDirectives = generator.NamespaceImportDeclaration("System");

            List<SyntaxNode> classMembers = new List<SyntaxNode>();

            var runCoreMember = generator.MemberAccessExpression(generator.ThisExpression(), "RunCore");
            var callRunCore = generator.InvocationExpression(runCoreMember);
            List<SyntaxNode> runStatements = new List<SyntaxNode>();
            var whileLoop = generator.WhileStatement(callRunCore, null);
            runStatements.Add(whileLoop);
            var runMethod = generator.MethodDeclaration("Run", accessibility: Accessibility.Public, statements: runStatements);

            classMembers.Add(runMethod);

            List<SyntaxNode> runCoreStatements = new List<SyntaxNode>();
            var lastStatement = generator.ReturnStatement(generator.LiteralExpression(false));
            runCoreStatements.Add(lastStatement.WithLeadingTrivia(this.commentNode));
            var boolType = generator.TypeExpression(SpecialType.System_Boolean);
            var runCoreMethod = generator.MethodDeclaration("RunCore", accessibility: Accessibility.Private, returnType: boolType, statements: runCoreStatements);

            classMembers.Add(runCoreMethod);

            var classDecl = generator.ClassDeclaration(this.name, accessibility: Accessibility.Internal, modifiers: DeclarationModifiers.Sealed, members: classMembers);
            return generator.CompilationUnit(usingDirectives, classDecl).NormalizeWhitespace().ToString();
        }
    }
}
