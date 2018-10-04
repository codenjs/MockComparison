namespace SystemUnderTest
{
    public interface IService
    {
        string GetData(int userId, int qty);
    }

    public interface IRepository
    {
        void Save(string data);
    }
}
