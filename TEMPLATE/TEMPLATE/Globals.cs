using Serilog;

namespace TEMPLATE
{
    public static class Globals
    {
        public static ConfigSettings Config { get; set; }
        public static string CompilationEnvironment { get; set; }
        public static LoggerConfiguration LogConf { get; set; }
    }
}
