﻿using System;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public class ResurrectingObject
    {
        private bool _hasFinalized;

        ~ResurrectingObject()
        {
            if (_hasFinalized)
            {
                return;
            }

            _hasFinalized = true;
            GC.ReRegisterForFinalize(this);
        }
    }
}