using FluentValidation;
using StudentProject.Domain.Students.Commands;
using StudentProject.Domain.Students.Repositories;

namespace StudentProject.Domain.Students.Validators
{
    public class UpdateStudentThirdPartyUIdCommandValidator : AbstractValidator<UpdateStudentThirdPartyUIdCommand>
    {
        public UpdateStudentThirdPartyUIdCommandValidator(IStudentReadOnlyRepository studentReadOnlyRepository)
        {
            RuleFor(a => a.StudentUId)
                .Must(a => a != Guid.Empty).WithMessage("StudentUId cannot be an empty GUID");

            RuleFor(a => a.ThirdPartyStudentUId)
                .Must(a => a != Guid.Empty).WithMessage("ThirdPartyStudentUId cannot be an empty GUID");

            When(a => a.StudentUId != Guid.Empty, () =>
            {
                RuleFor(a => a)
                    .Must(a =>
                    {
                        var student = studentReadOnlyRepository.GetByUId(a.StudentUId);
                        if (student == null) return false;

                        a.SetStudent(student);
                        return true;

                    }).WithMessage("Student does not exist");
            });

            When(a => a.ThirdPartyStudentUId != Guid.Empty, () =>
            {
                RuleFor(a => a)
                    .Must(a =>
                    {
                        var student = studentReadOnlyRepository.GetByThirdPartyStudentUId(a.ThirdPartyStudentUId);
                        if ((student == null) || student.UId == a.StudentUId) return true;
                        return false;
                    }).WithMessage("ThirdPartyStudentUId already exists for another user");
            });
        }
    }
}
