// <copyright file="BasicVisitor.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GWParse.Expressions;
    using GWParse.Statements;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Editing;

    internal sealed class BasicVisitor : ILineVisitor, IStatementVisitor
    {
        private readonly string name;
        private readonly SyntaxGenerator generator;
        private readonly List<SyntaxNode> intrinsics;
        private readonly Lines lines;
        private readonly Variables vars;

        private int lineNumber;

        public BasicVisitor(string name)
        {
            this.name = name;
            this.generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            this.lines = new Lines();
            this.intrinsics = new List<SyntaxNode>();
            this.vars = new Variables(this.generator);
        }

        public override string ToString()
        {
            List<SyntaxNode> declarations = new List<SyntaxNode>();

            this.Usings(declarations);

            this.Class(declarations);

            return this.generator.CompilationUnit(declarations).NormalizeWhitespace().ToString();
        }

        public void Line(int number, BasicStatement[] list)
        {
            this.lineNumber = number;
            foreach (BasicStatement stmt in list)
            {
                stmt.Accept(this);
            }
        }

        public void Assign(BasicExpression left, BasicExpression right)
        {
            ExpressionNode x = new ExpressionNode(this.generator, this.vars);
            left.Accept(x);
            ExpressionNode y = new ExpressionNode(this.generator, this.vars);
            right.Accept(y);
            this.lines.Add(this.lineNumber, this.generator.AssignmentStatement(x.Value, y.Value));
        }

        public void For(BasicExpression v, BasicExpression start, BasicExpression end, BasicExpression step)
        {
            throw new NotImplementedException();
        }

        public void Go(string name, int dest)
        {
            switch (name)
            {
                case "Goto":
                    this.AddGoto(dest);
                    break;
                default:
                    throw new NotImplementedException("Go:" + name);
            }
        }

        public void IfThen(BasicExpression cond, BasicStatement ifTrue)
        {
            throw new NotImplementedException();
        }

        public void Input(string prompt, BasicExpression v)
        {
            throw new NotImplementedException();
        }

        public void Many(string name, BasicExpression[] list)
        {
            switch (name)
            {
                case "Print":
                    this.AddPrint(list[0]);
                    break;
                default:
                    throw new NotImplementedException("Many:" + name);
            }
        }

        public void Remark(string text)
        {
            this.lines.AddComment(this.lineNumber, SyntaxFactory.Comment("// " + text));
        }

        public void Void(string name)
        {
            throw new NotImplementedException();
        }

        private void Usings(IList<SyntaxNode> declarations)
        {
            declarations.Add(this.generator.NamespaceImportDeclaration("System"));
            declarations.Add(this.generator.NamespaceImportDeclaration("System.IO"));
        }

        private void Class(IList<SyntaxNode> declarations)
        {
            List<SyntaxNode> classMembers = new List<SyntaxNode>();

            this.Fields(classMembers);
            this.Constructor(classMembers);
            this.RunMethod(classMembers);
            classMembers.Add(this.vars.Init());
            classMembers.AddRange(this.intrinsics);
            this.MainMethod(classMembers);

            var classDecl = this.generator.ClassDeclaration(
                this.name,
                accessibility: Accessibility.Internal,
                modifiers: DeclarationModifiers.Sealed,
                members: classMembers);

            declarations.Add(classDecl);
        }

        private void MainMethod(List<SyntaxNode> classMembers)
        {
            var firstStatement = this.generator.InvocationExpression(this.generator.MemberAccessExpression(this.generator.ThisExpression(), "Init"));
            this.lines.Add(0, firstStatement);
            var lastStatement = this.generator.ReturnStatement(this.generator.LiteralExpression(false));
            this.lines.Add(65535, lastStatement);

            var boolType = this.generator.TypeExpression(SpecialType.System_Boolean);
            var mainMethod = this.generator.MethodDeclaration("Main", accessibility: Accessibility.Private, returnType: boolType, statements: this.lines.Statements());
            classMembers.Add(mainMethod);
        }

        private void RunMethod(List<SyntaxNode> classMembers)
        {
            var runCoreMember = this.generator.MemberAccessExpression(this.generator.ThisExpression(), "Main");
            var callRunCore = this.generator.InvocationExpression(runCoreMember);
            List<SyntaxNode> runStatements = new List<SyntaxNode>();
            var whileLoop = this.generator.WhileStatement(callRunCore, null);
            runStatements.Add(whileLoop);
            var runMethod = this.generator.MethodDeclaration("Run", accessibility: Accessibility.Public, statements: runStatements);

            classMembers.Add(runMethod);
        }

        private void Constructor(List<SyntaxNode> classMembers)
        {
            List<SyntaxNode> ctorStatements = new List<SyntaxNode>();
            var thisInput = this.generator.MemberAccessExpression(this.generator.ThisExpression(), "input");
            var assignInput = this.generator.AssignmentStatement(thisInput, this.generator.IdentifierName("input"));
            ctorStatements.Add(assignInput);
            var thisOutput = this.generator.MemberAccessExpression(this.generator.ThisExpression(), "output");
            var assignOutput = this.generator.AssignmentStatement(thisOutput, this.generator.IdentifierName("output"));
            ctorStatements.Add(assignOutput);
            List<SyntaxNode> ctorParams = new List<SyntaxNode>();
            ctorParams.Add(this.generator.ParameterDeclaration("input", type: this.generator.IdentifierName("TextReader")));
            ctorParams.Add(this.generator.ParameterDeclaration("output", type: this.generator.IdentifierName("TextWriter")));
            var ctorMethod = this.generator.ConstructorDeclaration(accessibility: Accessibility.Public, parameters: ctorParams, statements: ctorStatements);

            classMembers.Add(ctorMethod);
        }

        private void Fields(List<SyntaxNode> classMembers)
        {
            classMembers.Add(this.generator.FieldDeclaration("input", this.generator.IdentifierName("TextReader"), accessibility: Accessibility.Private, modifiers: DeclarationModifiers.ReadOnly));
            classMembers.Add(this.generator.FieldDeclaration("output", this.generator.IdentifierName("TextWriter"), accessibility: Accessibility.Private, modifiers: DeclarationModifiers.ReadOnly));
            classMembers.AddRange(this.vars.Fields());
        }

        private void AddGoto(int destination)
        {
            this.lines.AddGoto(this.lineNumber, destination);
        }

        private void AddPrint(BasicExpression expr)
        {
            ExpressionNode node = new ExpressionNode(this.generator, this.vars);
            expr.Accept(node);
            var callPrint = this.generator.InvocationExpression(SyntaxFactory.IdentifierName("PRINT"), node.Value);
            this.lines.Add(this.lineNumber, callPrint);
            var output = this.generator.MemberAccessExpression(this.generator.ThisExpression(), this.generator.IdentifierName("output"));
            var callConsoleWriteLine = this.generator.MemberAccessExpression(output, "WriteLine");
            SyntaxNode[] printStatements = new SyntaxNode[] { this.generator.InvocationExpression(callConsoleWriteLine, this.generator.IdentifierName("expression")) };
            SyntaxNode[] parameters = new SyntaxNode[] { this.generator.ParameterDeclaration("expression", type: this.generator.TypeExpression(SpecialType.System_String)) };
            var printMethod = this.generator.MethodDeclaration("PRINT", accessibility: Accessibility.Private, parameters: parameters, statements: printStatements);
            this.intrinsics.Add(printMethod);
        }

        private sealed class ExpressionNode : IExpressionVisitor
        {
            private readonly SyntaxGenerator generator;
            private readonly Variables vars;

            public ExpressionNode(SyntaxGenerator generator, Variables vars)
            {
                this.generator = generator;
                this.vars = vars;
            }

            public SyntaxNode Value { get; private set; }

            public void Array(BasicType type, string name, BasicExpression[] subs)
            {
                throw new NotImplementedException();
            }

            public void Literal(BasicType type, object o)
            {
                this.Value = this.generator.LiteralExpression(o);
            }

            public void Operator(string name, BasicExpression[] operands)
            {
                throw new NotImplementedException();
            }

            public void Variable(BasicType type, string name)
            {
                this.Value = this.vars.Add(type, name);
            }
        }

        private sealed class Variables
        {
            private readonly SyntaxGenerator generator;
            private readonly Dictionary<string, Variable> strs;
            private readonly Dictionary<string, Variable> nums;

            public Variables(SyntaxGenerator generator)
            {
                this.generator = generator;
                this.strs = new Dictionary<string, Variable>();
                this.nums = new Dictionary<string, Variable>();
            }

            private IEnumerable<Variable> All => this.strs.Values.Concat(this.nums.Values);

            public IEnumerable<SyntaxNode> Fields()
            {
                foreach (Variable v in this.All)
                {
                    yield return v.Field();
                }
            }

            public SyntaxNode Init()
            {
                List<SyntaxNode> statements = new List<SyntaxNode>();
                foreach (Variable v in this.All)
                {
                    statements.Add(v.Init());
                }

                return this.generator.MethodDeclaration("Init", accessibility: Accessibility.Private, statements: statements);
            }

            public SyntaxNode Add(BasicType type, string name)
            {
                if (type == BasicType.Str)
                {
                    return this.Add(this.strs, type, name);
                }

                return this.Add(this.nums, type, name);
            }

            private SyntaxNode Add(IDictionary<string, Variable> vars, BasicType type, string name)
            {
                Variable v;
                if (!vars.TryGetValue(name, out v))
                {
                    v = new Variable(this.generator, type, name);
                    vars.Add(name, v);
                }

                return v.Ref();
            }

            private sealed class Variable
            {
                private readonly SyntaxGenerator generator;
                private readonly BasicType type;
                private readonly string name;

                public Variable(SyntaxGenerator generator, BasicType type, string name)
                {
                    this.generator = generator;
                    this.type = type;
                    this.name = name;
                }

                private SyntaxNode Type
                {
                    get
                    {
                        SpecialType ty = SpecialType.System_Single;
                        if (this.type == BasicType.Str)
                        {
                            ty = SpecialType.System_String;
                        }

                        return this.generator.TypeExpression(ty);
                    }
                }

                private string Name
                {
                    get
                    {
                        string suffix = "_n";
                        if (this.type == BasicType.Str)
                        {
                            suffix = "_s";
                        }

                        return this.name + suffix;
                    }
                }

                private SyntaxNode Default
                {
                    get
                    {
                        object lit = 0;
                        if (this.type == BasicType.Str)
                        {
                            lit = string.Empty;
                        }

                        return this.generator.LiteralExpression(lit);
                    }
                }

                public SyntaxNode Ref()
                {
                    return this.generator.IdentifierName(this.Name);
                }

                public SyntaxNode Field()
                {
                    return this.generator.FieldDeclaration(this.Name, this.Type, accessibility: Accessibility.Private);
                }

                public SyntaxNode Init()
                {
                    return this.generator.AssignmentStatement(this.Ref(), this.Default);
                }
            }
        }

        private sealed class Lines
        {
            private readonly SortedList<int, Line> statements;
            private readonly HashSet<int> references;

            public Lines()
            {
                this.statements = new SortedList<int, Line>();
                this.references = new HashSet<int>();
            }

            public void Add(int line, SyntaxNode node)
            {
                this.statements.Add(line, new Line(line, node, null));
            }

            public void AddComment(int line, SyntaxTrivia comment)
            {
                this.statements.Add(line, new Line(line, null, comment));
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
                foreach (Line line in this.statements.Values)
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
