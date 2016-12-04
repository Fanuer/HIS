using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HIS.Recipes.Services.DB
{
    internal class TemporaryDbContextFactory: IDbContextFactory<RecipeDbContext>
    {
        public RecipeDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<RecipeDbContext>();
            builder.UseSqlServer("Server=tcp:fanuerdbserver.database.windows.net,1433;Database=HIS_RecipeDB;User ID=Fanuer@fanuerdbserver;Password=h4Ky\\jMcrVj|Xj<l+GsSLh\\g?v&ZUe_b;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");
            return new RecipeDbContext(builder.Options);
        }
    }
}
