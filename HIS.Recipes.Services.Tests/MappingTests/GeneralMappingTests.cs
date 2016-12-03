using AutoMapper;
using HIS.Recipes.Services.Configs;
using Xunit;

namespace HIS.Recipes.Services.Tests.MappingTests
{
    public class GeneralMappingTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
