using System.Data.Common;

namespace DbIndependent
{
    public static class DbCommandExtension
    {
        public static void AddParamterWithValues(this DbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
