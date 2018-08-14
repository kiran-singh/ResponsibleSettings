using System;
using FluentAssertions;
using NUnit.Framework;

namespace Library.DotNet.Tests
{
    [TestFixture]
    public class SettingsFixture
    {
        [Test]
        public void Settings_Valid_AllPropertiesSet()
        {
            // Arrange
            // Act
            var actual = new ValidSettings();

            // Assert
            actual.AllowRedirect.Should().BeTrue();
            actual.ApiId.Should().Be(new Guid("BE0FACD3-343D-419F-A37D-267588E0A393"));
            actual.AppName.Should().Be("GoodApp");
            actual.ServerId.Should().Be(84);
            actual.StartDate.Should().Be(DateTime.Parse("24-11-2017"));
        }

        [Test]
        public void Settings_Missing_ExceptionThrown()
        {
            // Arrange  
            // Act
            var actual = Assert.Throws<Exception>(() => new MissingSettings());

            // Assert
            actual.Message.Should().Be(string.Format(Settings.FormatErrorMessageMissingEnvironmentVariables,
                string.Join(", ", nameof(MissingSettings.AllowDirect), nameof(MissingSettings.ApiNewId),
                    nameof(MissingSettings.AppNames), nameof(MissingSettings.ServerMap))));
        }

        [Test]
        public void Settings_Invalid_ExceptionThrown()
        {
            // Arrange  
            // Act
            var actual = Assert.Throws<Exception>(() => new InvalidSettings());

            // Assert
            actual.Message.Should().Be(string.Format(Settings.FormatErrorMessageInvalidEnvironmentVariables,
                string.Join(", ", nameof(InvalidSettings.AllowDirection), nameof(InvalidSettings.ApiIdentifier),
                    nameof(InvalidSettings.RedirectDate), nameof(InvalidSettings.ServerMapperId))));
        }

        public class ValidSettings : Settings
        {
            public bool AllowRedirect { get; set; }

            public Guid ApiId { get; set; }

            public string AppName { get; set; }

            public int ServerId { get; set; }

            public DateTime StartDate { get; set; } 
        }

        public class MissingSettings : Settings
        {
            public bool AllowDirect { get; set; }

            public Guid ApiNewId { get; set; }

            public string AppNames { get; set; }

            public int ServerMap { get; set; }
        }

        public class InvalidSettings : Settings
        {
            public bool AllowDirection { get; set; }

            public Guid ApiIdentifier { get; set; }

            public DateTime RedirectDate { get; set; }

            public int ServerMapperId { get; set; }
        }
    }
}