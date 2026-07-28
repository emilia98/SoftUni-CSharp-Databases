namespace NetPay.Data
{
    public class Configuration
    {
        public static string ConnectionString
            => @"Server=(localdb)\MSSQLLocalDB;Database=NetPay;Trusted_Connection=True;Encrypt=False;";
    }
}