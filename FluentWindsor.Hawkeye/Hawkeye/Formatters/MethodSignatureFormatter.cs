using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentlyWindsor.Hawkeye.Extensions;

namespace FluentlyWindsor.Hawkeye.Formatters
{
    public class MethodSignatureFormatter
    {
        public string GetSignature(MethodBase method, params object[] args)
        {
            return string.Format("{0}({1})", method.Name, string.Join(",", GetParams(method, args)));
        }

        private static IEnumerable<string> GetParams(MethodBase method, params object[] args)
        {
            var parameters = new List<string>();
            var parameterInfos = method.GetParameters();
            foreach (var parameter in parameterInfos)
            {
                ParameterInfo localParameter = parameter;
                var parameterIndex = parameterInfos.ToList().IndexOf(pi => pi.Name == localParameter.Name);
                var currentParameter = localParameter.ParameterType.Name + " " + localParameter.Name;

                if (args != null && args.Length != 0)
                {
                    currentParameter += ": ";
                    var localArgument = args[parameterIndex];
                    if (localArgument is IEnumerable && localArgument.GetType() != typeof(string))
                        currentParameter +=
                            string.Join(",",
                                        (localArgument as IEnumerable)
                                            .Cast<object>()
                                            .Select(FormatArgumentString)
                                            .ToArray());
                    else
                        currentParameter +=
                            localArgument == null
                                ? "null"
                                : localArgument.ToString();
                }
                parameters.Add(currentParameter);
            }
            return parameters;
        }

        private static string FormatArgumentString(object arg)
        {
            const int maxArgumentLength = 50;
            if (arg != null)
            {
                var localArg = arg.ToString();
                if (localArg.Length > maxArgumentLength)
                    return localArg.Substring(0, maxArgumentLength);
                return localArg;
            }
            return "null";
        }
    }
}
