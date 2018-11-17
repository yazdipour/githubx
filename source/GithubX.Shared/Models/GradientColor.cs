namespace GithubX.Shared.Models
{
	public class GradientColor
	{
		public GradientColor()
		{

		}
		public GradientColor(string Color1, string Color2)
		{
			this.Color1 = Color1;
			this.Color2 = Color2;
		}

		public string Color1 { get; set; } = "#111111";
		public string Color2 { get; set; } = "#111111";
	}
}
