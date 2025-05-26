using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Models;
using ZealandZooCase.Data;
using ZealandZooCase.Pages.AccountPages;

namespace UnitTestsZealand
{
    /// <summary>
    /// Unit tests for the Account Registration functionality
    /// Tests the OnPost method of RegisterModel to ensure proper user registration logic
    /// </summary>
    [TestClass]
    public class AccountRegistrationTests
    {
        // Test infrastructure fields
        private DbContextOptions<ZealandDBContext> _dbContextOptions;
        private ZealandDBContext _context;
        private ZealandZooCase.Pages.AccountPages.RegisterModel _pageModel;

        /// <summary>
        /// Sets up test environment before each test method runs
        /// Creates a fresh in-memory database and page model instance for isolation
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create in-memory database with unique name to ensure test isolation
            // Each test gets a completely fresh database instance
            _dbContextOptions = new DbContextOptionsBuilder<ZealandDBContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            // Initialize database context and page model for testing
            _context = new ZealandDBContext(_dbContextOptions);
            _pageModel = new RegisterModel(_context);
        }

        /// <summary>
        /// Cleanup after each test method completes
        /// Ensures no data persists between tests and prevents memory leaks
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Remove the in-memory database and dispose resources
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        /// <summary>
        /// Tests the happy path: valid user registration with unique username and email
        /// Should successfully create user and redirect to login page
        /// </summary>
        [TestMethod]
        public void OnPost_ValidUser_RedirectsToLoginPage()
        {
            // Arrange - Set up test data with unique username and email
            _pageModel.Username = "newuser";
            _pageModel.Email = "newuser@example.com";
            _pageModel.Password = "password123";

            // Act - Execute the method under test
            var result = _pageModel.OnPost();

            // Assert - Verify the expected behavior occurred
            // 1. Should return redirect result to login page
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectResult = (RedirectToPageResult)result;
            Assert.AreEqual("/AccountPages/LoginPage", redirectResult.PageName);

            // 2. Verify user was actually saved to database with correct data
            var addedUser = _context.Users.FirstOrDefault(u => u.UserName == "newuser");
            Assert.IsNotNull(addedUser, "User should be added to database");
            Assert.AreEqual("newuser@example.com", addedUser.UserEmail);
            Assert.AreEqual("password123", addedUser.UserPassword);
        }

        /// <summary>
        /// Tests username uniqueness validation
        /// When username already exists, should return error and not create duplicate user
        /// </summary>
        [TestMethod]
        public void OnPost_ExistingUsername_ReturnsPageWithError()
        {
            // Arrange - Create existing user with specific username
            var existingUser = new User
            {
                UserName = "existinguser",
                UserEmail = "existing@example.com",
                UserPassword = "password"
            };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            // Try to register with same username but different email
            _pageModel.Username = "existinguser"; // Same username
            _pageModel.Email = "newemail@example.com"; // Different email
            _pageModel.Password = "newpassword";

            // Act - Attempt registration
            var result = _pageModel.OnPost();

            // Assert - Should return page with error message, not redirect
            Assert.IsInstanceOfType(result, typeof(PageResult), "Should return Page() when username exists");
            Assert.AreEqual("Username already exists.", _pageModel.ErrorMessage);

            // Verify no duplicate user was added (should still be only 1)
            var userCount = _context.Users.Count(u => u.UserName == "existinguser");
            Assert.AreEqual(1, userCount, "Should not create duplicate user");
        }

        /// <summary>
        /// Tests email uniqueness validation
        /// When email already exists, should return error and not create duplicate user
        /// </summary>
        [TestMethod]
        public void OnPost_ExistingEmail_ReturnsPageWithError()
        {
            // Arrange - Create existing user with specific email
            var existingUser = new User
            {
                UserName = "existinguser",
                UserEmail = "existing@example.com",
                UserPassword = "password"
            };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            // Try to register with different username but same email
            _pageModel.Username = "newuser"; // Different username
            _pageModel.Email = "existing@example.com"; // Same email
            _pageModel.Password = "newpassword";

            // Act - Attempt registration
            var result = _pageModel.OnPost();

            // Assert - Should return page with Danish error message
            Assert.IsInstanceOfType(result, typeof(PageResult), "Should return Page() when email exists");
            Assert.AreEqual("Email is Already connected to another account", _pageModel.ErrorMessage);

            // Verify no duplicate user was added (should still be only 1)
            var userCount = _context.Users.Count(u => u.UserEmail == "existing@example.com");
            Assert.AreEqual(1, userCount, "Should not create duplicate user");
        }

