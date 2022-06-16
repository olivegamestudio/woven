using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "project")]
public class WeaveProject
{
    [XmlElement(ElementName = "repository")]
    public List<WeaveRepository> Repositories { get; set; }
}
