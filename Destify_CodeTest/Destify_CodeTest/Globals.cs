using Serilog;

namespace Destify_CodeTest
{
    /// <summary>
    /// Globally-accessible information about configurations, the environment, etc.
    /// </summary>
    public static class Globals
    {
        public static ConfigSettings Config { get; set; }
        public static string CompilationEnvironment { get; set; }
        public static LoggerConfiguration LogConf { get; set; }
    }
}
