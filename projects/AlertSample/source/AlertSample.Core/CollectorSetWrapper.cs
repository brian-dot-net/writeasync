//-----------------------------------------------------------------------
// <copyright file="CollectorSetWrapper.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using PlaLibrary;

    internal sealed class CollectorSetWrapper : ICollectorSet
    {
        private readonly DataCollectorSet dcs;

        public CollectorSetWrapper(DataCollectorSet dcs)
        {
            this.dcs = dcs;
        }

        public void Start()
        {
            this.dcs.start(true);
        }

        public void Stop()
        {
            this.dcs.Stop(true);
        }

        public void Delete()
        {
            this.dcs.Delete();
        }
    }
}
