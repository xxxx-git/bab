using Xunit;
using Services;
using System.IdentityModel.Tokens.Jwt;
using bab;
using Microsoft.IdentityModel.Tokens;
using Shared.Config;
using System.Collections.Generic;

namespace test
{
    public class JsonObjMock
    {
        public int IntParam;
        public string StrParam;
        public List<string> LstParam;
        public JsonObjMock Obj;
        public JsonObjMock(){}
    }

    public class JsonConvertService_UnitTest
    {
        private readonly SynJsonConvertService _jsonConvertService;
        
        public JsonConvertService_UnitTest() 
        {
            _jsonConvertService = new SynJsonConvertService();
        }

        [Theory]
        [MemberData(nameof(getSerializeTestInputs))]
        public void Serialize_ShouldSerializeObject(object obj, string expected) 
        {
            string actualStr =_jsonConvertService.Serialize(obj);
            Assert.Equal(expected, actualStr);
        }

        public static IEnumerable<object[]> getSerializeTestInputs()
        {
            var allTestInputs =  new List<object[]>
            {
                new object [] 
                {
                    new JsonObjMock()
                    {
                        IntParam = 1,
                        StrParam = "sabag",
                        LstParam = new List<string>{"a","b","c"},
                    },
                    "{\"IntParam\":1,\"StrParam\":\"sabag\",\"LstParam\":[\"a\",\"b\",\"c\"],\"Obj\":null}"
                },
                new object [] 
                {
                    new JsonObjMock()
                    {
                        IntParam = 1,
                        StrParam = "sabag",
                        LstParam = new List<string>{"a","b","c"},
                        Obj = new JsonObjMock() {
                            IntParam = 1,
                            StrParam = "sabag",
                            LstParam = new List<string>{"a","b","c"}
                        }
                    },
                    "{\"IntParam\":1,\"StrParam\":\"sabag\",\"LstParam\":[\"a\",\"b\",\"c\"],\"Obj\":{\"IntParam\":1,\"StrParam\":\"sabag\",\"LstParam\":[\"a\",\"b\",\"c\"],\"Obj\":null}}"
                },
                new object [] 
                {
                    new JsonObjMock()
                    {
                        IntParam = 1,
                        StrParam = "",
                        LstParam = new List<string>{"","",""},
                        Obj = new JsonObjMock()
                    },
                    "{\"IntParam\":1,\"StrParam\":\"\",\"LstParam\":[\"\",\"\",\"\"],\"Obj\":{\"IntParam\":0,\"StrParam\":null,\"LstParam\":null,\"Obj\":null}}"
                },
                new object [] 
                {
                    new JsonObjMock(),
                    "{\"IntParam\":0,\"StrParam\":null,\"LstParam\":null,\"Obj\":null}"
                },
                new object [] {new object(),"{}"},
                new object [] {null,"null"}
            };
            return allTestInputs;
        }
    }
}
