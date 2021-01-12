using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Storage.Net.Blobs;
using Xunit;

namespace Storage.Net.Tests.Integration.SFTP
{
   [Trait("Category", "Blobs")]
   public class SftpTest
   {
      private readonly ITestSettings _settings;
      private readonly IBlobStorage _storage;

      public SftpTest()
      {
         _settings = Settings.Instance;

         _storage = StorageFactory.Blobs.Sftp(new Renci.SshNet.ConnectionInfo(_settings.SftpHostName, _settings.SftpPort, new Renci.SshNet.PasswordAuthenticationMethod(_settings.SftpUsername, _settings.SftpPassword)));

      }

      [Fact]
      public async Task ListFolderAsync()
      {
         ListOptions lo = new ListOptions();

         lo.FolderPath = "/ftp/one";

         IReadOnlyCollection<Blob> result = await _storage.ListAsync(lo);

         Assert.NotNull(result);
      }

      [Fact]
      public async Task WriteAsync()
      {
         await _storage.WriteTextAsync("upload/testing", "test");

         Blob result = await _storage.GetBlobAsync("upload/testing");

         Assert.NotNull(result);
      }

      [Fact]
      public async Task GetBlobNullArgumentException()
      {
         await Assert.ThrowsAsync<ArgumentNullException>(() => _storage.GetBlobAsync(""));
      }

      [Fact]
      public async Task ExistsFalse()
      {
         bool result = await _storage.ExistsAsync("test");

         Assert.False(result);
      }

      [Fact]
      public async Task ListAsync()
      {
         ListOptions lo = new ListOptions();

         IEnumerable<Blob> result = await _storage.ListAsync(lo);

         Assert.NotEmpty(result);
      }

      [Fact]
      public async Task ListRecursiveAsync()
      {
         ListOptions lo = new ListOptions();

         lo.Recurse = true;

         IEnumerable<Blob> result = await _storage.ListAsync(lo);

         Assert.NotEmpty(result);
      }

   }
}
