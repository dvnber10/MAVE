using Microsoft.EntityFrameworkCore;

namespace MAVE.Seed;

public static class SeedModelBuilder
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.CatRole>().HasData(SeedData.Roles);
        modelBuilder.Entity<Models.CatStatus>().HasData(SeedData.Statuses);
        modelBuilder.Entity<Models.CatEvaluation>().HasData(SeedData.Evaluations);
        modelBuilder.Entity<Models.CatScore>().HasData(SeedData.Scores);
        modelBuilder.Entity<Models.CatArticleType>().HasData(SeedData.ArticleTypes);
        modelBuilder.Entity<Models.CatQuestion>().HasData(SeedData.Questions);
        modelBuilder.Entity<Models.CatOption>().HasData(SeedData.Options);
    }
}