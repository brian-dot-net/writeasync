// <copyright file="SourceCodeStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Editing;

    public sealed class SourceCodeStream : IDisposable
    {
        private readonly StreamReader reader;

        public SourceCodeStream(Stream input)
        {
            this.reader = new StreamReader(input);
        }

        public async Task TranslateAsync(string name, Stream output)
        {
            string line = await this.reader.ReadLineAsync();
            string[] numberAndStatement = line.Split(new char[] { ' ' }, 2);
            string[] keywordAndRest = numberAndStatement[1].Split(new char[] { ' ' }, 2);
            string comment = keywordAndRest[1];

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
            var commentNode = SyntaxFactory.Comment("// " + comment);
            runCoreStatements.Add(generator.ReturnStatement(generator.LiteralExpression(false)).WithLeadingTrivia(commentNode));
            var boolType = generator.TypeExpression(SpecialType.System_Boolean);
            var runCoreMethod = generator.MethodDeclaration("RunCore", accessibility: Accessibility.Private, returnType: boolType, statements: runCoreStatements);

            classMembers.Add(runCoreMethod);

            var classDecl = generator.ClassDeclaration(name, accessibility: Accessibility.Internal, modifiers: DeclarationModifiers.Sealed, members: classMembers);
            var outputCode = generator.CompilationUnit(usingDirectives, classDecl).NormalizeWhitespace();
            byte[] rawOutput = Encoding.UTF8.GetBytes(outputCode.ToString());
            await output.WriteAsync(rawOutput, 0, rawOutput.Length);
        }

        public void Dispose()
        {
            this.reader.Close();
        }
    }
}
