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
      //private readonly ITestSettings _settings;
      private readonly IBlobStorage _storage;
      private readonly IBlobStorage _storage2;

      public SftpTest()
      {
         //_settings = Settings.Instance;
         
         //_storage = StorageFactory.Blobs.Sftp(new Renci.SshNet.ConnectionInfo(_settings.SftpHostName, _settings.SftpPort, new Renci.SshNet.PasswordAuthenticationMethod(_settings.SftpUsername, _settings.SftpPassword)));
         _storage = StorageFactory.Blobs.Sftp(new Renci.SshNet.ConnectionInfo("localhost", 22, "foo", new Renci.SshNet.PasswordAuthenticationMethod("foo", "pass")));

         _storage2 = StorageFactory.Blobs.FtpFromFluentFtpClient(new FluentFTP.FtpClient("localhost", 21, new System.Net.NetworkCredential("one", "1234")));
      }

      [Fact]
      public async Task Read()
      {
         ListOptions lo = new ListOptions();

         lo.FolderPath = "ftp/one/";

         IReadOnlyCollection<Blob> result = await _storage2.ListAsync(lo);

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
         IEnumerable<Blob> result = await _storage.ListAsync();

         Assert.NotEmpty(result);
      }
   }
}
