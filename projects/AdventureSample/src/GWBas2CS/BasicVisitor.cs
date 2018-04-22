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
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Editing;

    internal sealed class BasicVisitor : ILineVisitor, IStatementVisitor
    {
        private readonly string name;
        private readonly SyntaxGenerator generator;
        private readonly Methods methods;
        private readonly Lines lines;
        private readonly Variables vars;

        private int lineNumber;

        public BasicVisitor(string name)
        {
            this.name = name;
            this.generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
            this.lines = new Lines();
            this.methods = new Methods();
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
            ExpressionNode expr = new ExpressionNode(this.generator, this.vars, this.methods);
            left.Accept(expr);
            SyntaxNode lval = expr.Value;
            right.Accept(expr);
            SyntaxNode rval = expr.Value;
            this.lines.Add(this.lineNumber, this.generator.AssignmentStatement(lval, rval));
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
                case "Dim":
                    this.AddDim(list[0]);
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
            switch (name)
            {
                case "Cls":
                    this.AddCls();
                    break;
                default:
                    throw new NotImplementedException("Void:" + name);
            }
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
            this.methods.Declare(classMembers);
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
            ExpressionNode node = new ExpressionNode(this.generator, this.vars, this.methods);
            expr.Accept(node);
            string name = "PRINT";
            var callPrint = this.generator.InvocationExpression(SyntaxFactory.IdentifierName(name), node.Value);
            this.lines.Add(this.lineNumber, callPrint);
            var output = this.generator.MemberAccessExpression(this.generator.ThisExpression(), this.generator.IdentifierName("output"));
            var callWriteLine = this.generator.MemberAccessExpression(output, "WriteLine");
            SyntaxNode[] printStatements = new SyntaxNode[] { this.generator.InvocationExpression(callWriteLine, this.generator.IdentifierName("expression")) };
            SyntaxNode[] parameters = new SyntaxNode[] { this.generator.ParameterDeclaration("expression", type: this.generator.TypeExpression(SpecialType.System_String)) };
            var printMethod = this.generator.MethodDeclaration(name, accessibility: Accessibility.Private, parameters: parameters, statements: printStatements);
            this.methods.Add(name, printMethod);
        }

        private void AddCls()
        {
            string name = "CLS";
            var callCls = this.generator.InvocationExpression(SyntaxFactory.IdentifierName(name));
            this.lines.Add(this.lineNumber, callCls);
            var output = this.generator.MemberAccessExpression(this.generator.ThisExpression(), this.generator.IdentifierName("output"));
            var callWrite = this.generator.MemberAccessExpression(output, "Write");
            var callClear = this.generator.MemberAccessExpression(this.generator.IdentifierName("Console"), "Clear");
            SyntaxNode[] clsStatements = new SyntaxNode[]
            {
                this.generator.InvocationExpression(callWrite, this.generator.LiteralExpression('\f')),
                this.generator.InvocationExpression(callClear),
            };
            var clsMethod = this.generator.MethodDeclaration(name, accessibility: Accessibility.Private, statements: clsStatements);
            this.methods.Add(name, clsMethod);
        }

        private void AddDim(BasicExpression expr)
        {
            ExpressionNode node = new ExpressionNode(this.generator, this.vars, this.methods);
            expr.Accept(node);
            this.lines.Add(this.lineNumber, node.Value);
        }

        private sealed class Methods
        {
            private readonly Dictionary<string, SyntaxNode> methods;

            public Methods()
            {
                this.methods = new Dictionary<string, SyntaxNode>();
            }

            public void Add(string name, SyntaxNode method)
            {
                if (!this.methods.ContainsKey(name))
                {
                    this.methods.Add(name, method);
                }
            }

            public void Declare(IList<SyntaxNode> classMembers)
            {
                foreach (SyntaxNode method in this.methods.Values)
                {
                    classMembers.Add(method);
                }
            }
        }

        private sealed class ExpressionNode : IExpressionVisitor
        {
            private readonly SyntaxGenerator generator;
            private readonly Variables vars;
            private readonly Methods methods;

            public ExpressionNode(SyntaxGenerator generator, Variables vars, Methods methods)
            {
                this.generator = generator;
                this.vars = vars;
                this.methods = methods;
            }

            public BasicType Type { get; private set; }

            public SyntaxNode Value { get; private set; }

            public void Array(BasicType type, string name, BasicExpression[] subs)
            {
                this.Type = type;
                this.Value = this.vars.Dim(this.methods, type, name, subs);
            }

            public void Literal(BasicType type, object o)
            {
                this.Value = this.generator.LiteralExpression(o);
            }

            public void Operator(string name, BasicExpression[] operands)
            {
                operands[0].Accept(this);
                SyntaxNode x = this.Value;
                if (operands.Length == 2)
                {
                    operands[1].Accept(this);
                    SyntaxNode y = this.Value;
                    this.Value = this.Binary(name, x, y);
                }
                else
                {
                    throw new NotSupportedException("Operator:" + name);
                }
            }

            public void Variable(BasicType type, string name)
            {
                this.Value = this.vars.Add(type, name);
            }

            private SyntaxNode Cast(SyntaxNode node)
            {
                return this.generator.CastExpression(this.generator.TypeExpression(SpecialType.System_Int32), node);
            }

            private SyntaxNode Binary(string name, SyntaxNode x, SyntaxNode y)
            {
                switch (name)
                {
                    case "Or": return this.generator.BitwiseOrExpression(this.Cast(x), this.Cast(y));
                    case "And": return this.generator.BitwiseAndExpression(this.Cast(x), this.Cast(y));
                    case "Add": return this.generator.AddExpression(x, y);
                    case "Sub": return this.generator.SubtractExpression(x, y);
                    case "Mult": return this.generator.MultiplyExpression(x, y);
                    case "Div": return this.generator.DivideExpression(x, y);
                    default: throw new NotSupportedException("Operator:" + name);
                }
            }
        }

        private sealed class Variables
        {
            private readonly SyntaxGenerator generator;
            private readonly Dictionary<string, Variable> strArrs;
            private readonly Dictionary<string, Variable> numArrs;
            private readonly Dictionary<string, Variable> strs;
            private readonly Dictionary<string, Variable> nums;

            public Variables(SyntaxGenerator generator)
            {
                this.generator = generator;
                this.strArrs = new Dictionary<string, Variable>();
                this.numArrs = new Dictionary<string, Variable>();
                this.strs = new Dictionary<string, Variable>();
                this.nums = new Dictionary<string, Variable>();
            }

            private IEnumerable<Variable> Scalars => this.strs.Values.Concat(this.nums.Values);

            private IEnumerable<Variable> Arrays => this.strArrs.Values.Concat(this.numArrs.Values);

            public IEnumerable<SyntaxNode> Fields()
            {
                foreach (Variable v in this.Arrays.Concat(this.Scalars))
                {
                    yield return v.Field();
                }
            }

            public SyntaxNode Init()
            {
                List<SyntaxNode> statements = new List<SyntaxNode>();
                foreach (Variable v in this.Scalars)
                {
                    statements.Add(v.Init());
                }

                return this.generator.MethodDeclaration("Init", accessibility: Accessibility.Private, statements: statements);
            }

            public SyntaxNode Dim(Methods methods, BasicType type, string name, BasicExpression[] subs)
            {
                ExpressionNode node = new ExpressionNode(this.generator, this, methods);
                SyntaxNode[] subNodes = new SyntaxNode[subs.Length];
                for (int i = 0; i < subs.Length; ++i)
                {
                    subs[i].Accept(node);
                    subNodes[i] = node.Value;
                }

                return this.Add(type, name, subNodes.Length).Dim(methods, subNodes);
            }

            public SyntaxNode Add(BasicType type, string name)
            {
                return this.Add(type, name, 0).Ref();
            }

            private Variable Add(BasicType type, string name, int subs)
            {
                IDictionary<string, Variable> dict;
                if (type == BasicType.Str)
                {
                    dict = (subs > 0) ? this.strArrs : this.strs;
                }
                else
                {
                    dict = (subs > 0) ? this.numArrs : this.nums;
                }

                return this.Add(dict, type, name, subs);
            }

            private Variable Add(IDictionary<string, Variable> vars, BasicType type, string name, int subs)
            {
                Variable v;
                if (!vars.TryGetValue(name, out v))
                {
                    v = new Variable(this.generator, type, name, subs);
                    vars.Add(name, v);
                }

                return v;
            }

            private sealed class Variable
            {
                private readonly SyntaxGenerator generator;
                private readonly BasicType type;
                private readonly string name;
                private readonly int subs;

                public Variable(SyntaxGenerator generator, BasicType type, string name, int subs)
                {
                    this.generator = generator;
                    this.type = type;
                    this.name = name;
                    this.subs = subs;
                }

                private TypeSyntax ElementType
                {
                    get
                    {
                        SyntaxKind kind = SyntaxKind.FloatKeyword;
                        if (this.type == BasicType.Str)
                        {
                            kind = SyntaxKind.StringKeyword;
                        }

                        return SyntaxFactory.PredefinedType(SyntaxFactory.Token(kind));
                    }
                }

                private SyntaxNode Type
                {
                    get
                    {
                        if (this.subs > 0)
                        {
                            return this.ArrayType();
                        }

                        return this.ElementType;
                    }
                }

                private string Name => this.name + this.Suffix;

                private string Suffix
                {
                    get
                    {
                        string suffix = "_n";
                        if (this.type == BasicType.Str)
                        {
                            suffix = "_s";
                        }

                        if (this.subs > 0)
                        {
                            suffix += "a";
                        }

                        return suffix;
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

                public SyntaxNode Dim(Methods methods, SyntaxNode[] sub)
                {
                    string name = "DIM" + sub.Length + this.Suffix;
                    var arr = this.generator.IdentifierName("a");
                    SyntaxNode[] subNodes = new SyntaxNode[sub.Length];

                    List<SyntaxNode> parameters = new List<SyntaxNode>();
                    parameters.Add(this.generator.ParameterDeclaration("a", type: this.ArrayType(), refKind: RefKind.Out));
                    for (int i = 0; i < sub.Length; ++i)
                    {
                        string n = "d" + (i + 1);
                        var p = this.generator.ParameterDeclaration(n, type: this.generator.TypeExpression(SpecialType.System_Single));
                        parameters.Add(p);

                        var dn = this.generator.IdentifierName(n);
                        var leftS = this.generator.CastExpression(this.generator.TypeExpression(SpecialType.System_Int32), dn);

                        subNodes[i] = this.generator.AddExpression(leftS, this.generator.LiteralExpression(1));
                    }

                    var arrR = SyntaxFactory.ArrayCreationExpression(this.ArrayType(subNodes));

                    List<SyntaxNode> dimStatements = new List<SyntaxNode>();
                    dimStatements.Add(this.generator.AssignmentStatement(arr, arrR));
                    if (this.type == BasicType.Str)
                    {
                        var fill = this.generator.MemberAccessExpression(this.generator.IdentifierName("Array"), "Fill");
                        var callFill = this.generator.InvocationExpression(fill, arr, this.Default);
                        dimStatements.Add(callFill);
                    }

                    var dimMethod = this.generator.MethodDeclaration(name, accessibility: Accessibility.Private, parameters: parameters, statements: dimStatements);
                    methods.Add(name, dimMethod);

                    var method = this.generator.IdentifierName(name);
                    List<SyntaxNode> args = new List<SyntaxNode>();
                    args.Add(this.generator.Argument(RefKind.Out, this.Ref()));
                    args.AddRange(sub);
                    return this.generator.InvocationExpression(method, args);
                }

                private ArrayTypeSyntax ArrayType(params SyntaxNode[] nodes)
                {
                    List<SyntaxNodeOrToken> sizes = new List<SyntaxNodeOrToken>();
                    var omit = SyntaxFactory.OmittedArraySizeExpression();
                    var comma = SyntaxFactory.Token(SyntaxKind.CommaToken);

                    for (int i = 0; i < this.subs; ++i)
                    {
                        if (i != 0)
                        {
                            sizes.Add(comma);
                        }

                        if (nodes.Length == 0)
                        {
                            sizes.Add(omit);
                        }
                        else
                        {
                            sizes.Add(nodes[i]);
                        }
                    }

                    var rank = SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SeparatedList<ExpressionSyntax>(sizes));

                    return SyntaxFactory.ArrayType(this.ElementType, SyntaxFactory.SingletonList(rank));
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

            public void Add(int number, SyntaxNode node)
            {
                this.Get(number).Add(node);
            }

            public void AddComment(int number, SyntaxTrivia comment)
            {
                this.Get(number).Add(comment);
            }

            public void AddGoto(int number, int destination)
            {
                var label = SyntaxFactory.IdentifierName(Label(destination));
                var gotoStatement = SyntaxFactory.GotoStatement(SyntaxKind.GotoStatement, label);
                this.Add(number, gotoStatement);
                this.references.Add(destination);
            }

            public IEnumerable<SyntaxNode> Statements()
            {
                foreach (Line line in this.statements.Values)
                {
                    foreach (SyntaxNode node in line.Nodes(this.references))
                    {
                        yield return node;
                    }
                }
            }

            private static string Label(int number)
            {
                return "L" + number;
            }

            private Line Get(int number)
            {
                Line line;
                if (!this.statements.TryGetValue(number, out line))
                {
                    line = new Line(number);
                    this.statements.Add(number, line);
                }

                return line;
            }

            private sealed class Line
            {
                private readonly int number;
                private readonly List<SyntaxNode> nodes;

                public Line(int number)
                {
                    this.number = number;
                    this.nodes = new List<SyntaxNode>();
                }

                public void Add(SyntaxNode node) => this.nodes.Add(node);

                public void Add(SyntaxTrivia comment)
                {
                    if (this.nodes.Count == 0)
                    {
                        this.Add(SyntaxFactory.EmptyStatement().WithTrailingTrivia(comment));
                    }
                    else
                    {
                        int n = this.nodes.Count - 1;
                        SyntaxNode last = this.nodes[n];
                        this.nodes[n] = last.WithTrailingTrivia(comment);
                    }
                }

                public IEnumerable<SyntaxNode> Nodes(ISet<int> references)
                {
                    if (references.Contains(this.number))
                    {
                        yield return SyntaxFactory.LabeledStatement(Label(this.number), SyntaxFactory.EmptyStatement());
                    }

                    foreach (SyntaxNode node in this.nodes)
                    {
                        yield return node;
                    }
                }
            }
        }
    }
}
