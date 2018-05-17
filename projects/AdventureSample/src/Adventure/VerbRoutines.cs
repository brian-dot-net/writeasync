// <copyright file="VerbRoutines.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    internal sealed class VerbRoutines
    {
        private readonly Dictionary<string, Func<string, VerbResult>> verbRoutines;
        private readonly Func<string, VerbResult> unknown;

        public VerbRoutines(Func<VerbResult> unknown)
        {
            this.verbRoutines = new Dictionary<string, Func<string, VerbResult>>();
            this.unknown = _ => unknown();
        }

        public void Add(string verb, Func<string, VerbResult> handler)
        {
            this.verbRoutines.Add(verb, handler);
        }

        public void Add(string verb, Func<VerbResult> handler)
        {
            this.Add(verb, s => handler());
        }

        public void Add<T>(string verb, Func<string, T> noun, Func<T, VerbResult> handler)
        {
            this.Add(verb, s => handler(noun(s)));
        }

        public VerbResult Handle(string verb, string noun)
        {
            Func<string, VerbResult> verbRoutine;
            if (!this.verbRoutines.TryGetValue(verb, out verbRoutine))
            {
                verbRoutine = this.unknown;
            }

            return verbRoutine(noun);
        }
    }
}