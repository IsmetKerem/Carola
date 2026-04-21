using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carola.DataAccessLayer.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.CommentText)
                .HasMaxLength(1000);

            builder.HasOne(c => c.Car)
                .WithMany(car => car.Comments)
                .HasForeignKey(c => c.CarId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}