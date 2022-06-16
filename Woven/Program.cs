using System.Xml.Linq;
using System.Xml.Serialization;
using CommandLine;
using LibGit2Sharp;
using Microsoft.Alm.Authentication;

Console.WriteLine("Welcome to Woven...");
string weaveFile = "weave.xml";
var destPath = string.Empty;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(o =>
    {
        weaveFile = o.WeaveFile;
        destPath = o.Destination;
    });

Console.WriteLine($"Reading weave file: '{weaveFile}'.");
if (!File.Exists(weaveFile))
{
    Console.WriteLine($"Failed to find weave file: '{weaveFile}'.");
    Environment.Exit(-1);
}

XDocument weaveDocument = XDocument.Load(weaveFile);
WeaveProject project = (WeaveProject)new XmlSerializer(typeof(WeaveProject)).Deserialize(weaveDocument.CreateReader());

foreach (WeaveRepository repositoryConfiguration in project.Repositories)
{
    var secrets = new SecretStore("git");
    var auth = new BasicAuthentication(secrets);

    var target = new TargetUri(new Uri(repositoryConfiguration.Url));
    var root = target.QueryUri.GetLeftPart(UriPartial.Authority);
    var creds = auth.GetCredentials(new Uri(root));

    var folderName = Path.GetFileNameWithoutExtension(target.QueryUri.LocalPath);
    
    var options = new CloneOptions
    {
        CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
        {
            Username = creds.Username,
            Password = creds.Password
        },
        BranchName = repositoryConfiguration.Branch,
    };

    destPath = Path.Combine(destPath, folderName);
    Console.WriteLine($"Downloading '{repositoryConfiguration.Name}' to '{destPath}'...");
    var path = Repository.Clone(repositoryConfiguration.Url, destPath, options);
}
