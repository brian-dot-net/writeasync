// <copyright file="ILineVisitor.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWParse.Statements
{
    public interface ILineVisitor
    {
        void Line(int number, BasicStatement[] list);
    }
}