﻿using System;
using System.Collections.Generic;

namespace Fixie
{
    public class CaseResult
    {
        readonly List<Exception> exceptions;

        public CaseResult(Case @case)
        {
            Case = @case;
            exceptions = new List<Exception>();
        }

        public Case Case { get; private set; }
        
        public TimeSpan Duration { get; set; }

        public string Output { get; set; }

        public IReadOnlyList<Exception> Exceptions { get { return exceptions; } }

        public void Fail(Exception reason)
        {
            var wrapped = reason as PreservedException;

            if (wrapped != null)
                exceptions.Add(wrapped.OriginalException);
            else
                exceptions.Add(reason);
        }
    }
}