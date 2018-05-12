// <copyright file="Command.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    internal sealed class Command
    {
        private Command(string verb, string noun)
        {
            this.Verb = verb;
            this.Noun = noun;
        }

        public string Verb { get; private set; }

        public string Noun { get; private set; }

        public static Command Parse(string command)
        {
            int c = 0;

            string verb = string.Empty;
            while (true)
            {
                ++c;
                if (c > command.Length)
                {
                    break;
                }

                char w = command[c - 1];
                if (w == ' ')
                {
                    break;
                }

                verb += w;
            }

            if (verb.Length > 3)
            {
                verb = verb.Substring(0, 3);
            }

            string noun = string.Empty;
            while (true)
            {
                ++c;
                if (c > command.Length)
                {
                    break;
                }

                char w = command[c - 1];
                if (w == ' ')
                {
                    break;
                }

                noun += w;
            }

            if (noun.Length > 3)
            {
                noun = noun.Substring(0, 3);
            }

            return new Command(verb, noun);
        }
    }
}