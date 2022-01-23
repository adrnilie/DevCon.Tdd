using System;

namespace DevCon.Code.Testing.Common.Helpers
{
    public static class TestingHelpers
    {
        public static T Copy<T>(this T original)
        {
            return (T)Copy((object)original);
        }
    }
}