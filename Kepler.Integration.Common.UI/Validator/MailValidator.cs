using AE.Net.Mail;
using FluentAssertions;
using NUnit.Framework;

namespace Kepler.Integration.Common.UI.Validator
{
    internal class MailValidator
    {
        private MailMessage actualMail;

        public MailValidator(MailMessage actualMail)
        {
            this.actualMail = actualMail;
        }

        public void ValidateSubject(string expectedSubject)
        {
            Assert.AreEqual(expectedSubject.Trim().ToLower(), actualMail.Subject.Trim().ToLower(), "Названия темы письма некорректно");
        }

        public void ValidateTextContainsInSubject(string expectedText)
        {
            actualMail.Subject.Trim().ToLower().Should().Contain(expectedText.ToLower());
        }

        public void ValidateTextContainsInBody(string expectedText)
        {
            actualMail.Body.ToLower().Should().Contain(expectedText.ToLower());
        }
    }
}