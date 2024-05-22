namespace Api.Application.Startup
{
    public static class EnvManager
    {
        public static void LoadConfig()
        {
            DotNetEnv.Env.Load();
        }
    }
}
