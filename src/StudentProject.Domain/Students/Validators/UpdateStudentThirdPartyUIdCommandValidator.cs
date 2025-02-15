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

            RuleFor(a => a.ThirdPartyUId)
                .Must(a => a != Guid.Empty).WithMessage("ThirdPartyUId cannot be an empty GUID");

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

            When(a => a.ThirdPartyUId != Guid.Empty, () =>
            {
                RuleFor(a => a)
                    .Must(a =>
                    {
                        var student = studentReadOnlyRepository.GetByThirdPartyUId(a.ThirdPartyUId);
                        if ((student == null) || student.UId == a.StudentUId) return true;
                        return false;
                    }).WithMessage("ThirdPartyUId already exists for another user");
            });
        }
    }
}
