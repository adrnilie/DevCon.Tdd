using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DevCon.Code.Business.Services;
using DevCon.Code.Common;
using DevCon.Code.Messaging.Handlers;
using DevCon.Code.Testing.Common.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace DevCon.Code.Testing.Common
{
    public class TestableSystem : IDisposable
    {
        protected FakeDevConUserRepository FakeDevConUserRepository { get; private set; } = new FakeDevConUserRepository();
        protected FakeMessagingRepository FakeMessagingRepository { get; private set; } = new FakeMessagingRepository();

        protected DevConUserController DevConUserController => new DevConUserController(DevConUserService, TokenProviderService, CommandsHandler);

        protected IEnumerable<T> GetSentMessages<T>() where T : class
        {
            var sentMessages = new List<T>();

            sentMessages.AddRange(FakeMessagingRepository.SentMessages.Where(x => x.Message is T).Select(x => x.Message as T));

            return sentMessages;
        }

        protected Response GetObjectResult<T>(ActionResult actionResult)
            where T : ObjectResult
        {
            var objectResult = actionResult as T;

            return objectResult?.Value as Response;
        }

        protected Response<TValue> GetObjectResult<T, TValue>(ActionResult actionResult)
            where T : ObjectResult
            where TValue : class
        {
            var objectResult = actionResult as T;

            return objectResult?.Value as Response<TValue>;
        }

        protected static void EnsureBadRequestStatusCode(Response response)
        {
            response.Success.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        protected static void EnsureConflictStatusCode(Response response)
        {
            response.Success.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        }

        protected static void EnsureSuccessStatusCode(Response response)
        {
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private static ISecurePasswordService SecurePasswordService => new SecurePasswordService();
        private static ITokenProviderService TokenProviderService => new TokenProviderService();

        private IDevConUserService DevConUserService => new DevConUserService(FakeDevConUserRepository, SecurePasswordService);
        private ICommandsHandler CommandsHandler => new CommandsHandler(FakeMessagingRepository);
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FakeDevConUserRepository = new FakeDevConUserRepository();
                FakeMessagingRepository = new FakeMessagingRepository();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}