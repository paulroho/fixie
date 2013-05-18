﻿using System;
using Fixie.Conventions;

namespace Fixie.Tests.ClassFixtures
{
    public class CaseTests
    {
        public void ShouldPassUponSuccessfulExecution()
        {
            var listener = new StubListener();

            new SelfTestConvention().Execute(listener , typeof(PassFixture));

            listener.ShouldHaveEntries(
                "Fixie.Tests.ClassFixtures.CaseTests+PassFixture.Pass passed.");
        }

        public void ShouldFailWithOriginalExceptionWhenCaseMethodThrows()
        {
            var listener = new StubListener();

            new SelfTestConvention().Execute(listener, typeof(FailFixture));

            listener.ShouldHaveEntries(
                "Fixie.Tests.ClassFixtures.CaseTests+FailFixture.Fail failed: Exception of type " +
                "'Fixie.Tests.ClassFixtures.CaseTests+MethodInvokedException' was thrown.");
        }

        public void ShouldPassOrFailCasesIndividually()
        {
            var listener = new StubListener();

            new SelfTestConvention().Execute(listener, typeof(PassFailFixture));

            listener.ShouldHaveEntries(
                "Fixie.Tests.ClassFixtures.CaseTests+PassFailFixture.FailingCaseA failed: Failing Case A",
                "Fixie.Tests.ClassFixtures.CaseTests+PassFailFixture.PassingCaseA passed.",
                "Fixie.Tests.ClassFixtures.CaseTests+PassFailFixture.FailingCaseB failed: Failing Case B",
                "Fixie.Tests.ClassFixtures.CaseTests+PassFailFixture.PassingCaseB passed.",
                "Fixie.Tests.ClassFixtures.CaseTests+PassFailFixture.PassingCaseC passed.");
        }

        class PassFixture
        {
            public void Pass() { }
        }

        class FailFixture
        {
            public void Fail()
            {
                throw new MethodInvokedException();
            }
        }

        class PassFailFixture
        {
            public void FailingCaseA() { throw new Exception("Failing Case A"); }

            public void PassingCaseA() { }

            public void FailingCaseB() { throw new Exception("Failing Case B"); }

            public void PassingCaseB() { }

            public void PassingCaseC() { }
        }

        class MethodInvokedException : Exception { }
    }
}