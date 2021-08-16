namespace Core.Exception
{
    public class ApplicationException : System.Exception
    {
        public ApplicationException(string msg) : base(msg)
        {
        }
    }
}