        /// <summary>
        /// Tests priority of validation checks when both username and email exist
        /// Should return username error first due to if-else logic structure
        /// </summary>
        [TestMethod]
        public void OnPost_BothUsernameAndEmailExist_ReturnsUsernameError()
        {
            // Arrange - Create two existing users: one with target username, one with target email
            var existingUser1 = new User
            {
                UserName = "existinguser",
                UserEmail = "other@example.com",
                UserPassword = "password"
            };
            var existingUser2 = new User
            {
                UserName = "otheruser",
                UserEmail = "existing@example.com",
                UserPassword = "password"
            };
            _context.Users.AddRange(existingUser1, existingUser2);
            _context.SaveChanges();

            // Try to register with both existing username AND existing email
            _pageModel.Username = "existinguser"; // This username exists
            _pageModel.Email = "existing@example.com"; // This email also exists
            _pageModel.Password = "newpassword";

            // Act - Attempt registration
            var result = _pageModel.OnPost();

            // Assert - Should return username error first (due to if-else logic order)
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Username already exists.", _pageModel.ErrorMessage,
                "Should return username error first when both username and email exist");
        }

        /// <summary>
        /// Tests user creation when database is completely empty
        /// Verifies the basic functionality works with no existing data
        /// </summary>
        [TestMethod]
        public void OnPost_EmptyDatabase_CreatesFirstUser()
        {
            // Arrange - Database is already empty from Setup()
            _pageModel.Username = "firstuser";
            _pageModel.Email = "first@example.com";
            _pageModel.Password = "firstpassword";

            // Act - Create first user
            var result = _pageModel.OnPost();

            // Assert - Should successfully create user and redirect
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult), "Should redirect when creating first user");
            Assert.AreEqual(1, _context.Users.Count(), "Database should contain exactly one user");

            // Verify the created user has correct data
            var createdUser = _context.Users.First();
            Assert.AreEqual("firstuser", createdUser.UserName);
            Assert.AreEqual("first@example.com", createdUser.UserEmail);
            Assert.AreEqual("firstpassword", createdUser.UserPassword);
        }

        /// <summary>
        /// Tests that username validation is case-insensitive
        /// Should prevent registration when username exists with different casing
        /// This enforces better UX by treating "TestUser" and "testuser" as the same
        /// </summary>
        [TestMethod]
        public void OnPost_CaseInsensitiveUsername_ReturnsError()
        {
            // Arrange - Create user with specific case
            var existingUser = new User
            {
                UserName = "TestUser", // Mixed case
                UserEmail = "test@example.com",
                UserPassword = "password"
            };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            // Try to register with same username but different case
            _pageModel.Username = "testuser"; // All lowercase - should be treated as same username
            _pageModel.Email = "newemail@example.com";
            _pageModel.Password = "newpassword";

            // Act - Attempt registration
            var result = _pageModel.OnPost();

            // Assert - Should reject registration due to case-insensitive username match
            // This test will ONLY PASS if your method implements case-insensitive comparison
            Assert.IsInstanceOfType(result, typeof(PageResult),
                "Should return Page() when username exists with different casing");
            Assert.AreEqual("Username already exists.", _pageModel.ErrorMessage,
                "Should show username exists error for case-insensitive match");

            // Verify no duplicate user was added
            var userCount = _context.Users.Count();
            Assert.AreEqual(1, userCount, "Should not create duplicate user with different casing");
        }

        /// <summary>
        /// Tests that usernames with special characters are accepted
        /// Verifies system doesn't reject valid special characters in usernames
        /// </summary>
        [TestMethod]
        public void OnPost_SpecialCharactersInUsername_CreatesUser()
        {
            // Arrange - Username with various special characters
            _pageModel.Username = "user@123_test"; // Contains @, numbers, underscore
            _pageModel.Email = "special@example.com";
            _pageModel.Password = "password123";

            // Act - Attempt registration
            var result = _pageModel.OnPost();

            // Assert - Should accept special characters and create user
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult),
                "Should accept usernames with special characters");

            var createdUser = _context.Users.FirstOrDefault(u => u.UserName == "user@123_test");
            Assert.IsNotNull(createdUser, "User with special characters should be created successfully");
        }
    }
}