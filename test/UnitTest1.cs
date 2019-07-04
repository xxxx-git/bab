using Xunit;
using bab.Controllers;

namespace bab_test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1,1);
        }

        [Fact]
        public void GetValuesFromValuesController_ShouldReturnValues()
        {
            var controller = new ValuesController();
            
            var values = controller.Get();
            Assert.Equal(values.Value, new string[] { "value11", "value2"});
        }
    }
}
