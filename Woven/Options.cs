using CommandLine;

public class Options
{
    [Option('i', "input", Required = false, HelpText = "The input weave file.")]
    public string WeaveFile { get; set; } = "weave.xml";

    [Option('o', "output", Required = true, HelpText = "The destination folder.")]
    public string Destination { get; set; }
}
