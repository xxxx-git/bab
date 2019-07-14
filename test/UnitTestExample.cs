using Xunit;
using bab.Controllers;

namespace test
{
    public class UnitTestExample
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
            Assert.Equal(values.Value, new string[] { "value1", "value2"});
        }
    }
}
