using DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore;

namespace DPK.Mosavabat.EntityFrameworkCore.Base.Seeder;

public class DefaultLanguagesCreator(MosavabatDbContext context)
{
    public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();

    private static List<ApplicationLanguage> GetInitialLanguages()
    {
        var tenantId = (int?)1;
        return
        [
            new ApplicationLanguage(tenantId, "en", "English", "famfamfam-flags us"),
            new ApplicationLanguage(tenantId, "fa", "فارسی", "famfamfam-flag-iran")

        ];
    }

    public void Create()
    {
        CreateLanguages();
    }

    private void CreateLanguages()
    {
        foreach (var language in InitialLanguages)
        {
            AddLanguageIfNotExists(language);
        }
    }

    private void AddLanguageIfNotExists(ApplicationLanguage language)
    {
        if (context.Languages.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
        {
            return;
        }

        context.Languages.Add(language);

        context.SaveChanges();
    }
}