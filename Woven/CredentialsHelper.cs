using Microsoft.Alm.Authentication;

public class CredentialsHelper
{
    public Credential? GetCredentials(string url)
    {
        var secrets = new SecretStore("git");
        var auth = new BasicAuthentication(secrets);

        var target = new TargetUri(new Uri(url));
        var root = target.QueryUri.GetLeftPart(UriPartial.Authority);
        return auth.GetCredentials(new Uri(root));
    }
}
