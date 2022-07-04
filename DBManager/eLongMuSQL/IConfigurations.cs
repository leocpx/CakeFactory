namespace eLongMuSQL
{
	public interface IConfigurations
	{
		bool EnableDebugInfo { get; set; }
		string SQLAddress { get; set; }
		string SQLDataBase { get; set; }
		string SQLPass { get; set; }
		string SQLUser { get; set; }
		string CSV_SEPARATOR { get; set; }
	}
}