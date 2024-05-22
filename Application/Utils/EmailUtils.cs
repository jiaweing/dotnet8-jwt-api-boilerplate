namespace Api.Application.Utils
{
    public class EmailUtils
    {
        private readonly IWebHostEnvironment _env;

        public EmailUtils(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool IsEmailInTempList(string email)
        {
            var path = Path.Combine(_env.ContentRootPath, "Data/temp-email-list.txt");
            Console.WriteLine(path);
            if (File.Exists(path))
            {
                var emailList = File.ReadAllLines(path);
                email = email.ToLower();
                var emailDomain = email.Split('@')[1];
                return emailList.Contains(emailDomain);
            }

            return false;
        }
    }

}
