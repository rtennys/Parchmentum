using Newtonsoft.Json;
using Parchmentum.Domain;
using Parchmentum.XmlConverter;

try
{
    var path = args[0];

    var pagesPath = Path.Combine(path, "pages");
    foreach (var filename in Directory.GetFiles(pagesPath, "*.xml"))
    {
        var xml = File.ReadAllText(filename);
        var json = XmlConverter.ToJson(xml);
        Console.WriteLine(json);
        try
        {
            var page = JsonConvert.DeserializeObject<Page>(json) ?? throw new Exception($"Error deserializing {filename}");
            page.Id = Guid.Parse(Path.GetFileNameWithoutExtension(filename));
            page.Content = page.Content.Trim();
            File.WriteAllText(Path.Combine(pagesPath, $"{page.DateCreated:yyyy-MM-dd}-{page.Slug}.json"), JsonConvert.SerializeObject(page, Formatting.Indented));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    var postsPath = Path.Combine(path, "posts");
    foreach (var filename in Directory.GetFiles(postsPath, "*.xml"))
    {
        var xml = File.ReadAllText(filename);
        var json = XmlConverter.ToJson(xml);
        Console.WriteLine(json);
        try
        {
            var post = JsonConvert.DeserializeObject<Post>(json) ?? throw new Exception($"Error deserializing {filename}");
            post.Id = Guid.Parse(Path.GetFileNameWithoutExtension(filename));
            post.Content = post.Content.Trim();
            File.WriteAllText(Path.Combine(postsPath, $"{post.PubDate:yyyy-MM-dd}-{post.Slug}.json"), JsonConvert.SerializeObject(post, Formatting.Indented));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        Console.WriteLine();
        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
