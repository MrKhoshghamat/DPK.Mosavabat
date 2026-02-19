namespace DPK.Mosavabat.DomainEntities.Base;

/// <summary>
/// This class is used to find root path of the web project in;
/// unit tests (to find views) and entity framework core command line commands (to find conn string).
/// </summary>
public static class WebContentDirectoryFinder
{
    public static string CalculateContentRootFolder()
    {
        var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(DpkDomainLayerModule).GetAssembly().Location);
        if (coreAssemblyDirectoryPath != null)
        {
            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "DPK.Mosavabat.sln"))
            {
                directoryInfo = directoryInfo.Parent ?? throw new Exception("Could not find content root folder!");
            }

            var webMvcFolder = Path.Combine(directoryInfo.FullName,
                $"Presentation{Path.DirectorySeparatorChar}DPK.Mosavabat.Api");
            if (Directory.Exists(webMvcFolder))
            {
                return webMvcFolder;
            }

            var webHostFolder = Path.Combine(directoryInfo.FullName,
                $"src{Path.DirectorySeparatorChar}DPK.Mosavabat.Api.Host");
            if (Directory.Exists(webHostFolder))
            {
                return webHostFolder;
            }

            throw new Exception("Could not find root folder of the web project!");
        }

        throw new Exception("Could not find location of DPK.Mosavabat.Core assembly!");
    }

    private static bool DirectoryContains(string directory, string fileName)
    {
        return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
    }
}