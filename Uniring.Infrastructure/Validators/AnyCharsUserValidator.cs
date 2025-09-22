using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniring.Infrastructure.Validators
{

    /// <summary>
    /// Validator that accepts most Unicode characters and spaces while:
    /// - normalizing to FormC,
    /// - rejecting empty/whitespace-only,
    /// - rejecting control characters,
    /// - enforcing min/max length,
    /// </summary>
    public class AnyCharsUserValidator<TUser> : IUserValidator<TUser> where TUser : class
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public AnyCharsUserValidator(int minLength = 1, int maxLength = 256)
        {
            if (minLength < 0) throw new ArgumentOutOfRangeException(nameof(minLength));
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (minLength > maxLength) throw new ArgumentException("minLength > maxLength");
            _minLength = minLength;
            _maxLength = maxLength;
        }
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager is null) throw new ArgumentNullException(nameof(manager));
            if (user is null) throw new ArgumentNullException(nameof(user));

            var errors = new List<IdentityError>();
            var userNameProperty = typeof(TUser).GetProperty("UserName");
            var username = userNameProperty?.GetValue(user) as string;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username)
                || username.Length < _minLength)
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameIsEmptyOrTooShort",
                    Description = "User name must not be empty."
                });
            }

            // enforce min/max length
            else if (username.Length > _maxLength)
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameTooLong",
                    Description = "User name is too long."
                });
            }

            // Trim leading/trailing whitespace by default
            username = username.Trim();

            // Unicode normalization (use FormC)
            var normalized = username.Normalize(NormalizationForm.FormC);

            // Reject control characters and unprintable separators
            if (normalized.Any(ch => char.IsControl(ch) || char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.LineSeparator || char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.ParagraphSeparator))
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameContainsInvalidCharacters",
                    Description = "User name contains disallowed control or separator characters."
                });
            }

            if (errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
