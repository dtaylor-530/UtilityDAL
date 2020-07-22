namespace UtilityDAL.Abstract
{
    public interface IFileParser
    {
        string Filter();

        System.Collections.ICollection Parse(string path);

        string Map(string value);
    }
}