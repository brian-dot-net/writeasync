// <copyright file="VerbRoutines.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    internal sealed class VerbRoutines
    {
        private readonly Dictionary<string, Func<VerbResult>> verbRoutines;
        private readonly Func<VerbResult> unknown;

        public VerbRoutines(Func<VerbResult> unknown)
        {
            this.verbRoutines = new Dictionary<string, Func<VerbResult>>();
            this.unknown = unknown;
        }

        public void Add(string verb, Func<VerbResult> handler)
        {
            this.verbRoutines.Add(verb, handler);
        }

        public VerbResult Handle(string verb)
        {
            Func<VerbResult> verbRoutine;
            if (!this.verbRoutines.TryGetValue(verb, out verbRoutine))
            {
                verbRoutine = this.unknown;
            }

            return verbRoutine();
        }
    }
}