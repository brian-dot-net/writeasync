// <copyright file="Stmt.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    internal static class Stmt
    {
        public static BasicStatement FromString(string input)
        {
            return new RemarkStatement();
        }
    }
}