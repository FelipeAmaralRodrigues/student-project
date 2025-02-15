using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Datas;

namespace StudentProject.Infra.CrossCutting.Bus.Sagas.Mapping
{
    public class StudentCreatedThirdPartyRegistrationSagaDataMapping
        : IEntityTypeConfiguration<StudentCreatedThirdPartyRegistrationSagaData>
    {
        public void Configure(EntityTypeBuilder<StudentCreatedThirdPartyRegistrationSagaData> builder)
        {
            builder.HasKey(pl => pl.CorrelationId)
                .HasName("PKstudentcreatedthirdpartyregistrationsagadata")
                .IsClustered();

            builder.Property(pl => pl.CorrelationId)
                .HasColumnName("correlation_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired(true);

            builder.Property(pl => pl.CurrentState)
                .HasColumnName("current_state")
                .HasColumnType("varchar(64)")
                .IsRequired(true);

            builder.Property(pl => pl.StudentUId)
                .HasColumnName("student_uid")
                .HasColumnType("uniqueidentifier")
                .IsRequired(true);

            builder.Property(pl => pl.RequestCreateStudentThirdPartyUIdSendedAt)
                .HasColumnName("request_create_student_third_party_uid_sended_at")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(pl => pl.ResponseCreateStudentThirdPartyUIdWaitedLastAt)
                .HasColumnName("response_create_student_third_party_uid_waited_last_at")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(pl => pl.ResponseCreateStudentThirdPartyUIdReceivedAt)
                .HasColumnName("response_create_student_third_party_uid_received_at")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(pl => pl.StudentThirdPartyUIdUpdatedAt)
                .HasColumnName("student_third_party_uid_updated_at")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.ToTable("tb_student_created_third_party_registration_saga_data", "mst");
        }
    }
}