﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fixie.Behaviors;
using Fixie.Conventions;
using Should;

namespace Fixie.Tests.Behaviors
{
    public class ExecuteCasesTests
    {
        readonly List<string> log;
        readonly Type testClass;
        readonly Convention convention;

        public ExecuteCasesTests()
        {
            log = new List<string>();
            testClass = typeof(SampleTestClass);

            convention = new SelfTestConvention();
            convention.CaseExecution.Wrap((caseExecution, instance, innerBehavior) =>
            {
                log.Add(caseExecution.Case.Method.Name);
                innerBehavior();
            });
        }

        public void ShouldPerformCaseExecutionBehaviorForAllGivenCases()
        {
            var caseA = new Case(testClass.GetInstanceMethod("Pass"));
            var caseB = new Case(testClass.GetInstanceMethod("Fail"));

            var caseExecutions = new[]
            {
                new CaseExecution(caseA),
                new CaseExecution(caseB)
            };

            var executeCases = new ExecuteCases();
            var executionPlan = new ExecutionPlan(convention);
            var instanceExecution = new InstanceExecution(executionPlan, testClass, new SampleTestClass(), caseExecutions);
            executeCases.Execute(instanceExecution);

            caseExecutions[0].Exceptions.Any().ShouldBeFalse();
            caseExecutions[1].Exceptions.Single().Message.ShouldEqual("'Fail' failed!");
            log.ShouldEqual("Pass", "Fail");
        }

        private class SampleTestClass
        {
            public void Pass() { }
            
            public void Fail()
            {
                throw new FailureException();
            }

            public void Ignored()
            {
                throw new ShouldBeUnreachableException();
            }
        }
    }
}