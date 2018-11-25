namespace GithubX.Shared.Models
{
	public class Category
	{
		public int Id { get; set; } = -1;
		public int OrderId { get; set; }
		public string Color { get; set; } = "#ffffffff";
		public string Title { get; set; } = "new_category";
		public GradientColor Background { get; set; } = new GradientColor("#5B247A", "#1BCEDF");
		public string Icon { get; set; } = "👽";
	}
}
