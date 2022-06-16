using System.Xml.Serialization;

[XmlRoot(ElementName = "repositories")]
public class WeaveRepositories
{
    [XmlElement(ElementName = "repository")]
    public WeaveRepository Repository { get; set; }
}
