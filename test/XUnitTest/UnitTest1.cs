using AsIKnow.Mail;
using AsIKnow.XUnitExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTest
{
    [TestCaseOrderer(Constants.PriorityOrdererTypeName, Constants.PriorityOrdererTypeAssemblyName)]
    [Collection("Dependency collection")]
    public class UnitTest1
    {
        protected DependencyFixture Fixture { get; set; }

        public UnitTest1(DependencyFixture fixture)
        {
            Fixture = fixture;
        }

        //[Trait("Category", "General")]
        //[Fact(DisplayName = nameof(Test1))]
        //[TestPriority(0)]
        //public void Test1()
        //{
        //    int value = 10;
        //    string result = new Dictionary<string, object>() { { "value", value } }.AsViewData().RenderInView("Test").Result;

        //    Assert.Equal($"<h1>Test - {value}</h1>", result);
        //}
    }
}
