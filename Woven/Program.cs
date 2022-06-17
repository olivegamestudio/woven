using System.Xml.Linq;
using System.Xml.Serialization;
using CommandLine;
using LibGit2Sharp;
using Microsoft.Alm.Authentication;

Console.WriteLine("Welcome to Woven...");
string weaveFile = "weave.xml";
var destPath = string.Empty;

// get source weave file and destination path from the command line
Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(o =>
    {
        weaveFile = o.WeaveFile;
        destPath = o.Destination;
    });

// and validate destination path
if (string.IsNullOrEmpty(destPath))
{
    Console.WriteLine("Invalid destination path.");
    Environment.Exit(-1);
}

// and source file exists
Console.WriteLine($"Reading weave file: '{weaveFile}'.");
if (!File.Exists(weaveFile))
{
    Console.WriteLine($"Failed to find weave file: '{weaveFile}'.");
    Environment.Exit(-1);
}

// load the source file
XDocument weaveDocument = XDocument.Load(weaveFile);
WeaveProject project = (WeaveProject)new XmlSerializer(typeof(WeaveProject)).Deserialize(weaveDocument.CreateReader());

// and process the repositories
foreach (WeaveRepository repositoryConfiguration in project.Repositories)
{
    var credentials = new CredentialsHelper().GetCredentials(repositoryConfiguration.Url);
    if (credentials == null)
    {
        Console.WriteLine("Failed to get credentials.");
        Environment.Exit(-2);
    }

    var options = new CloneOptions
    {
        CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
        {
            Username = credentials.Username,
            Password = credentials.Password
        },
        BranchName = repositoryConfiguration.Branch,
    };

    var target = new TargetUri(new Uri(repositoryConfiguration.Url));
    var root = target.QueryUri.GetLeftPart(UriPartial.Authority);
    var folderName = Path.GetFileNameWithoutExtension(target.QueryUri.LocalPath);
    string relativeDestPath = Path.Combine(destPath, folderName);

    // clone repository
    Console.WriteLine($"Downloading '{repositoryConfiguration.Name}' to '{relativeDestPath}'...");
    var _ = Repository.Clone(repositoryConfiguration.Url, relativeDestPath, options);
}
