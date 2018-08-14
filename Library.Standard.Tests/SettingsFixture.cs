using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Library.Standard.Tests
{
    [TestFixture]
    public class SettingsFixture
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void Settings_Valid_AllPropertiesSet()
        {
            // Arrange
            var apiId = Guid.NewGuid().ToString();
            var appName = _fixture.Create<string>();
            var serverId = _fixture.Create<int>();
            var startDate = new DateTime(2018, 5, 19);

            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {nameof(ValidSettings.AllowRedirect), "true" },
                {nameof(ValidSettings.ApiId), apiId },
                {nameof(ValidSettings.AppName), appName },
                {nameof(ValidSettings.ServerId), serverId.ToString() },
                {nameof(ValidSettings.StartDate), startDate.ToString("d") },
            });

            var configuration = builder.Build();

            // Act
            var actual = new ValidSettings(configuration);

            // Assert
            actual.AllowRedirect.Should().BeTrue();
            actual.ApiId.Should().Be(apiId);
            actual.AppName.Should().Be(appName);
            actual.ServerId.Should().Be(serverId);
            actual.StartDate.Should().Be(startDate);
        }

        [Test]
        public void Settings_Missing_ExceptionThrown()
        {
            // Arrange  
            var builder = new ConfigurationBuilder();

            // Act
            var actual = Assert.Throws<Exception>(() => new MissingSettings(builder.Build()));

            // Assert
            actual.Message.Should().Be(string.Format(Settings.FormatErrorMessageMissingEnvironmentVariables,
                string.Join(", ", nameof(MissingSettings.AllowDirect), nameof(MissingSettings.ApiNewId),
                    nameof(MissingSettings.AppNames), nameof(MissingSettings.ServerMap))));
        }

        [Test]
        public void Settings_Invalid_ExceptionThrown()
        {
            // Arrange  
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {nameof(InvalidSettings.AllowDirection), "yes" },
                {nameof(InvalidSettings.ApiIdentifier), "NotGuid" },
                {nameof(InvalidSettings.RedirectDate), "24-37-2008" },
                {nameof(InvalidSettings.ServerMapperId), "j8d4" },
            });

            var configuration = builder.Build();

            // Act
            var actual = Assert.Throws<Exception>(() => new InvalidSettings(configuration));

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

            public ValidSettings(IConfiguration configuration) : base(configuration)
            {
            }
        }

        public class MissingSettings : Settings
        {
            public bool AllowDirect { get; set; }

            public Guid ApiNewId { get; set; }

            public string AppNames { get; set; }

            public int ServerMap { get; set; }

            public MissingSettings(IConfiguration configuration) : base(configuration)
            {
            }
        }

        public class InvalidSettings : Settings
        {
            public bool AllowDirection { get; set; }

            public Guid ApiIdentifier { get; set; }

            public DateTime RedirectDate { get; set; }

            public int ServerMapperId { get; set; }

            public InvalidSettings(IConfiguration configuration) : base(configuration)
            {
            }
        }
    }
}