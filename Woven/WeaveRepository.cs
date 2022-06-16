using System.Xml.Serialization;

[XmlRoot(ElementName = "repository")]
public class WeaveRepository
{
    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "url")]
    public string Url { get; set; }

    [XmlElement(ElementName = "branch")]
    public string Branch { get; set; }
}
