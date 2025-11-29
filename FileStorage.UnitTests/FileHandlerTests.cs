using System.Text;
using FileStorage.Infrastructure.Storage.Handlers;

namespace FileStorage.UnitTests
{
    public class FileHandlerTests : IDisposable
    {
        private readonly string _testStoragePath;
        private readonly TestFileHandler _fileHandler;

        public FileHandlerTests()
        {
            _testStoragePath = Path.Combine(Path.GetTempPath(), $"FileHandlerTests_{Guid.NewGuid()}");
            _fileHandler = new TestFileHandler(_testStoragePath);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testStoragePath))
            {
                try
                {
                    Directory.Delete(_testStoragePath, true);
                }
                catch
                {
                    // ignored
                }
            }
        }

        [Fact]
        public async Task SaveFile_ValidStream_SavesCorrectlyAndReturnsGuid()
        {
            // Arrange
            var content = "Test file content with special chars: äöü 测试";
            using var stream = CreateStreamFromString(content);

            // Act
            var result = await _fileHandler.SaveFile(stream);

            // Assert
            Assert.False(result.IsFailure);
            Assert.NotEqual(Guid.Empty, result.Value);

            var filePath = Path.Combine(_testStoragePath, result.Value.ToString());
            Assert.True(File.Exists(filePath));

            var actualContent = await File.ReadAllTextAsync(filePath);
            Assert.Equal(content, actualContent);
        }

        [Fact]
        public async Task SaveFile_NullStream_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _fileHandler.SaveFile(null!));
        }

        [Fact]
        public async Task SaveFile_MultipleFiles_CreatesUniqueGuids()
        {
            // Arrange
            using var stream1 = CreateStreamFromString("content1");
            using var stream2 = CreateStreamFromString("content2");

            // Act
            var result1 = await _fileHandler.SaveFile(stream1);
            var result2 = await _fileHandler.SaveFile(stream2);

            // Assert
            Assert.NotEqual(result1.Value, result2.Value);
            Assert.False(result1.IsFailure);
            Assert.False(result2.IsFailure);
        }

        [Fact]
        public async Task SaveFile_StreamAtNonZeroPosition_ResetsAndSavesAllContent()
        {
            // Arrange
            var content = "Full content to save";
            using var stream = CreateStreamFromString(content);

            var buffer = new byte[5];
            await stream.ReadExactlyAsync(buffer, 0, 5);

            // Act
            var result = await _fileHandler.SaveFile(stream);

            // Assert
            var filePath = Path.Combine(_testStoragePath, result.Value.ToString());
            var savedContent = await File.ReadAllTextAsync(filePath);
            Assert.Equal(content, savedContent);
        }

        [Fact]
        public async Task DeleteFile_ExistingFile_RemovesFileSuccessfully()
        {
            // Arrange
            var guid = await CreateTestFile("test content");
            var filePath = Path.Combine(_testStoragePath, guid.ToString());

            // Act
            var result = await _fileHandler.DeleteFile(guid.ToString());

            // Assert
            Assert.False(result.IsFailure);
            Assert.False(File.Exists(filePath));
        }

        [Fact]
        public async Task DeleteFile_NonExistingFile_ReturnsFailure()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid().ToString();

            // Act
            var result = await _fileHandler.DeleteFile(nonExistingId);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task GetFile_ExistingFile_ReturnsCorrectStream()
        {
            // Arrange
            var expectedContent = "File content for reading";
            var guid = await CreateTestFile(expectedContent);

            // Act
            var result = await _fileHandler.GetFile(guid.ToString());

            // Assert
            Assert.False(result.IsFailure);
            Assert.NotNull(result.Value);
            Assert.True(result.Value.CanRead);

            using var reader = new StreamReader(result.Value);
            var actualContent = await reader.ReadToEndAsync();
            Assert.Equal(expectedContent, actualContent);

            await result.Value.DisposeAsync();
        }

        [Fact]
        public async Task GetFile_NonExistingFile_ReturnsFailure()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid().ToString();

            // Act
            var result = await _fileHandler.GetFile(nonExistingId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains("File", result.Errors.Keys);

            Assert.Single(result.Errors["File"]);
            Assert.Equal("File not found", result.Errors["File"][0]);
        }

        [Fact]
        public async Task SaveAndGetFile_RoundTrip_PreservesContent()
        {
            // Arrange
            var originalContent = "Round trip test 🚀\nMultiline\nContent";
            using var saveStream = CreateStreamFromString(originalContent);

            // Act
            var saveResult = await _fileHandler.SaveFile(saveStream);
            var getResult = await _fileHandler.GetFile(saveResult.Value.ToString());

            // Assert
            using var reader = new StreamReader(getResult.Value);
            var retrievedContent = await reader.ReadToEndAsync();
            Assert.Equal(originalContent, retrievedContent);

            await getResult.Value.DisposeAsync();
        }

        [Fact]
        public Task Constructor_CreatesStorageDirectory()
        {
            // Assert
            Assert.True(Directory.Exists(_testStoragePath));
            return Task.CompletedTask;
        }

        private MemoryStream CreateStreamFromString(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(bytes);
        }

        private async Task<Guid> CreateTestFile(string content)
        {
            using var stream = CreateStreamFromString(content);
            var result = await _fileHandler.SaveFile(stream);
            return result.Value;
        }

        private class TestFileHandler : FileHandler
        {
            public TestFileHandler(string storagePath)
            {
                var field = typeof(FileHandler).GetField("StoragePath");
                field?.SetValue(this, storagePath);

                if (!Directory.Exists(storagePath))
                    Directory.CreateDirectory(storagePath);
            }
        }
    }
}