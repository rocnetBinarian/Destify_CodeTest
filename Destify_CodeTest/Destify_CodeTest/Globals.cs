using Serilog;

namespace Destify_CodeTest
{
    public static class Globals
    {
        public static ConfigSettings Config { get; set; }
        public static string CompilationEnvironment { get; set; }
        public static LoggerConfiguration LogConf { get; set; }
    }
}
