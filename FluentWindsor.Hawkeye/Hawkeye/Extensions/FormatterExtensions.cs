using System.Text;

namespace FluentlyWindsor.Hawkeye.Extensions
{
	public static class FormatterExtensions
	{
		public static void WritePair(this StringBuilder builder, string value1, object value2)
		{
			builder.AppendLine(value1);
			builder.Append("\t");
			builder.AppendLine(value2 == null ? "null" : value2.ToString());
			builder.AppendLine();
		}
	}
}