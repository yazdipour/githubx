namespace GithubX.Shared.Models
{
	public class Category
	{
		public int Id { get; set; }
		public int OrderId;
		public string Color { get; set; } = "#ffffffff";
		public string Title { get; set; } = "new_category";
		public GradientColor Background { get; set; } = new GradientColor() { Color1 = "#5B247A", Color2 = "#1BCEDF" };
		public string Icon { get; set; } = "👽";
	}
}
