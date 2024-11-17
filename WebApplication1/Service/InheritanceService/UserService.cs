using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WebApplication1.Data;
using WebApplication1.Data.Authentication;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;

namespace WebApplication1.Service.InheritanceService
{
    public class UserService
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly ISendMailService _sendMailService;
        private readonly ILogger<UserService> logger;
        private readonly FacebookClientModel _facebookSettings;
        private readonly HttpClient _httpClient;

        public UserService(
            MyDbContext context,
            IUserRepository userRepository,
            IOptionsMonitor<AppSetting> optionsMonitor,
            IOptions<FacebookClientModel> facebookSettings,
            ILogger<UserService> _logger,
            ISendMailService sendMailService,
            HttpClient httpClient,
            IOptions<AppSetting> appSettings
        )
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _userRepository = userRepository;
            _sendMailService = sendMailService;
            _httpClient = httpClient;
            logger = _logger;

            _facebookSettings = facebookSettings.Value;
            _facebookSettings.FacebookClientId = (Environment.GetEnvironmentVariable("FACEBOOK_CLIENT_ID") ?? "").Length > 0
            ? Environment.GetEnvironmentVariable("FACEBOOK_CLIENT_ID") : _facebookSettings.FacebookClientId;
            _facebookSettings.FacebookSecretKey = (Environment.GetEnvironmentVariable("FACEBOOK_SECRET_KEY") ?? "").Length > 0
           ? Environment.GetEnvironmentVariable("FACEBOOK_SECRET_KEY") : _facebookSettings.FacebookSecretKey;
        }

        public async Task<Response> Validate(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.UserName == model.UserName);
            var hashInputPassword = ConvertToStringFromByteArray(
                HashPassword(model.Password, ConvertStringToByteArray(user.PasswordSalt))
            );

            if (user.LockoutEnd != null)
            {
                if (user.LockoutEnd > DateTime.UtcNow)
                {
                    var errorPhase = "";

                    TimeSpan difference = (user.LockoutEnd - DateTime.UtcNow).Value;

                    if (difference > TimeSpan.Zero)
                    {
                        // Kiểm tra chênh lệch thời gian
                        if (difference.TotalHours >= 1)
                        {
                            int hours = (int)difference.TotalHours;
                            int minutes = difference.Minutes;

                            return new Response
                            {
                                resultCd = 1,
                                MessageCode = $"C001-ex-{hours}-{minutes}",
                            };
                        }
                        else
                        {
                            int minutes = (int)difference.TotalMinutes;
                            Console.WriteLine($"{minutes} phút");

                            return new Response
                            {
                                resultCd = 1,
                                MessageCode = $"C001-ex-0-{minutes}",
                            };
                        }
                    }
                }
                else
                    user.LockoutEnd = null;
            }

            if (hashInputPassword != user.PasswordHash) //không đúng
            {
                user.AccessFailedCount++;

                if (user.AccessFailedCount >= 4 && user.LockoutEnd == null)
                {
                    var timeNow = DateTime.UtcNow.AddMinutes(15);
                    user.LockoutEnd = timeNow;
                    await _context.SaveChangesAsync();
                    return new Response { resultCd = 1, MessageCode = "C001-m4" };
                }

                if (user.AccessFailedCount >= 8 && user.LockoutEnd == null)
                {
                    var timeNow = DateTime.UtcNow.AddHours(2);
                    user.LockoutEnd = timeNow;
                    await _context.SaveChangesAsync();
                    return new Response { resultCd = 1, MessageCode = "C001-h2" };
                }

                await _context.SaveChangesAsync();
                return new Response { resultCd = 1, MessageCode = "C001" };
            }

            var generateSalt = GenerateSalt();
            var newHashInputPassword = ConvertToStringFromByteArray(
                HashPassword(model.Password, generateSalt)
            );
            user.PasswordHash = newHashInputPassword;
            user.PasswordSalt = ConvertToStringFromByteArray(generateSalt);
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;

            await _context.SaveChangesAsync();

            //cấp token
            var token = await GenerateToken(user);

            return new Response
            {
                resultCd = 0,
                MessageCode = "I000",
                Data = token
            };
        }

        public async Task<Response> CheckExist(String userName)
        {
            var user = _context.Users.FirstOrDefault(p => p.UserName == userName);
            if (user == null) //không đúng
            {
                return new Response { resultCd = 0, MessageCode = "I010" };
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C010" };
            }
        }

        public async Task<Response> CheckEmailExist(string email)
        {
            var user = _context.Users.SingleOrDefault(p => p.Email == email);
            if (user == null)
            {
                return new Response { resultCd = 0, MessageCode = "I010" };
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C010e" };
            }
        }

        public async Task<Response> CheckReclaimExist(string reclaimInfo)
        {
            var user = _context.Users.SingleOrDefault(p =>
                p.Email == reclaimInfo || p.UserName == reclaimInfo
            );
            if (user != null)
            {
                return new Response { resultCd = 0, MessageCode = "I010b" };
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C010n" };
            }
        }

        public async Task<Response> GoogleSignIn(UserModel model)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(
                model.GoogleToken
            );

            var userExist = _context.Users.Count(p => p.Email == model.Email);

            if (payload == null)
            {
                return new Response { resultCd = 1, MessageCode = "C008" };
            }

            if (userExist < 1) //không đúng
            {
                var userInsert = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = "None",
                    PasswordSalt = "None",
                    PhoneNumber = model.PhoneNumber,
                    Role = UserRole.User,
                    Address = model.Address,
                    FullName = model.FullName,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                };
                _context.Users.Add(userInsert);
                await _context.SaveChangesAsync();
            }

            var user = _context.Users.FirstOrDefault(p => p.Email == model.Email);
            var token = await GenerateToken(user);

            return new Response
            {
                resultCd = 0,
                MessageCode = "I000",
                Data = token
            };
        }

        public async Task<Response> FacebookSignIn(UserModel model)
        {
            Boolean payload = false;

            payload = await ValidateFaceBookTokenAsync(
                model.GoogleToken
            );

            var userExist = _context.Users.Count(p => p.Email == model.Email);

            if (!payload)
            {
                return new Response { resultCd = 1, MessageCode = "C008" };
            }

            if (model.Email == null || model.Email.Length <= 0)
            {
                return new Response { resultCd = 1, MessageCode = "C010fe" };
            }

            if (userExist < 1) //không đúng
            {
                var userInsert = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = "None",
                    PasswordSalt = "None",
                    PhoneNumber = model.PhoneNumber,
                    Role = UserRole.User,
                    Address = model.Address,
                    FullName = model.FullName,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                };
                _context.Users.Add(userInsert);
                await _context.SaveChangesAsync();
            }

            var user = _context.Users.FirstOrDefault(p => p.Email == model.Email);
            var token = await GenerateToken(user);

            return new Response
            {
                resultCd = 0,
                MessageCode = "I000",
                Data = token
            };
        }

        public async Task<Boolean> ValidateFaceBookTokenAsync(string accessToken)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<FacebookTokenDebugResponse>(
                                $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={_facebookSettings.FacebookClientId}|{_facebookSettings.FacebookSecretKey}");

                logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response.data));
                if (response == null || response.data == null || !response.data.is_valid)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<Response> SignIn(UserModel model)
        {
            var userExist = _context.Users.Count(p =>
                p.UserName == model.UserName || p.Email == model.Email
            );
            if (userExist < 1) //không đúng
            {
                var generateSalt = GenerateSalt();
                var hashInputPassword = ConvertToStringFromByteArray(
                    HashPassword(model.Password, generateSalt)
                );
                var userInsert = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = hashInputPassword,
                    PasswordSalt = ConvertToStringFromByteArray(generateSalt),
                    PhoneNumber = model.PhoneNumber,
                    Role = UserRole.New,
                    Address = model.Address,
                    FullName = model.FullName,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                };
                await _context.Users.AddAsync(userInsert);
                await _context.SaveChangesAsync();

                var userTokenSample = userInsert;
                userTokenSample.Role = model.Role ?? UserRole.User;

                var signInToken = await GenerateSignInToken(userTokenSample);

                var baseUrl = _appSettings.BaseUrl;

                var mailContent = new MailContent
                {
                    To = model.Email,
                    Subject = "Roxanne: Tạo tài khoản mới",
                    Body =
                        "<html>"
                        + "<head>"
                        + "<style>"
                        + "body { font-family: Arial, sans-serif; background-color: #f4f4f4; }"
                        + ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1); }"
                        + ".message { margin-bottom: 20px; }"
                        + ".message p { margin: 0; font-size: 16px; margin-bottom: 10px; }"
                        + ".btn { display: inline-block; background-color: #007bff; color: #fff !important; text-decoration: none; padding: 10px 20px; border-radius: 5px; }"
                        + "</style>"
                        + "</head>"
                        + "<body>"
                        + "<div class='container'>"
                        + "<div class='message'>"
                        + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin chào <strong>"
                        + model.FullName
                        + "</strong>.</p>"
                        + "<p style='font-size: 18px; margin-bottom: 10px;'>Chúng tôi đã nhận được yêu cầu tạo tài khoản của bạn, vui lòng truy cập:</p>"
                        + "<p><a class='btn' href='"
                        + _appSettings.BaseUrl
                        + "/api/User/ValidateSignIn?token="
                        + signInToken.AccessToken
                        + "'>Xác nhận tài khoản</a></p>"
                        + "</div>"
                        + "</div>"
                        + "</body>"
                        + "</html>"
                };

                var mailResult = await _sendMailService.SendMail(mailContent);

                if (mailResult.resultCd == 0)
                {
                    return new Response
                    {
                        resultCd = 0,
                        MessageCode = "I010a",
                        Data = GenerateSignInToken(userInsert)
                    };
                }
                else
                    return mailResult;
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C010" };
            }
        }

        public async Task<Response> Reclaim(UserReClaimModel model)
        {
            var userExist = _context.Users.FirstOrDefault(p =>
                p.UserName == model.ReclaimInfo || p.Email == model.ReclaimInfo
            );
            if (userExist != null)
            {
                var signInToken = await GenerateReclaimToken(userExist);
                var baseUrl = _appSettings.BaseUrl;
                var mailContent = new MailContent
                {
                    To = userExist.Email,
                    Subject = "Roxanne: Khôi phục tài khoản",
                    Body =
                        "<html>"
                        + "<head>"
                        + "<style>"
                        + "body { font-family: Arial, sans-serif; background-color: #f4f4f4; }"
                        + ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1); }"
                        + ".message { margin-bottom: 20px; }"
                        + ".message p { margin: 0; font-size: 16px; margin-bottom: 10px; }"
                        + ".btn { display: inline-block; background-color: #007bff; color: #fff !important; text-decoration: none; padding: 10px 20px; border-radius: 5px; }"
                        + "</style>"
                        + "</head>"
                        + "<body>"
                        + "<div class='container'>"
                        + "<div class='message'>"
                        + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin chào <strong>"
                        + userExist.FullName
                        + "</strong>.</p>"
                        + "<p style='font-size: 18px; margin-bottom: 10px;'>Chúng tôi đã nhận được yêu cầu khôi phục tài khoản của bạn, vui lòng truy cập:</p>"
                        + "<p><a class='btn' href='"
                        + _appSettings.BaseUrl
                        + "/api/User/ValidateReclaim?token="
                        + signInToken.AccessToken
                        + "'>Khôi phục tài khoản</a></p>"
                        + "</div>"
                        + "</div>"
                        + "</body>"
                        + "</html>"
                };

                var mailResult = await _sendMailService.SendMail(mailContent);

                if (mailResult.resultCd == 0)
                {
                    return new Response { resultCd = 0, MessageCode = "I010a" };
                }
                else
                    return mailResult;
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C010" };
            }
        }

        public async Task<Response> Update(UserUpdateModel model, string bearer_token)
        {
            Response checkResult = checkIfTokenFormIsValid(bearer_token);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var role = tokenInverification.FindFirstValue("Role");
            var enumValue = (UserRole)Enum.Parse(typeof(UserRole), role);

            var user = _context.Users.SingleOrDefault(p =>
                p.UserName == model.UserName
            );
            if (user == null)
            {
                return new Response { resultCd = 1, MessageCode = "C011", };
            }

            var isSelfUpdate = tokenInverification.FindFirstValue("Id") == user.Id.ToString();

            if (role == "AdminManager")
            {
                var response = await this._userRepository.UpdateInfo(
                    UserRole.AdminManager,
                    isSelfUpdate,
                    model
                );
                return response;
            }

            if (role == "Admin")
            {
                var response = await this._userRepository.UpdateInfo(
                    UserRole.Admin,
                    isSelfUpdate,
                    model
                );
                return response;
            }
            else if (role == "User")
            {
                var userName = tokenInverification.FindFirstValue("UserName");
                var updateModel = model;

                var response = await this._userRepository.UpdateInfo(
                    UserRole.User,
                    isSelfUpdate,
                    updateModel
                );
                return response;
            }

            return new Response { resultCd = 1, MessageCode = "C011", };
        }

        public async Task<Response> UpdatePassword(
            UserUpdatePasswordModel model,
            string bearer_token
        )
        {
            Response checkResult = checkIfTokenFormIsValid(bearer_token);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var role = tokenInverification.FindFirstValue("Role");

            if (role == "User" || role == "Admin" || role == "AdminManager")
            {
                var userName = tokenInverification.FindFirstValue("UserName");

                var generateSalt = GenerateSalt();
                var hashInputPassword = ConvertToStringFromByteArray(
                    HashPassword(model.NewPassword, generateSalt)
                );

                var updateModel = model;
                updateModel.UserName = userName;
                updateModel.NewPassword = hashInputPassword;
                updateModel.PasswordSalt = ConvertToStringFromByteArray(generateSalt);

                var entity = await this._userRepository.UpdatePassword(updateModel);
                return entity;
            }

            return new Response { resultCd = 1, MessageCode = "C012", };
        }

        public async Task<Response> ReclaimPassword(UserReclaimPasswordModel model)
        {
            var existToken = _context
                .ReqTemporaryTokens.Where(token =>
                    token.Token == model.ReclaimToken && token.Purpose == TokenPurpose.Reclaim
                )
                .FirstOrDefault();
            if (existToken == null)
                return new Response { resultCd = 1, MessageCode = "C008", };

            existToken.IsActivated = false;
            existToken.ClosedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            Response checkResult = checkIfTokenFormIsValid(model.ReclaimToken);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var role = tokenInverification.FindFirstValue("Role");

            if (role == "User" || role == "Admin" || role == "AdminManager")
            {
                var userName = tokenInverification.FindFirstValue("UserName");
                var generateSalt = GenerateSalt();
                var hashInputPassword = ConvertToStringFromByteArray(
                    HashPassword(model.NewPassword, generateSalt)
                );

                var updateModel = model;
                updateModel.UserName = userName;
                updateModel.NewPassword = hashInputPassword;
                updateModel.PasswordSalt = ConvertToStringFromByteArray(generateSalt);

                var entity = await this._userRepository.ReclaimPassword(updateModel);
                return entity;
            }

            return new Response { resultCd = 1, MessageCode = "C012", };
        }

        public Response GetUserList(
            string bearer_token,
            string? search,
            int type,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            Response checkResult = checkIfTokenFormIsValid(bearer_token);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var role = tokenInverification.FindFirstValue("Role");

            if (role == "Admin")
            {
                if (type >= 2)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C015",
                        Data = null,
                        count = 0
                    };
                }

                var entityList = this._userRepository.GetAll(
                    search,
                    type,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return entityList;
            }

            if (role == "AdminManager")
            {
                var entityList = this._userRepository.GetAll(
                    search,
                    type,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return entityList;
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C013", };
            }
        }

        public Response GetUser(Guid id, int type, string bearer_token)
        {
            Response checkResult = checkIfTokenFormIsValid(bearer_token);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var role = tokenInverification.FindFirstValue("Role");

            if (role == "Admin")
            {
                if (type >= 2)
                    return new Response { resultCd = 1, MessageCode = "C015", };

                var entityList = this._userRepository.getById(id, type);
                return entityList;
            }

            if (role == "AdminManager")
            {
                var entityList = this._userRepository.getById(id, type);
                return entityList;
            }
            else
            {
                return new Response { resultCd = 1, MessageCode = "C014", };
            }
        }

        public Response SelfGetUser(string bearer_token)
        {
            Response checkResult = checkIfTokenFormIsValid(bearer_token);
            if (checkResult.resultCd != 0)
                return checkResult;
            var tokenInverification = (ClaimsPrincipal)checkResult.Data;

            var id = tokenInverification.FindFirstValue("Id");
            var role = tokenInverification.FindFirstValue("Role");
            var enumValue = (UserRole)Enum.Parse(typeof(UserRole), role);

            var entity = this._userRepository.getById(new Guid(id), (int)enumValue);
            return entity;
        }

        private async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var permissions = getPermission(user.Role);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("Email", user.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Id", user.Id.ToString()),
                        //roles
                        new Claim("Role", user.Role.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(
                            "Permissions",
                            permissions != null
                                ? JsonSerializer.Serialize(permissions)
                                : string.Empty,
                            JsonClaimValueTypes.JsonArray
                        )
                    }
                ),
                Issuer = _appSettings.Issuer,
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            // Save to database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                TokenDescription = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssueDate = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddHours(1),
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel { AccessToken = accessToken, RefreshToken = refreshToken, };
        }

        public async Task<TokenModel> GenerateSignInToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var permissions = getPermission(user.Role);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Purpose", "signIn"),
                        //roles
                        new Claim("Role", user.Role.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(
                            "Permissions",
                            permissions != null
                                ? JsonSerializer.Serialize(permissions)
                                : string.Empty,
                            JsonClaimValueTypes.JsonArray
                        )
                    }
                ),
                Issuer = _appSettings.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);

            var reqToken = new ReqTemporaryToken
            {
                Token = accessToken,
                IsActivated = true,
                CreateDate = DateTime.UtcNow,
                Purpose = TokenPurpose.SignIn
            };

            await _context.ReqTemporaryTokens.AddAsync(reqToken);
            await _context.SaveChangesAsync();

            return new TokenModel { AccessToken = accessToken, RefreshToken = null, };
        }

        public async Task<bool> validateSignInToken(string bearerToken)
        {
            var checkResult = checkIfTokenFormIsValid(bearerToken);

            if (checkResult.resultCd == 0)
            {
                var existToken = _context
                    .ReqTemporaryTokens.Where(entity =>
                        entity.Token == bearerToken && entity.Purpose == TokenPurpose.SignIn
                    )
                    .OrderByDescending(entity => entity.CreateDate)
                    .FirstOrDefault();

                if (existToken == null)
                    return false;
                if (!existToken.IsActivated)
                    return false;

                var tokenInverification = (ClaimsPrincipal)checkResult.Data;
                var purpose = tokenInverification.FindFirstValue("Purpose");
                if (purpose == "signIn")
                {
                    var userName = tokenInverification.FindFirstValue("UserName");
                    var role = tokenInverification.FindFirstValue("Role");
                    var enumValue = (UserRole)Enum.Parse(typeof(UserRole), role);
                    var user = await _context.Users.SingleOrDefaultAsync(user =>
                        user.UserName == userName
                    );
                    if (user != null)
                    {
                        if (enumValue == UserRole.Admin)
                            user.Role = UserRole.AdminTemp;
                        else if (enumValue == UserRole.User)
                            user.Role = UserRole.User;

                        existToken.IsActivated = false;
                        await _context.SaveChangesAsync();

                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<TokenModel> GenerateReclaimToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Purpose", "reClaim"),
                        //roles
                        new Claim("Role", user.Role.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                    }
                ),
                Issuer = _appSettings.Issuer,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);

            var reqToken = new ReqTemporaryToken
            {
                Token = accessToken,
                IsActivated = true,
                CreateDate = DateTime.UtcNow,
                Purpose = TokenPurpose.Reclaim
            };

            await _context.ReqTemporaryTokens.AddAsync(reqToken);
            await _context.SaveChangesAsync();

            return new TokenModel { AccessToken = accessToken, RefreshToken = null, };
        }

        public async Task<bool> validateReclaimToken(string bearerToken)
        {
            var checkResult = checkIfTokenFormIsValid(bearerToken);
            if (checkResult.resultCd == 0)
            {
                var existToken = _context
                    .ReqTemporaryTokens.Where(entity =>
                        entity.Token == bearerToken && entity.Purpose == TokenPurpose.Reclaim
                    )
                    .OrderByDescending(entity => entity.CreateDate)
                    .FirstOrDefault();

                if (existToken == null)
                    return false;
                if (!existToken.IsActivated)
                    return false;

                var tokenInverification = (ClaimsPrincipal)checkResult.Data;
                var purpose = tokenInverification.FindFirstValue("Purpose");
                if (purpose == "reClaim")
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<TokenModel> GeneratePaymentToken(
            string email,
            string fullReceiptId,
            string fullServiceReceiptId
        )
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);

            if (user != null)
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("UserName", user.UserName),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Purpose", "payment"),
                            new Claim("FullReceiptId", fullReceiptId),
                            new Claim("FullServiceReceiptId", fullServiceReceiptId),
                            //roles
                            new Claim("Role", user.Role.ToString()),
                            new Claim(ClaimTypes.Role, user.Role.ToString()),
                        }
                    ),
                    Issuer = _appSettings.Issuer,
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secretKeyBytes),
                        SecurityAlgorithms.HmacSha512Signature
                    )
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                var accessToken = jwtTokenHandler.WriteToken(token);

                var reqToken = new ReqTemporaryToken
                {
                    Token = accessToken,
                    IsActivated = true,
                    CreateDate = DateTime.UtcNow,
                    Purpose = TokenPurpose.Payment
                };

                await _context.ReqTemporaryTokens.AddAsync(reqToken);
                await _context.SaveChangesAsync();

                return new TokenModel { AccessToken = accessToken, RefreshToken = null, };
            }
            else
                return new TokenModel { AccessToken = "", RefreshToken = null, };
        }

        public async Task<bool> validatePaymentToken(
            string bearerToken,
            string fullReceiptId,
            string fullServiceReceiptId
        )
        {
            var checkResult = checkIfTokenFormIsValid(bearerToken);
            if (checkResult.resultCd == 0)
            {
                var existToken = _context
                    .ReqTemporaryTokens.Where(entity =>
                        entity.Token == bearerToken && entity.Purpose == TokenPurpose.Payment
                    )
                    .OrderByDescending(entity => entity.CreateDate)
                    .FirstOrDefault();

                if (existToken == null)
                    return false;
                if (!existToken.IsActivated)
                    return false;

                var tokenInverification = (ClaimsPrincipal)checkResult.Data;
                var purpose = tokenInverification.FindFirstValue("Purpose");
                if (purpose == "payment")
                {
                    var fullReceiptIdInToken = tokenInverification.FindFirstValue("FullReceiptId");
                    var fullServiceReceiptIdInToken = tokenInverification.FindFirstValue(
                        "FullServiceReceiptId"
                    );

                    if (
                        fullReceiptId != fullReceiptIdInToken
                        || fullServiceReceiptId != fullServiceReceiptIdInToken
                    )
                    {
                        return false;
                    }

                    existToken.IsActivated = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        private string[] getPermission(UserRole role)
        {
            var permissions = new List<string>();
            if (role == UserRole.Admin)
            {
                //BOOK policy
                permissions.Add(PolicyTerm.CREATE_TOURISH_PLAN);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_PLAN);
                permissions.Add(PolicyTerm.DELETE_TOURISH_PLAN);

                permissions.Add(PolicyTerm.CREATE_TOURISH_CATEGORY);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_CATEGORY);
                permissions.Add(PolicyTerm.DELETE_TOURISH_CATEGORY);

                permissions.Add(PolicyTerm.CREATE_TOURISH_COMMENT);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_COMMENT);
                permissions.Add(PolicyTerm.DELETE_TOURISH_COMMENT);

                permissions.Add(PolicyTerm.CREATE_MOVING_CONTACT);
                permissions.Add(PolicyTerm.UPDATE_MOVING_CONTACT);
                permissions.Add(PolicyTerm.DELETE_MOVING_CONTACT);

                permissions.Add(PolicyTerm.CREATE_RESTHOUSE_CONTACT);
                permissions.Add(PolicyTerm.UPDATE_RESTHOUSE_CONTACT);
                permissions.Add(PolicyTerm.DELETE_RESTHOUSE_CONTACT);

                permissions.Add(PolicyTerm.CREATE_RESTAURANT);
                permissions.Add(PolicyTerm.UPDATE_RESTAURANT);
                permissions.Add(PolicyTerm.DELETE_RESTAURANT);

                permissions.Add(PolicyTerm.CREATE_NOTIFICATION);
                permissions.Add(PolicyTerm.UPDATE_NOTIFICATION);
                permissions.Add(PolicyTerm.DELETE_NOTIFICATION);

                permissions.Add(PolicyTerm.CREATE_RECEIPT);
                permissions.Add(PolicyTerm.UPDATE_RECEIPT);
                permissions.Add(PolicyTerm.DELETE_RECEIPT);

                permissions.Add(PolicyTerm.GET_USER_LIST);
                permissions.Add(PolicyTerm.SELF_GET_USER);
                permissions.Add(PolicyTerm.UPDATE_INFO_USER);
                permissions.Add(PolicyTerm.UPDATE_PASSWORD_USER);
            }

            if (role == UserRole.AdminManager)
            {
                //BOOK policy
                permissions.Add(PolicyTerm.CREATE_TOURISH_PLAN);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_PLAN);
                permissions.Add(PolicyTerm.DELETE_TOURISH_PLAN);

                permissions.Add(PolicyTerm.CREATE_TOURISH_CATEGORY);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_CATEGORY);
                permissions.Add(PolicyTerm.DELETE_TOURISH_CATEGORY);

                permissions.Add(PolicyTerm.CREATE_TOURISH_COMMENT);
                permissions.Add(PolicyTerm.UPDATE_TOURISH_COMMENT);
                permissions.Add(PolicyTerm.DELETE_TOURISH_COMMENT);

                permissions.Add(PolicyTerm.CREATE_MOVING_CONTACT);
                permissions.Add(PolicyTerm.UPDATE_MOVING_CONTACT);
                permissions.Add(PolicyTerm.DELETE_MOVING_CONTACT);

                permissions.Add(PolicyTerm.CREATE_RESTHOUSE_CONTACT);
                permissions.Add(PolicyTerm.UPDATE_RESTHOUSE_CONTACT);
                permissions.Add(PolicyTerm.DELETE_RESTHOUSE_CONTACT);

                permissions.Add(PolicyTerm.CREATE_RESTAURANT);
                permissions.Add(PolicyTerm.UPDATE_RESTAURANT);
                permissions.Add(PolicyTerm.DELETE_RESTAURANT);

                permissions.Add(PolicyTerm.CREATE_NOTIFICATION);
                permissions.Add(PolicyTerm.UPDATE_NOTIFICATION);
                permissions.Add(PolicyTerm.DELETE_NOTIFICATION);

                permissions.Add(PolicyTerm.CREATE_RECEIPT);
                permissions.Add(PolicyTerm.UPDATE_RECEIPT);
                permissions.Add(PolicyTerm.DELETE_RECEIPT);

                permissions.Add(PolicyTerm.GET_USER_LIST);
                permissions.Add(PolicyTerm.SELF_GET_USER);
                permissions.Add(PolicyTerm.UPDATE_INFO_USER);
                permissions.Add(PolicyTerm.UPDATE_PASSWORD_USER);
            }

            return permissions.ToArray();
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<Response> RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenValidateParam = new TokenValidationParameters
            {
                // Seft-supply token
                ValidateIssuer = true,
                ValidIssuer = _appSettings.Issuer,
                ValidateAudience = false,

                // Sign token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                // No check expired
                ValidateLifetime = false,
            };

            try
            {
                // Check 1: Check if accessToken's format is validated
                var tokenInverification = jwtTokenHandler.ValidateToken(
                    tokenModel.AccessToken,
                    tokenValidateParam,
                    out var validatedToken
                );

                // Check 2: Check algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase
                    );

                    if (!result)
                    {
                        return new Response
                        {
                            resultCd = 1,
                            MessageCode = "C002",
                            //// Wrong algorithm
                        };
                    }
                }

                // Check 3: Check expired
                var utcExpiredDate = long.Parse(
                    tokenInverification
                        .Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)
                        .Value
                );

                var expiredDate = ConvertUnixTimeToDateTime(utcExpiredDate);
                if (expiredDate > DateTime.UtcNow)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C003",
                        // Not expired yet
                    };
                }

                // Check 4: Check expired
                if (expiredDate < DateTime.UtcNow)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C004",
                        // expired
                    };
                }

                // Check 5: Check if refreshToken exist in db
                var existToken = _context.RefreshTokens.FirstOrDefault(token =>
                    token.TokenDescription == tokenModel.RefreshToken
                );

                if (existToken is null)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C005",
                        // Refresh token does not exist
                    };
                }

                // Check 6: Check if refreshToken is used or revoked
                if (existToken.IsUsed)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C006",
                        // Refresh token has been used
                    };
                }

                if (existToken.IsRevoked)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C007",
                        // Refresh token has been revoked
                    };
                }

                // Check 7: Check if accesstoken.id == JwtId in refreshtoken
                var jti = tokenInverification
                    .Claims.FirstOrDefault(token => token.Type == JwtRegisteredClaimNames.Jti)
                    .Value;
                if (existToken.JwtId != jti)
                {
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C008",
                        // Token doesnt match
                    };
                }

                // Update token
                existToken.IsRevoked = true;
                existToken.IsUsed = true;
                _context.Update(existToken);
                await _context.SaveChangesAsync();

                // Create new token
                var user = await _context.Users.SingleOrDefaultAsync(user =>
                    user.Id == existToken.Id
                );
                var token = await GenerateToken(user);

                return new Response
                {
                    resultCd = 0,
                    MessageCode = "I001",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C009",
                    Error = ex.Message
                };
            }
        }

        public Response checkIfTokenFormIsValid(string bearer_token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenValidateParam = new TokenValidationParameters
            {
                // Seft-supply token
                ValidateIssuer = true,
                ValidIssuer = _appSettings.Issuer,
                ValidateAudience = false,

                // Sign token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                // No check expired
                ValidateLifetime = false,
            };

            try
            {
                // Check 1: Check if accessToken's format is validated
                var tokenInverification = jwtTokenHandler.ValidateToken(
                    bearer_token,
                    tokenValidateParam,
                    out var validatedToken
                );

                // Check 2: Check algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase
                    );

                    if (!result)
                    {
                        return new Response
                        {
                            resultCd = 1,
                            MessageCode = "C002",
                            //// Wrong algorithm
                        };
                    }

                    if (tokenInverification == null)
                    {
                        return new Response { resultCd = 1, MessageCode = "C008", };
                    }
                }

                // Check 3: Check expired
                var expClaim = tokenInverification.Claims.FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Exp
                );
                logger.LogInformation(expClaim.Value);
                if (expClaim != null)
                {
                    var utcExpiredDate = long.Parse(expClaim.Value);
                    logger.LogInformation(utcExpiredDate.ToString());

                    var expiredDate = ConvertUnixTimeToDateTime(utcExpiredDate);

                    logger.LogInformation(expiredDate.ToString());
                    // Check 4: Check expired
                    if (expiredDate < DateTime.UtcNow)
                    {
                        logger.LogInformation(DateTime.UtcNow.ToString());
                        logger.LogInformation("Failed time");
                        return new Response
                        {
                            resultCd = 1,
                            MessageCode = "C004e",
                            // expired
                        };
                    }
                }

                return new Response { resultCd = 0, Data = tokenInverification };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C009",
                    Error = ex.Message
                };
            }
        }

        public Response getUserByEmail(string email)
        {
            try
            {
                return _userRepository.getByEmail(email);
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }

        public Response getByName(String userName, int? type)
        {
            try
            {
                return _userRepository.getByName(userName, type);
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpiredDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpiredDate).ToUniversalTime();
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // You can adjust the size of the salt according to your needs
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Concatenate the password and salt
                byte[] combinedBytes = Encoding.UTF8.GetBytes(
                    password + Convert.ToBase64String(salt)
                );

                // Compute the hash
                return sha256.ComputeHash(combinedBytes);
            }
        }

        private byte[] ConvertStringToByteArray(string saltString)
        {
            return Convert.FromBase64String(saltString);
        }

        private string ConvertToStringFromByteArray(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }
    }
}
