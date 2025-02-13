using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaExampleMassTransit.Domain.Entities;

namespace SagaExampleMassTransit.Infra.Data.Mappings
{
    public class StudentMapping : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(pl => pl.Id)
                .HasName("PKtbstudent")
                .IsClustered();

            builder.Property(pl => pl.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder.Property(pl => pl.UId)
                .HasColumnName("uid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()")
                .IsRequired(true);

            builder.Property(pl => pl.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(128)")
                .IsRequired(true);

            builder.Property(pl => pl.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(128)")
                .IsRequired(true);

            builder.Property(pl => pl.Email)
               .HasColumnName("email")
               .HasColumnType("varchar(128)")
               .IsRequired(true);

            builder.Property(pl => pl.BirthDate)
               .HasColumnName("birth_date")
               .HasColumnType("datetime2")
               .IsRequired(true);

            builder.Property(pl => pl.ThirdPartyStudentUId)
               .HasColumnName("third_party_student_uid")
               .HasColumnType("uniqueidentifier")
               .IsRequired(false);

            builder.ToTable("tb_student", "dbo");
        }
    }
}
